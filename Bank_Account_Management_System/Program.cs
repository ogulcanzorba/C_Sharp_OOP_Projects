// Bank Account Management System
using System;
using System.Collections.Generic;
using System.Linq;
namespace BankAccountManagement
{
    public class BankAccount
    {
        // Static list to track all used account numbers
        private static HashSet<string> _usedAccountNumbers = new HashSet<string>();
        private static Random _random = new Random();

        public string AccountNumber { get; private set; } //only can be used by the class which contains the property
        public string AccountHolderName { get; set; }
        public decimal Balance { get; protected set; } // only derived classes can set

        public BankAccount(string holder)
        {
            AccountNumber = GenerateUniqueAccountNumber();
            Balance = 0;
            if (string.IsNullOrWhiteSpace(holder)) throw new ArgumentException("Account name required");
            AccountHolderName = holder;
        }
        private string GenerateUniqueAccountNumber()
        {
            string accountnumber;
            int attempts = 0;
            const int maxAttempts = 1000; //prevent infinite loop
            do
            {
                int number = _random.Next(0, 100000);
                accountnumber = number.ToString("D5"); // Format as 5 digits with leading zeros
                attempts++;
            } while(_usedAccountNumbers.Contains(accountnumber) && attempts < maxAttempts);
            if (attempts >= maxAttempts)
                throw new InvalidOperationException("Unable to generate unique account number. All numbers may be in use.");
            _usedAccountNumbers.Add(accountnumber);
            return accountnumber;
        }

