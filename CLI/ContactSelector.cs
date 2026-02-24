using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ContactSelector : IContactSelector
{
    private readonly IContactService _service;

    public ContactSelector(IContactService service)
    {
        _service = service;
    }

    public async Task<Contact> Select(string action)
    {
        Console.Write($"  Search contact to {action}: ");
        string query = Console.ReadLine().Trim();

        List<Contact> matches = await _service.GetByQuery(query);

        if (matches.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  No contacts found matching \"{query}\".");
            Console.ResetColor();
            return null;
        }

        if (matches.Count == 1)
            return matches[0];

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  {matches.Count} contacts found. Pick one:\n");
        Console.ResetColor();

        for (int i = 0; i < matches.Count; i++)
            Console.WriteLine($"  {i + 1}.  {matches[i].Name,-25} {matches[i].Phone,-15} {matches[i].Email}");

        Console.WriteLine();

        while (true)
        {
            Console.Write("  Enter number: ");
            string input = Console.ReadLine().Trim();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= matches.Count)
                return matches[choice - 1];

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  Invalid. Enter a number between 1 and {matches.Count}.");
            Console.ResetColor();
        }
    }
}