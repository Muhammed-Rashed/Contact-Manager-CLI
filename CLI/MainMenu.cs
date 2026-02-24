public class MainMenu
{
    private readonly Dictionary<string, IContactHandler> _handlers;
    private readonly IContactService _service;

    public MainMenu(Dictionary<string, IContactHandler> handlers, IContactService service)
    {
        _handlers = handlers;
        _service = service;
    }

    private void Display()
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

    public void Run()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(_service.Count > 0
        ? $"  {_service.Count} contact(s) loaded.\n"
        : "  No contacts found.\n");
        Console.ResetColor();
        while (true)
        {
            Display();
            string choice = Console.ReadLine()?.Trim();

            if (choice == "9") break;

            if (_handlers.TryGetValue(choice, out var handler))
                handler.Handle();
            else
                Console.WriteLine("  Invalid option.");
        }
    }
}