        public virtual void Deposit(decimal amount, Bank bank) 
        { 
            if(amount <= 0)
            {
                Console.WriteLine("Deposit amount must be positive.");
                return;
            }
            else
            {
                Balance += amount;
                Console.WriteLine($"Deposited ${amount:F2}. New balance: ${Balance:F2}"); // 2 digits after '.' eg: $123.56
                
                // Record transaction
                bank.RecordTransaction("Deposit", amount, AccountNumber);
            }
        }
        public virtual void Withdraw(decimal amount, Bank bank) 
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be positive.");
                return;
            }
            
            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }
            
            Balance -= amount;
            Console.WriteLine($"Withdrawn ${amount:F2}. New balance: ${Balance:F2}");
            
            // Record transaction
            bank.RecordTransaction("Withdraw", amount, AccountNumber);
        }
        public virtual void Transfer(BankAccount toAccount, decimal amount, Bank bank) 
        {
            if (amount <= 0)
            {
                Console.WriteLine("Transfer amount must be positive.");
                return;
            }
            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds for transfer.");
                return;
            }
            if (toAccount == null)
            {
                Console.WriteLine("Invalid destination account.");
                return;
            }
            Balance -= amount;
            toAccount.Balance += amount;
            Console.WriteLine($"Transferred ${amount:F2} to account {toAccount.AccountNumber}");                                   
            
            // Record transaction
            bank.RecordTransaction("Transfer", amount, AccountNumber, toAccount.AccountNumber);
        }
        public virtual void PrintAccountSummary() 
        {
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Account Holder: {AccountHolderName}");
            Console.WriteLine($"Balance: ${Balance:F2}");
        }
    }

    public class SavingsAccount : BankAccount
    {
        public decimal InterestRate { get; set; }

        public SavingsAccount(string holder, decimal rate) : base(holder)
        {
            InterestRate = rate;
        }

        public void ApplyInterest(Bank bank)
        {
            decimal interest = Balance * (InterestRate / 100);
            Balance += interest;
            Console.WriteLine($"Interest applied: ${interest:F2}. New balance: ${Balance:F2}");
            
            // Record interest transaction
            bank.RecordTransaction("Interest", interest, AccountNumber);
        }

        public override void PrintAccountSummary()
        {
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Account Holder: {AccountHolderName}");
            Console.WriteLine($"Balance: ${Balance:F2}");
            Console.WriteLine($"Interest Rate: {InterestRate:F2}%");
            Console.WriteLine($"Account Type: Savings");
        }
    }

    public class CheckingAccount : BankAccount
    {
        public decimal OverdraftLimit { get; set; }

        public CheckingAccount(string holder, decimal limit) : base(holder)
        {
            OverdraftLimit = limit;
        }

        public override void Withdraw(decimal amount, Bank bank)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be positive.");
                return;
            }
            
            decimal availableBalance = Balance + OverdraftLimit;
            if (amount > availableBalance)
            {
                Console.WriteLine($"Insufficient funds. Available balance (including overdraft): ${availableBalance:F2}");
                return;
            }
            
            Balance -= amount;
            Console.WriteLine($"Withdrawn ${amount:F2}. New balance: ${Balance:F2}");
            
            if (Balance < 0)
            {
                Console.WriteLine($"Warning: Account is overdrawn by ${Math.Abs(Balance):F2}");
            }
            
            // Record transaction
            bank.RecordTransaction("Withdraw", amount, AccountNumber);
        }

        public override void PrintAccountSummary()
        {
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Account Holder: {AccountHolderName}");
            Console.WriteLine($"Balance: ${Balance:F2}");
            Console.WriteLine($"Overdraft Limit: ${OverdraftLimit:F2}");
            Console.WriteLine($"Account Type: Checking");
            
            if (Balance < 0)
            {
                Console.WriteLine($"Overdrawn by: ${Math.Abs(Balance):F2}");
            }
        }
    }

    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // Deposit, Withdraw, Transfer
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }

        public Transaction(DateTime date, decimal amount, string type, string fromAccount, string toAccount)
        {
            Date = date;
            Amount = amount;
            Type = type;
            FromAccount = fromAccount;
            ToAccount = toAccount;
        }
    }

    public class Bank
    {
        public List<BankAccount> Accounts { get; set; } = new List<BankAccount>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public void CreateAccount(BankAccount account) 
        {
            Accounts.Add(account);
            Console.WriteLine($"Account {account.AccountNumber} created successfully!");            
        }
        public BankAccount FindAccount(string accountNumber)
        {
            return Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber); // FirstOrDefault is a LINQ (Language Integrated Query) method and also a=> a is a lambda method
        }

        // Add transaction recording method
        public void RecordTransaction(string type, decimal amount, string fromAccount, string toAccount = "")
        {
            var transaction = new Transaction(DateTime.Now, amount, type, fromAccount, toAccount);
            Transactions.Add(transaction);
        }

        public void DisplayAllAccounts() 
        {
            if (Accounts.Count == 0)
            {
                Console.WriteLine("No accounts found.");
                return;
            }
            Console.WriteLine("\n--- All Accounts ---");
            foreach (var account in Accounts)
            {
                account.PrintAccountSummary();
                Console.WriteLine("---");
            }            
        }
        public void DisplayTransactionHistory()
        {
            if (Transactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            Console.WriteLine("\n--- Transaction History ---");
            foreach (var transaction in Transactions)
            {
                Console.WriteLine($"Date: {transaction.Date:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Type: {transaction.Type}");
                Console.WriteLine($"Amount: ${transaction.Amount:F2}");
                Console.WriteLine($"From: {transaction.FromAccount}");
                if (!string.IsNullOrEmpty(transaction.ToAccount))
                    Console.WriteLine($"To: {transaction.ToAccount}");
                Console.WriteLine("---");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n--- Bank Menu ---");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Transfer Money");
                Console.WriteLine("5. View Account Summary");
                Console.WriteLine("6. View All Accounts");
                Console.WriteLine("7. View Transaction History");
                Console.WriteLine("8. Apply Interest (Savings)");
                Console.WriteLine("9. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter account holder name: ");
                        string holdername = Console.ReadLine() ?? "";
                        
                        Console.WriteLine("Select account type:");
                        Console.WriteLine("1. Regular Account");
                        Console.WriteLine("2. Savings Account");
                        Console.WriteLine("3. Checking Account");
                        Console.Write("Choose account type: ");
                        string accountType = Console.ReadLine();
                        
                        BankAccount newaccount;
                        switch (accountType)
                        {
                            case "1":
                                newaccount = new BankAccount(holdername);
                                break;
                            case "2":
                                Console.Write("Enter interest rate (%): ");
                                if (decimal.TryParse(Console.ReadLine(), out decimal interestRate))
                                {
                                    newaccount = new SavingsAccount(holdername, interestRate);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid interest rate. Creating regular account.");
                                    newaccount = new BankAccount(holdername);
                                }
                                break;
                            case "3":
                                Console.Write("Enter overdraft limit: ");
                                if (decimal.TryParse(Console.ReadLine(), out decimal overdraftLimit))
                                {
                                    newaccount = new CheckingAccount(holdername, overdraftLimit);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid overdraft limit. Creating regular account.");
                                    newaccount = new BankAccount(holdername);
                                }
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Creating regular account.");
                                newaccount = new BankAccount(holdername);
                                break;
                        }
                        
                        bank.CreateAccount(newaccount);
                        break;

                    case "2":
                        Console.Write("Enter account number: ");
                        string depositAccount = Console.ReadLine() ?? "";
                        var depositAcc = bank.FindAccount(depositAccount);
                        
                        if (depositAcc != null)
                        {
                            Console.Write("Enter amount to deposit: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                            {
                                depositAcc.Deposit(depositAmount, bank);
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case "3":
                        Console.Write("Enter account number: ");
                        string withdrawAccount = Console.ReadLine() ?? "";
                        var withdrawAcc = bank.FindAccount(withdrawAccount);
                        
                        if (withdrawAcc != null)
                        {
                            Console.Write("Enter amount to withdraw: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                            {
                                withdrawAcc.Withdraw(withdrawAmount, bank);
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case "4":
                        Console.Write("Enter source account number: ");
                        string fromAccount = Console.ReadLine() ?? "";
                        var fromAcc = bank.FindAccount(fromAccount);
                        
                        Console.Write("Enter destination account number: ");
                        string toAccount = Console.ReadLine() ?? "";
                        var toAcc = bank.FindAccount(toAccount);
                        
                        if (fromAcc != null && toAcc != null)
                        {
                            Console.Write("Enter amount to transfer: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount))
                            {
                                fromAcc.Transfer(toAcc, transferAmount, bank);
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("One or both accounts not found.");
                        }
                        break;

                    case "5":
                        Console.Write("Enter account number: ");
                        string summaryAccount = Console.ReadLine() ?? "";
                        var summaryAcc = bank.FindAccount(summaryAccount);
                        
                        if (summaryAcc != null)
                        {
                            summaryAcc.PrintAccountSummary();
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                        break;

                    case "6":
                        bank.DisplayAllAccounts();
                        break;


                    case "7":
                        bank.DisplayTransactionHistory();
                        break;

                    case "8":
                        Console.Write("Enter savings account number: ");
                        string interestAccount = Console.ReadLine() ?? "";
                        var interestAcc = bank.FindAccount(interestAccount);
                        
                        if (interestAcc is SavingsAccount savingsAcc)
                        {
                            savingsAcc.ApplyInterest(bank);
                        }
                        else
                        {
                            Console.WriteLine("Account not found or not a savings account.");
                        }
                        break;
                        
                    case "9":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
