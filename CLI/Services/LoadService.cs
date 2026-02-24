using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LoadService
{
    private readonly Dictionary<Guid, Contact> _byId;
    private readonly List<ContactIndex> _indexes;
    private readonly IContactRepository _repository;

    public LoadService(Dictionary<Guid, Contact> byId, List<ContactIndex> indexes, IContactRepository repository)
    {
        _byId = byId;
        _indexes = indexes;
        _repository = repository;
    }

    public async Task Load()
    {
        List<Contact> contacts = await _repository.Load();
        foreach (var c in contacts)
        {
            _byId[c.Id] = c;
            foreach (var index in _indexes)
                index.Add(c);
        }
    }
}