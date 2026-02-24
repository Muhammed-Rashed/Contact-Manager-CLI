using System.Collections.Generic;
using System.Threading.Tasks;

public interface IContactRepository
{
    Task Save(IEnumerable<Contact> contacts);
    Task<List<Contact>> Load();
}