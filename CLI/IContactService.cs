using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IContactService
{
    Task<Contact> Add(string name, string phone, string email);
    Task<Contact> GetById(Guid id);
    Task<Contact> GetByQuery(string query);
    Task<IReadOnlyList<Contact>> GetAll();
    Task<bool> Edit(Guid id, string name, string phone, string email);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<Contact>> Search(string query);
    Task<IEnumerable<Contact>> Filter(string nameContains, string emailContains, string phoneContains, DateTime? createdAfter, DateTime? createdBefore);
    int Count { get; }
}