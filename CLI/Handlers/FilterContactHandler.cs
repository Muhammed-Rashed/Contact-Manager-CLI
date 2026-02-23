public class FilterContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public FilterContactHandler(IContactService service)
    {
        _service = service;
    }

    public void Handle()
    {
        Console.Clear();
        SectionHeader("Filter Contacts");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        Console.WriteLine("  Leave any field blank to skip it.\n");

        Console.Write("  Name  : ");
        string? name = Console.ReadLine()?.Trim();

        Console.Write("  Email : ");
        string? email = Console.ReadLine()?.Trim();

        Console.Write("  Phone : ");
        string? phone = Console.ReadLine()?.Trim();

        Console.Write("  Created after  (yyyy-MM-dd, blank to skip): ");
        DateTime? after = DateTime.TryParse(Console.ReadLine()?.Trim(), out var a) ? a : null;

        Console.Write("  Created before (yyyy-MM-dd, blank to skip): ");
        DateTime? before = DateTime.TryParse(Console.ReadLine()?.Trim(), out var b) ? b : null;

        var results = _service.Filter(
            string.IsNullOrWhiteSpace(name) ? null : name,
            string.IsNullOrWhiteSpace(email) ? null : email,
            string.IsNullOrWhiteSpace(phone) ? null : phone,
            after, before
        ).ToList();

        if (results.Count == 0) { DisplayError("No contacts match the given filters."); return; }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  {results.Count} contact(s) found:\n");
        Console.ResetColor();

        foreach (var c in results)
            PrintContactCard(c);

        Pause();
    }
}