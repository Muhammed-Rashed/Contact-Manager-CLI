public class DeleteContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public DeleteContactHandler(IContactService service) { _service = service; }

    public void Handle()
    {
        Console.Clear();
        SectionHeader("Delete Contact");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        Console.Write("  Enter contact ID or Name: ");
        string query = Console.ReadLine()?.Trim();

        Contact found = _service.GetByQuery(query);
        if (found == null) { DisplayError($"No contact found matching \"{query}\"."); return; }

        PrintContactCard(found);

        Console.Write("\n  Are you sure you want to delete this contact? (y/n): ");
        string confirm = Console.ReadLine()?.Trim().ToLower();

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
        Console.WriteLine("\n  Contact deleted successfully.");
        Console.ResetColor();

        Pause();
    }
}