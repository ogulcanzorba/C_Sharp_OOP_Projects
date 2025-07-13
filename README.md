# C# Mini OOP Projects ��

Welcome to the **C# Object-Oriented Programming Practice Projects**! This repository contains two beginner-friendly projects designed to help you master OOP concepts in C#.

## 📚 Projects Overview

### 1. 🏦 Bank Account Management System
A console-based banking application that demonstrates:
- **Inheritance** (Regular, Savings, and Checking accounts)
- **Encapsulation** (Private properties, protected access)
- **Polymorphism** (Method overriding)
- **Static members** (Account number generation)
- **Collections** (Lists for accounts and transactions)

### 2. �� Library Management System
A library management application that showcases:
- **Class relationships** (Library, Member, Book)
- **Data validation** (Input checking)
- **Collections management** (Books and Members)
- **User interaction** (Menu-driven interface)

## 🎯 Learning Objectives

By completing these projects, you will practice:

### **OOP Concepts**
- ✅ **Classes and Objects**
- ✅ **Inheritance** (Base and derived classes)
- ✅ **Encapsulation** (Access modifiers)
- ✅ **Polymorphism** (Method overriding)
- ✅ **Abstraction** (Abstracting complex operations)

### **C# Features**
- ✅ **Properties** (Auto-implemented and custom)
- ✅ **Constructors** (Default and parameterized)
- ✅ **Static members** (Static methods and fields)
- ✅ **Collections** (List<T>, HashSet<T>)
- ✅ **Exception handling** (Try-catch blocks)
- ✅ **LINQ** (FirstOrDefault, Where, etc.)

## 🚀 Getting Started

### **Prerequisites**
- Visual Studio 2022 or Visual Studio Code
- .NET 6.0 or later
- Basic C# knowledge

### **Setup Instructions**
1. Clone this repository
2. Open the solution file `C# Mini OOP Projects.sln`
3. Build the solution
4. Run either project from the console

## 📋 Project Templates & Tasks

### **🏦 Bank Account Management System**

#### **Template Features:**
- Basic account creation with unique 5-digit account numbers
- Deposit and withdrawal functionality
- Account balance tracking
- Simple console menu interface

#### **Your Tasks:**

**Phase 1: Basic Operations**
- [ ] Implement the `Deposit()` method
- [ ] Implement the `Withdraw()` method  
- [ ] Implement the `Transfer()` method
- [ ] Add proper error handling for invalid amounts

**Phase 2: Account Types**
- [ ] Complete the `SavingsAccount` class with interest calculation
- [ ] Complete the `CheckingAccount` class with overdraft protection
- [ ] Add account type selection in the menu
- [ ] Implement different withdrawal rules for each account type

**Phase 3: Transaction Recording**
- [ ] Create a `Transaction` class to record all operations
- [ ] Add transaction history functionality
- [ ] Record deposits, withdrawals, and transfers
- [ ] Display transaction history with timestamps

**Phase 4: Enhanced Features**
- [ ] Add account search by holder name
- [ ] Implement account deletion
- [ ] Add data persistence (save/load from file)
- [ ] Add input validation and better error messages

#### **Advanced Challenges:**
- [ ] Implement interest calculation for savings accounts
- [ ] Add transaction fees for checking accounts
- [ ] Create account statements with detailed history
- [ ] Add PIN/password protection for accounts

---

### **�� Library Management System**

#### **Template Features:**
- Basic book and member management
- Book checkout and return functionality
- Member registration system
- Simple library operations

#### **Your Tasks:**

**Phase 1: Core Functionality**
- [ ] Complete the member login system
- [ ] Implement book borrowing functionality
- [ ] Add book return functionality
- [ ] Create member menu with all options

**Phase 2: Enhanced Features**
- [ ] Add book categories/genres
- [ ] Implement due date tracking
- [ ] Add late fee calculation
- [ ] Create book search functionality

**Phase 3: Data Management**
- [ ] Add book inventory management
- [ ] Implement member account status
- [ ] Add book reservation system
- [ ] Create overdue book notifications

**Phase 4: Advanced Features**
- [ ] Add librarian/admin role
- [ ] Implement book recommendations
- [ ] Add book ratings and reviews
- [ ] Create library statistics and reports

#### **Advanced Challenges:**
- [ ] Implement a book recommendation algorithm
- [ ] Add multiple library branches
- [ ] Create an online catalog system
- [ ] Add book availability notifications

## 🎓 Learning Tips

### **For Beginners:**
1. **Start Simple**: Begin with basic functionality before adding complex features
2. **Test Often**: Run your code frequently to catch errors early
3. **Use Comments**: Document your code to understand your logic
4. **Break Down Problems**: Divide complex tasks into smaller, manageable pieces

### **For Intermediate Learners:**
1. **Focus on Design**: Think about class relationships and responsibilities
2. **Practice SOLID Principles**: Single responsibility, Open/closed, etc.
3. **Add Error Handling**: Implement proper exception handling
4. **Optimize Performance**: Consider efficiency in your solutions

### **For Advanced Learners:**
1. **Implement Design Patterns**: Use Factory, Singleton, or Observer patterns
2. **Add Unit Tests**: Write tests for your classes and methods
3. **Use Advanced C# Features**: Implement interfaces, delegates, events
4. **Add Data Persistence**: Implement file I/O or database connectivity

## 🔧 Code Structure Examples

### **Property Examples:**
```csharp
// Auto-implemented property
public string Name { get; set; }

// Read-only property
public string ID { get; }

// Property with custom logic
private decimal _balance;
public decimal Balance 
{ 
    get { return _balance; }
    set 
    { 
        if (value < 0) throw new ArgumentException("Balance cannot be negative");
        _balance = value; 
    }
}
```

### **Inheritance Example:**
```csharp
public class BankAccount
{
    public virtual void Withdraw(decimal amount) { /* base logic */ }
}

public class CheckingAccount : BankAccount
{
    public override void Withdraw(decimal amount) 
    { 
        // Override with overdraft logic
    }
}
```

## 🎓 Success Metrics

You've successfully learned OOP when you can:
- ✅ Create classes with proper encapsulation
- ✅ Implement inheritance hierarchies
- ✅ Use polymorphism effectively
- ✅ Design clean, maintainable code
- ✅ Handle errors gracefully
- ✅ Extend functionality without breaking existing code

## 🎉 Happy Coding!

Remember: **Practice makes perfect!** Start with the basic tasks and gradually work your way up to the advanced challenges. Each project builds upon the previous concepts, so take your time to understand each concept before moving forward.

**Good luck with your OOP journey! ��** 