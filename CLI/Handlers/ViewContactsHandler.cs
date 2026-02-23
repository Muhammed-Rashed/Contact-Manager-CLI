public class ViewContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public ViewContactHandler(IContactService service)
    {
        _service = service;
    }

    public void Handle()
    {
        Console.Clear();
        SectionHeader("View Contacts");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        var contacts = _service.GetAll();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  Total: {contacts.Count} contact(s)\n");
        Console.ResetColor();

        foreach (var c in contacts)
            PrintContactCard(c);

        Pause();
    }
}