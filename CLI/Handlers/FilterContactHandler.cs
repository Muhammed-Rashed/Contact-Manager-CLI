using System;
using System.Collections.Generic;
using System.Linq;

public class FilterContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public FilterContactHandler(IContactService service) { _service = service; }

    public async void Handle()
    {
        Console.Clear();
        SectionHeader("Filter Contacts");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        Console.WriteLine("  Leave any field blank to skip it.\n");

        Console.Write("  Name  : ");
        string name = Console.ReadLine().Trim();

        Console.Write("  Email : ");
        string email = Console.ReadLine().Trim();

        Console.Write("  Phone : ");
        string phone = Console.ReadLine().Trim();

        Console.Write("  Created after  (yyyy-MM-dd, blank to skip): ");
        string afterInput = Console.ReadLine().Trim();

        Console.Write("  Created before (yyyy-MM-dd, blank to skip): ");
        string beforeInput = Console.ReadLine().Trim();

        DateTime afterDate;
        DateTime beforeDate;
        DateTime? after = DateTime.TryParse(afterInput, out afterDate) ? afterDate : (DateTime?)null;
        DateTime? before = DateTime.TryParse(beforeInput, out beforeDate) ? beforeDate : (DateTime?)null;

        IEnumerable<Contact> results = await _service.Filter(
            string.IsNullOrWhiteSpace(name) ? null : name,
            string.IsNullOrWhiteSpace(email) ? null : email,
            string.IsNullOrWhiteSpace(phone) ? null : phone,
            after, before);

        List<Contact> list = results.ToList();

        if (list.Count == 0) { DisplayError("No contacts match the given filters."); return; }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  {list.Count} contact(s) found:\n");
        Console.ResetColor();

        foreach (var c in list)
            PrintContactCard(c);

        Pause();
    }
}