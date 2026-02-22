using System;

namespace ContactManager
{
    class Program
    {
        static readonly ContactService _service = new ContactService();

        static void Main(string[] args)
        {
            Console.Title = "Microsoft Contact Manager";

            DisplayWelcomeBanner();
            RunMainMenu();
        }

        static void DisplayWelcomeBanner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("      Microsoft Contact Manager CLI      ");
            Console.WriteLine("         Cairo University FCAI          ");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void RunMainMenu()
        {
            bool running = true;

            while (running)
            {
                DisplayMainMenu();

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        HandleAddContact();
                        break;
                    case "2":
                        HandleEditContact();
                        break;
                    case "3":
                        HandleDeleteContact();
                        break;
                    case "4":
                        HandleViewContact();
                        break;
                    case "5":
                        HandleListContacts();
                        break;
                    case "6":
                        HandleSearch();
                        break;
                    case "7":
                        HandleFilter();
                        break;
                    case "8":
                        HandleSave();
                        break;
                    case "9":
                        running = false;
                        HandleExit();
                        break;
                    default:
                        DisplayError("Invalid option. Please enter a number from 1 to 9.");
                        break;
                }
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌─────────────────────────────┐");
            Console.WriteLine("  │          Main Menu          │");
            Console.WriteLine("  ├─────────────────────────────┤");
            Console.ResetColor();
            Console.WriteLine("  │  1.  Add Contact            │");
            Console.WriteLine("  │  2.  Edit Contact           │");
            Console.WriteLine("  │  3.  Delete Contact         │");
            Console.WriteLine("  │  4.  View Contact           │");
            Console.WriteLine("  │  5.  List Contacts          │");
            Console.WriteLine("  │  6.  Search                 │");
            Console.WriteLine("  │  7.  Filter                 │");
            Console.WriteLine("  │  8.  Save                   │");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  │  9.  Exit                   │");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  └─────────────────────────────┘");
            Console.ResetColor();
            Console.Write("\n  Enter your choice: ");
        }

        static void HandleAddContact()
        {
            Console.Clear();
            DisplayWelcomeBanner();
            SectionHeader("Add Contact");

            string name = PromptRequired("  Name  : ");
            string phone = PromptRequired("  Phone : ");
            string email = PromptRequired("  Email : ");

            Contact contact = _service.Add(name, phone, email);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n Contact added successfully!");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"     ID      : {contact.Id}");
            Console.WriteLine($"     Created : {contact.CreationDate:yyyy-MM-dd HH:mm:ss}");
            Console.ResetColor();

            Pause();
            
        }

        static void HandleViewContact()
        {
            Console.Clear();
            DisplayWelcomeBanner();
            SectionHeader("View Contacts");

            if (_service.Count == 0)
            {
                DisplayError("No contacts available.");
                return;
            }

            var contacts = _service.GetAll();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  Total: {contacts.Count} contact(s)\n");
            Console.ResetColor();

            foreach (var c in contacts)
                PrintContactCard(c);

            Pause();
        }

        static void HandleEditContact()
        {
            DisplayStub("Edit Contact");
        }

        static void HandleDeleteContact()
        {
            Console.Clear();
            DisplayWelcomeBanner();
            SectionHeader("Delete Contact");

            if (_service.Count == 0)
            {
                DisplayError("No contacts available.");
                return;
            }

            Console.Write("  Enter contact ID or Name: ");
            string query = Console.ReadLine()?.Trim() ?? "";

            Contact found = _service.GetByQuery(query);
            if (found == null)
            {
                DisplayError($"No contact found matching \"{query}\".");
                return;
            }

            // Show the contact before deleting
            PrintContactCard(found);

            Console.Write("\n  Are you sure you want to delete this contact? (y/n): ");
            string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (confirm != "y")
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n  Delete cancelled.");
                Console.ResetColor();
                Pause();
                return;
            }

            _service.Delete(found.Id);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n Contact deleted successfully.");
            Console.ResetColor();

