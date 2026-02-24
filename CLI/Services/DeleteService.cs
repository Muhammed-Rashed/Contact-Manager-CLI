using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DeleteService
{
    private readonly Dictionary<Guid, Contact> _byId;
    private readonly List<ContactIndex> _indexes;

    public DeleteService(Dictionary<Guid, Contact> byId, List<ContactIndex> indexes)
    {
        _byId = byId;
        _indexes = indexes;
    }

    public async Task<bool> Delete(Guid id)
    {
        if (!_byId.ContainsKey(id))
            return await Task.FromResult(false);

        Contact contact = _byId[id];
        _byId.Remove(id);
        foreach (var index in _indexes)
            index.Remove(contact);

        return await Task.FromResult(true);
    }
}