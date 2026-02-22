using System;

namespace ContactManager
{
    class Program
    {
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
            DisplayStub("Add Contact");
        }

        static void HandleEditContact()
        {
            DisplayStub("Edit Contact");
        }

        static void HandleDeleteContact()
        {
            DisplayStub("Delete Contact");
        }

        static void HandleViewContact()
        {
            DisplayStub("View Contact");
        }

        static void HandleListContacts()
        {
            DisplayStub("List Contacts");
        }

        static void HandleSearch()
        {
            DisplayStub("Search");
        }

        static void HandleFilter()
        {
            DisplayStub("Filter");
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
    }
}