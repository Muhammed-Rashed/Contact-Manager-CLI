using System;

public class DeleteContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;
    private readonly IContactSelector _selector;

    public DeleteContactHandler(IContactService service, IContactSelector selector)
    {
        _service = service;
        _selector = selector;
    }

    public async void Handle()
    {
        Console.Clear();
        SectionHeader("Delete Contact");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        Contact found = await _selector.Select("delete");
        if (found == null) { Pause(); return; }

        PrintContactCard(found);

        Console.Write("\n  Are you sure you want to delete this contact? (y/n): ");
        string confirm = Console.ReadLine().Trim().ToLower();

        if (confirm != "y")
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  Delete cancelled.");
            Console.ResetColor();
            Pause();
            return;
        }

        await _service.Delete(found.Id);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n  Contact deleted successfully.");
        Console.ResetColor();

        Pause();
    }
}