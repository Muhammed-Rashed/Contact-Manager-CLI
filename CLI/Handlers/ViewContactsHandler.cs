using System;
using System.Collections.Generic;

public class ViewContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public ViewContactHandler(IContactService service) { _service = service; }

    public async void Handle()
    {
        Console.Clear();
        SectionHeader("View Contacts");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        IReadOnlyList<Contact> contacts = await _service.GetAll();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  Total: {contacts.Count} contact(s)\n");
        Console.ResetColor();

        foreach (var c in contacts)
            PrintContactCard(c);

        Pause();
    }
}