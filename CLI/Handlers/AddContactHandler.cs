using System;

public class AddContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public AddContactHandler(IContactService service) { _service = service; }

    public async void Handle()
    {
        Console.Clear();
        SectionHeader("Add Contact");

        string name = PromptRequired("  Name  : ");
        string phone = PromptRequired("  Phone : ");
        string email = PromptRequired("  Email : ");

        Contact contact = await _service.Add(name, phone, email);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n  Contact added successfully!");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"     ID      : {contact.Id}");
        Console.WriteLine($"     Created : {contact.CreationDate:yyyy-MM-dd HH:mm:ss}");
        Console.ResetColor();

        Pause();
    }
}