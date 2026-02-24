using System.Threading.Tasks;

public interface IContactSelector
{
    Task<Contact> Select(string action);
}