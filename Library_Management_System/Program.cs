// Library Management Project
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LibraryManagementSystem
{
    public class Book
    {
        public string Title;
        public string Author;
        public string ISBN;
        public bool IsCheckedOut { get; set; }
        public Book(string title, string author, string isbn) //Constructor
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title required");
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author required");
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("ISBN required");

            Title = title;
            Author = author;
            ISBN = isbn;
        }
        public void CheckOut()
        {
            if (!IsCheckedOut)
            {
                IsCheckedOut = true;
                Console.WriteLine($"'{Title}' has been checked out.");
            }
            else
            {
                Console.WriteLine($"'{Title}' is already checked out.");
            }
        }
        public void Return()
        {
            if (IsCheckedOut)
            {
                IsCheckedOut = false;
                Console.WriteLine($"'{Title}' has been returned.");
            }
            else
            {
                Console.WriteLine($"'{Title}' was not checked out.");
            }
        }
    }
    public class Member
    {
        public string Name;
        public string MemberID;
        public Member(string name, string memberid)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Title required");
            if (string.IsNullOrWhiteSpace(memberid)) throw new ArgumentException("MemberID required");

            Name = name;
            MemberID = memberid;
        }
        public List<Book> BorrowedBooks { get; set; } = new List<Book>();
        public void BorrowBook(Book book)
        {
            if (!book.IsCheckedOut)
            {
                BorrowedBooks.Add(book);
                book.CheckOut();
            }
            else
            {
                Console.WriteLine($"'{book.Title}' is not available.");
            }
        }

        public void ReturnBook(Book book)
        {
            if (BorrowedBooks.Contains(book))
            {
                BorrowedBooks.Remove(book);
                book.Return();
            }
            else
            {
                Console.WriteLine($"'{book.Title}' is not borrowed by {Name}.");
            }
        }
    }
    public class Library
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public List<Member> Members { get; set; } = new List<Member>();

        public void AddBook(Book book)
        {
            Books.Add(book);
            Console.WriteLine($"Book '{book.Title}' added to library.");
        }

        public void RegisterMember(Member member)
        {
            if (Members.Any(m => m.MemberID == member.MemberID))
            {
                Console.WriteLine($"Member with ID {member.MemberID} already exists.");
            }
            else
            {
                Members.Add(member);
                Console.WriteLine($"Member '{member.Name}' registered successfully.");
            }
        }

        public void DisplayAvailableBooks()
        {
            Console.WriteLine("--- Available Books ---");
            foreach (var book in Books.Where(b => !b.IsCheckedOut))
            {
                Console.WriteLine($"- {book.Title} by {book.Author}");
            }
        }

        public Book? FindBookByTitle(string title)
        {
            return Books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public Member? FindMemberById(string id)
        {
            return Members.FirstOrDefault(m => m.MemberID == id);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var library = new Library();
            Member? currentMember = null;

            while (true)
            {
                Console.WriteLine("\n--- Library System ---");
                Console.WriteLine("1. Register Member");
                Console.WriteLine("2. Log In");
                Console.WriteLine("3. Add Book");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                string input = Console.ReadLine() ?? "";

                switch (input)
                {
                    case "1":
                        RegisterMember(library);
                        break;
                    case "2":
                        currentMember = LogIn(library);
                        if (currentMember != null)
                            MemberMenu(currentMember, library);
                        break;
                    case "3":
                        AddBookToLibrary(library);
                        break;
                    case "4":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
        static void RegisterMember(Library library)
        {
            Console.WriteLine("\n-- Register New Member --");
            Console.Write("Enter name (or type 'exit' to cancel): ");
            string name = Console.ReadLine() ?? "";
            if (name.ToLower() == "exit") return;

            string id;
            while (true)
            {
                Console.Write("Enter Member ID (3-digit number, or 'exit' to cancel): ");
                id = Console.ReadLine() ?? "";
                if (id.ToLower() == "exit") return;

                if (id.Length == 3 && id.All(char.IsDigit)) break;
                Console.WriteLine("Invalid Member ID.");
            }

            try
            {
                var member = new Member(name, id);
                library.RegisterMember(member);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        static Member? LogIn(Library library)
        {
            Console.Write("\nEnter Member ID to log in (or type 'exit' to cancel): ");
            string id = Console.ReadLine() ?? "";
            if (id.ToLower() == "exit") return null;

            var member = library.FindMemberById(id);
            if (member == null)
            {
                Console.WriteLine("Member not found.");
            }
            else
            {
                Console.WriteLine($"Welcome, {member.Name}!");
            }
            return member;
        }


        static void AddBookToLibrary(Library library)
        {
            Console.WriteLine("\n-- Add New Book --");
            Console.Write("Enter title (or 'exit' to cancel): ");
            string title = Console.ReadLine() ?? "";
            if (title.ToLower() == "exit") return;

            Console.Write("Enter author: ");
            string author = Console.ReadLine() ?? "";
            if (author.ToLower() == "exit") return;

            Console.Write("Enter ISBN: ");
            string isbn = Console.ReadLine() ?? "";
            if (isbn.ToLower() == "exit") return;

            try
            {
                var book = new Book(title, author, isbn);
                library.AddBook(book);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        static void MemberMenu(Member member, Library library)
        {
            while (true)
            {
                Console.WriteLine($"\n--- Member Menu ({member.Name}) ---");
                Console.WriteLine("1. View Available Books");
                Console.WriteLine("2. Borrow Book");
                Console.WriteLine("3. Return Book");
                Console.WriteLine("4. Log Out");
                Console.Write("Select an option: ");
                string input = Console.ReadLine() ?? "";

                switch (input)
                {
                    case "1":
                        library.DisplayAvailableBooks();
                        break;
                    case "2":
                        Console.Write("Enter title to borrow: ");
                        string borrowTitle = Console.ReadLine() ?? "";
                        var bookToBorrow = library.FindBookByTitle(borrowTitle);
                        if (bookToBorrow != null)
                            member.BorrowBook(bookToBorrow);
                        else
                            Console.WriteLine("Book not found.");
                        break;
                    case "3":
                        Console.Write("Enter title to return: ");
                        string returnTitle = Console.ReadLine() ?? "";
                        var bookToReturn = library.FindBookByTitle(returnTitle);
                        if (bookToReturn != null)
                            member.ReturnBook(bookToReturn);
                        else
                            Console.WriteLine("Book not found.");
                        break;
                    case "4":
                        Console.WriteLine("Logged out.");
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }

}
