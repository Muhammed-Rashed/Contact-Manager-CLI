using System;

public class SaveContactsHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public SaveContactsHandler(IContactService service) { _service = service; }

    public async void Handle()
    {
        Console.Clear();
        SectionHeader("Save Contacts");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n  Saving contacts...");
        Console.ResetColor();

        await _service.Save();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  Contacts saved successfully!");
        Console.ResetColor();

        Pause();
    }
}