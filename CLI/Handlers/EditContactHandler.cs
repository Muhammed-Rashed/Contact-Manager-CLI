using System;

public class EditContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;
    private readonly IContactSelector _selector;

    public EditContactHandler(IContactService service, IContactSelector selector)
    {
        _service = service;
        _selector = selector;
    }

    public async void Handle()
    {
        Console.Clear();
        SectionHeader("Edit Contact");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        Contact found = await _selector.Select("edit");
        if (found == null) { Pause(); return; }

        PrintContactCard(found);
        Console.WriteLine("\n  Enter new values (leave blank to keep current):\n");

        Console.Write($"  Name  [{found.Name}] : ");
        string name = Console.ReadLine().Trim();

        Console.Write($"  Phone [{found.Phone}] : ");
        string phone = Console.ReadLine().Trim();

        Console.Write($"  Email [{found.Email}] : ");
        string email = Console.ReadLine().Trim();

        await _service.Edit(found.Id,
            string.IsNullOrWhiteSpace(name) ? found.Name : name,
            string.IsNullOrWhiteSpace(phone) ? found.Phone : phone,
            string.IsNullOrWhiteSpace(email) ? found.Email : email);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n  Contact updated successfully!");
        Console.ResetColor();

        Contact updated = await _service.GetById(found.Id);
        PrintContactCard(updated);

        Pause();
    }
}