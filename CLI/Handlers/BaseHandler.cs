using System;

public abstract class BaseHandler
{
    protected void SectionHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  [ {title} ]");
        Console.ResetColor();
        Console.WriteLine();
    }

    protected void PrintContactCard(Contact c)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  ┌──────────────────────────────────────────────────┐");
        Console.WriteLine("  │                  Contact Details                 │");
        Console.WriteLine("  ├──────────────────────────────────────────────────┤");
        Console.ResetColor();
        Console.WriteLine($"  │  ID      : {c.Id,-38}│");
        Console.WriteLine($"  │  Name    : {c.Name,-38}│");
        Console.WriteLine($"  │  Phone   : {c.Phone,-38}│");
        Console.WriteLine($"  │  Email   : {c.Email,-38}│");
        Console.WriteLine($"  │  Created : {c.CreationDate:yyyy-MM-dd HH:mm:ss}                   │");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  └──────────────────────────────────────────────────┘");
        Console.ResetColor();
    }

    protected void DisplayError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  {message}");
        Console.ResetColor();
        Pause();
    }

    protected string PromptRequired(string label)
    {
        string value;
        do
        {
            Console.Write(label);
            value = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(value))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Field is required. Please try again.");
                Console.ResetColor();
            }
        }
        while (string.IsNullOrWhiteSpace(value));
        return value;
    }

    protected void Pause()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("\n  Press any key to return to menu...");
        Console.ResetColor();
        Console.ReadKey(intercept: true);
        Console.Clear();
    }
}