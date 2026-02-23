public class SaveContactsHandler : BaseHandler, IContactHandler
{
    public void Handle()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n  not done");
        Console.ResetColor();
        Pause();
    }
}