            Pause();
        }

        static void HandleListContacts()
        {
            Console.Clear();
            DisplayWelcomeBanner();
            SectionHeader("All Contacts");

            if (_service.Count == 0)
            {
                DisplayError("No contacts available.");
                return;
            }

            var contacts = _service.GetAll();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  {"ID",-38}  Name");
            Console.WriteLine($"  {new string('-', 38)}  {new string('-', 20)}");
            Console.ResetColor();

            foreach (var c in contacts)
                Console.WriteLine($"  {c.Id,-38}  {c.Name}");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n  Total: {contacts.Count} contact(s)");
            Console.ResetColor();

            Pause();
        }

        static void HandleSearch()
        {
            Console.Clear();
            DisplayWelcomeBanner();
            SectionHeader("Search Contacts");

            if (_service.Count == 0)
            {
                DisplayError("No contacts available.");
                return;
            }

            Console.Write("  Search (name, phone, or email): ");
            string query = Console.ReadLine()?.Trim() ?? "";

            var results = _service.Search(query).ToList();

            if (results.Count == 0)
            {
                DisplayError($"No contacts found matching \"{query}\".");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  {results.Count} result(s) for \"{query}\":\n");
            Console.ResetColor();

            foreach (var c in results)
                PrintContactCard(c);

            Pause();
        }

        static void HandleFilter()
        {
            Console.Clear();
            DisplayWelcomeBanner();
            SectionHeader("Filter Contacts");

            if (_service.Count == 0)
            {
                DisplayError("No contacts available.");
                return;
            }

            Console.WriteLine("  Leave any field blank to skip it.\n");

            Console.Write("  Name  : ");
            string? name = Console.ReadLine()?.Trim();

            Console.Write("  Email : ");
            string? email = Console.ReadLine()?.Trim();

            Console.Write("  Phone : ");
            string? phone = Console.ReadLine()?.Trim();

            Console.Write("  Created after  (yyyy-MM-dd, blank to skip): ");
            string? afterInput = Console.ReadLine()?.Trim();

            Console.Write("  Created before (yyyy-MM-dd, blank to skip): ");
            string? beforeInput = Console.ReadLine()?.Trim();

            DateTime? after = DateTime.TryParse(afterInput, out var a) ? a : null;
            DateTime? before = DateTime.TryParse(beforeInput, out var b) ? b : null;

            var results = _service.Filter(
                nameContains: string.IsNullOrWhiteSpace(name) ? null : name,
                emailContains: string.IsNullOrWhiteSpace(email) ? null : email,
                phoneContains: string.IsNullOrWhiteSpace(phone) ? null : phone,
                createdAfter: after,
                createdBefore: before
            ).ToList();

            if (results.Count == 0)
            {
                DisplayError("No contacts match the given filters.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  {results.Count} contact(s) found:\n");
            Console.ResetColor();

            foreach (var c in results)
                PrintContactCard(c);

            Pause();
        }

        static void HandleSave()
        {
            DisplayStub("Save");
        }

        static void HandleExit()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  Goodbye");
            Console.ResetColor();
        }

        static void DisplayStub(string featureName)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n  [ {featureName} ] not done");
            Console.ResetColor();
            Pause();
        }

        static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n {message}");
            Console.ResetColor();
            Pause();
        }

        static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n  Press any key to return to menu...");
            Console.ResetColor();
            Console.ReadKey(intercept: true);
            Console.Clear();
            DisplayWelcomeBanner();
        }

        static void SectionHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  [ {title} ]");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void PrintContactCard(Contact c)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ┌──────────────────────────────────────────────────┐");
            Console.WriteLine("  │                  Contact Details                 │");
            Console.WriteLine("  ├──────────────────────────────────────────────────┤");
            Console.ResetColor();
            Console.WriteLine($"  │  ID      : {c.Id,-38}│");
            Console.WriteLine($"  │  Name    : {c.Name,-38}│");
            Console.WriteLine($"  │  Phone   : {c.Phone,-38}│");
            Console.WriteLine($"  │  Email   : {c.Email,-38}│");
            Console.WriteLine($"  │  Created : {c.CreationDate:yyyy-MM-dd HH:mm:ss}                   │");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  └──────────────────────────────────────────────────┘");
            Console.ResetColor();
        }

        static string PromptRequired(string label)
        {
            string value;
            do
            {
                Console.Write(label);
                value = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Field is required. Please try again.");
                    Console.ResetColor();
                }
            }
            while (string.IsNullOrWhiteSpace(value));
            return value;
        }
    }
}