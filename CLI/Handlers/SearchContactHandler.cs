using System;
using System.Collections.Generic;
using System.Linq;

public class SearchContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public SearchContactHandler(IContactService service) { _service = service; }

    public async void Handle()
    {
        Console.Clear();
        SectionHeader("Search Contacts");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        Console.Write("  Search (name, phone, or email): ");
        string query = Console.ReadLine().Trim();

        IEnumerable<Contact> results = await _service.Search(query);
        List<Contact> list = results.ToList();

        if (list.Count == 0) { DisplayError($"No contacts found matching \"{query}\"."); return; }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  {list.Count} result(s) for \"{query}\":\n");
        Console.ResetColor();

        foreach (var c in list)
            PrintContactCard(c);

        Pause();
    }
}