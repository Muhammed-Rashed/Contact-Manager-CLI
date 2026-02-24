using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ContactService : IContactService
{
    private readonly Dictionary<Guid, Contact> _byId = new Dictionary<Guid, Contact>();

    private readonly List<ContactIndex> _indexes;
    private readonly IContactRepository _repository;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
        _indexes = new List<ContactIndex>
        {
            new ContactIndex(c => c.Name),
            new ContactIndex(c => c.Phone),
            new ContactIndex(c => c.Email),
            new ContactIndex(c => c.CreationDate.ToString("yyyy-MM-dd")),
        };
    }

    private void IndexContact(Contact contact)
    {
        _byId[contact.Id] = contact;
        foreach (var index in _indexes)
            index.Add(contact);
    }

    private void RemoveFromIndex(Contact contact)
    {
        _byId.Remove(contact.Id);
        foreach (var index in _indexes)
            index.Remove(contact);
    }

    public async Task<Contact> Add(string name, string phone, string email)
    {
        var contact = new Contact(name, phone, email);
        IndexContact(contact);
        return await Task.FromResult(contact);
    }

    public async Task<Contact> GetById(Guid id)
    {
        if (_byId.ContainsKey(id))
            return await Task.FromResult(_byId[id]);
        return await Task.FromResult<Contact>(null);
    }

    public async Task<Contact> GetByQuery(string query)
    {
        if (Guid.TryParse(query, out Guid id))
            return await GetById(id);

        Contact found = _byId.Values
            .FirstOrDefault(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
        return await Task.FromResult(found);
    }

    public async Task<IReadOnlyList<Contact>> GetAll()
    {
        return await Task.FromResult<IReadOnlyList<Contact>>(
            new List<Contact>(_byId.Values).AsReadOnly());
    }

    public async Task<bool> Edit(Guid id, string name, string phone, string email)
    {
        if (!_byId.ContainsKey(id))
            return await Task.FromResult(false);

        Contact contact = _byId[id];
        RemoveFromIndex(contact);

        contact.Name = name;
        contact.Phone = phone;
        contact.Email = email;

        IndexContact(contact);
        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(Guid id)
    {
        if (!_byId.ContainsKey(id))
            return await Task.FromResult(false);

        Contact contact = _byId[id];
        RemoveFromIndex(contact);
        return await Task.FromResult(true);
    }

    public async Task<IEnumerable<Contact>> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await Task.FromResult(_byId.Values.AsEnumerable());

        if (Guid.TryParse(query, out Guid id))
        {
            var single = new List<Contact>();
            if (_byId.ContainsKey(id)) single.Add(_byId[id]);
            return await Task.FromResult(single.AsEnumerable());
        }

        HashSet<Guid> seen = new HashSet<Guid>();
        List<Contact> results = new List<Contact>();

        foreach (var index in _indexes)
            foreach (var c in index.Search(query))
                if (seen.Add(c.Id)) results.Add(c);

        return await Task.FromResult(results.AsEnumerable());
    }

    public async Task<IEnumerable<Contact>> Filter(
        string nameContains = null,
        string emailContains = null,
        string phoneContains = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null)
    {
        IEnumerable<Contact> result = _byId.Values;

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

    public async Task Save()
    {
        await _repository.Save(_byId.Values);
    }

    public async Task Load()
    {
        List<Contact> contacts = await _repository.Load();
        foreach (var c in contacts)
            IndexContact(c);
    }

    public int Count => _byId.Count;
}