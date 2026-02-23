using System;

public class EditContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public EditContactHandler(IContactService service) { _service = service; }

    public async void Handle()
    {
        Console.Clear();
        SectionHeader("Edit Contact");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        Console.Write("  Enter contact ID or Name: ");
        string query = Console.ReadLine().Trim();

        Contact found = await _service.GetByQuery(query);
        if (found == null) { DisplayError($"No contact found matching \"{query}\"."); return; }

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