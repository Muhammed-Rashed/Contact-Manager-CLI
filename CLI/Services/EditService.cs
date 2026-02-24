using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EditService
{
    private readonly Dictionary<Guid, Contact> _byId;
    private readonly List<ContactIndex> _indexes;

    public EditService(Dictionary<Guid, Contact> byId, List<ContactIndex> indexes)
    {
        _byId = byId;
        _indexes = indexes;
    }

    public async Task<bool> Edit(Guid id, string name, string phone, string email)
    {
        if (!_byId.ContainsKey(id))
            return await Task.FromResult(false);

        Contact contact = _byId[id];

        foreach (var index in _indexes)
            index.Remove(contact);

        contact.Name = name;
        contact.Phone = phone;
        contact.Email = email;

        foreach (var index in _indexes)
            index.Add(contact);

        return await Task.FromResult(true);
    }
}