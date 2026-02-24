using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AddService
{
    private readonly Dictionary<Guid, Contact> _byId;
    private readonly List<ContactIndex> _indexes;

    public AddService(Dictionary<Guid, Contact> byId, List<ContactIndex> indexes)
    {
        _byId = byId;
        _indexes = indexes;
    }

    public async Task<Contact> Add(string name, string phone, string email)
    {
        var contact = new Contact(name, phone, email);
        _byId[contact.Id] = contact;
        foreach (var index in _indexes)
            index.Add(contact);
        return await Task.FromResult(contact);
    }
}