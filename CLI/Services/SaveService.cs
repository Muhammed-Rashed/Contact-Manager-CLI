using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SaveService
{
    private readonly Dictionary<Guid, Contact> _byId;
    private readonly IContactRepository _repository;

    public SaveService(Dictionary<Guid, Contact> byId, IContactRepository repository)
    {
        _byId = byId;
        _repository = repository;
    }

    public async Task Save()
    {
        await _repository.Save(_byId.Values);
    }
}