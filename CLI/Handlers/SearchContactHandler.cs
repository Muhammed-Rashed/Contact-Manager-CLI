public class SearchContactHandler : BaseHandler, IContactHandler
{
    private readonly IContactService _service;

    public SearchContactHandler(IContactService service)
    {
        _service = service;
    }

    public void Handle()
    {
        Console.Clear();
        SectionHeader("Search Contacts");

        if (_service.Count == 0) { DisplayError("No contacts available."); return; }

        Console.Write("  Search (name, phone, or email): ");
        string query = Console.ReadLine()?.Trim() ?? "";

        var results = _service.Search(query).ToList();

        if (results.Count == 0) { DisplayError($"No contacts found matching \"{query}\"."); return; }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  {results.Count} result(s) for \"{query}\":\n");
        Console.ResetColor();

        foreach (var c in results)
            PrintContactCard(c);

        Pause();
    }
}