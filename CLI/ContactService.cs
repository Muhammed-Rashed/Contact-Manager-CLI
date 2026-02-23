using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ContactService : IContactService
{
    private readonly Dictionary<Guid, Contact> _contacts = new Dictionary<Guid, Contact>();

    public async Task<Contact> Add(string name, string phone, string email)
    {
        var contact = new Contact(name, phone, email);
        _contacts[contact.Id] = contact;
        return await Task.FromResult(contact);
    }

    public async Task<Contact> GetById(Guid id)
    {
        if (_contacts.ContainsKey(id))
            return await Task.FromResult(_contacts[id]);
        return await Task.FromResult<Contact>(null);
    }

    public async Task<Contact> GetByQuery(string query)
    {
        if (Guid.TryParse(query, out Guid id))
            return await GetById(id);

        Contact found = _contacts.Values.FirstOrDefault(c =>
            c.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(found);
    }

    public async Task<IReadOnlyList<Contact>> GetAll()
    {
        return await Task.FromResult<IReadOnlyList<Contact>>(
            new List<Contact>(_contacts.Values).AsReadOnly());
    }

    public async Task<bool> Edit(Guid id, string name, string phone, string email)
    {
        if (!_contacts.ContainsKey(id))
            return await Task.FromResult(false);

        _contacts[id].Name = name;
        _contacts[id].Phone = phone;
        _contacts[id].Email = email;
        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Guid id)
    {
        if (!_contacts.ContainsKey(id))
            return await Task.FromResult(false);

        _contacts.Remove(id);
        return await Task.FromResult(true);
    }

    public async Task<IEnumerable<Contact>> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await Task.FromResult(_contacts.Values.AsEnumerable());

        var results = _contacts.Values.Where(c =>
            c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            c.Phone.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains(query, StringComparison.OrdinalIgnoreCase));

        return await Task.FromResult(results);
    }

    public async Task<IEnumerable<Contact>> Filter(
        string nameContains = null,
        string emailContains = null,
        string phoneContains = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null)
    {
        IEnumerable<Contact> result = _contacts.Values;

        if (!string.IsNullOrWhiteSpace(nameContains))
            result = result.Where(c =>
                c.Name.Contains(nameContains, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(emailContains))
            result = result.Where(c =>
                c.Email.Contains(emailContains, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(phoneContains))
            result = result.Where(c =>
                c.Phone.Contains(phoneContains, StringComparison.OrdinalIgnoreCase));

        if (createdAfter.HasValue)
            result = result.Where(c => c.CreationDate >= createdAfter.Value);

        if (createdBefore.HasValue)
            result = result.Where(c => c.CreationDate <= createdBefore.Value);

        return await Task.FromResult(result);
    }

    public int Count => _contacts.Count;
}