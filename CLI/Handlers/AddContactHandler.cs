public class AddContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;
    public AddContactHandler(IContactService service) { _service = service; }

    public void Handle()
    {
        Console.Clear();
        SectionHeader("Add Contact");

        string name = PromptRequired("  Name  : ");
        string phone = PromptRequired("  Phone : ");
        string email = PromptRequired("  Email : ");

        Contact contact = _service.Add(name, phone, email);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n  Contact added successfully!");
        Console.ResetColor();
        Console.WriteLine($"     ID      : {contact.Id}");
        Console.WriteLine($"     Created : {contact.CreationDate:yyyy-MM-dd HH:mm:ss}");

        Pause();
    }
}