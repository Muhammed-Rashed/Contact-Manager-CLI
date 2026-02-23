public class ListContactsHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public ListContactsHandler(IContactService service)
    {
        _service = service;
    }

    public void Handle()
    {
        Console.Clear();
        SectionHeader("All Contacts");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

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
}