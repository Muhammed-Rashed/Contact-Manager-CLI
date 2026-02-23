using System;
using System.Collections.Generic;
using System.Linq;

public class ContactService : IContactService
{
    private readonly List<Contact> _contacts = new List<Contact>();

    public Contact Add(string name, string phone, string email)
    {
        var contact = new Contact(name, phone, email);
        _contacts.Add(contact);
        return contact;
    }

    public Contact GetById(Guid id)
    {
        return _contacts.FirstOrDefault(c => c.Id == id);
    }

    public Contact GetByQuery(string query)
    {
        if (Guid.TryParse(query, out Guid id))
            return GetById(id);

        return _contacts.FirstOrDefault(c =>
            c.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<Contact> GetAll()
    {
        return _contacts.AsReadOnly();
    }

    public bool Edit(Guid id, string name, string phone, string email)
    {
        var contact = GetById(id);
        if (contact == null) return false;

        contact.Name = name;
        contact.Phone = phone;
        contact.Email = email;
        return true;
    }

    public bool Delete(Guid id)
    {
        var contact = GetById(id);
        if (contact == null) return false;

        _contacts.Remove(contact);
        return true;
    }

    public IEnumerable<Contact> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return _contacts;

        return _contacts.Where(c =>
            c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            c.Phone.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Contact> Filter(
        string? nameContains = null,
        string? emailContains = null,
        string? phoneContains = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null)
    {
        IEnumerable<Contact> result = _contacts;

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

        return result;
    }

    public int Count => _contacts.Count;
}