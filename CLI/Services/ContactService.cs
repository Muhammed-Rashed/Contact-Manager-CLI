using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ContactService : IContactService
{
    private readonly Dictionary<Guid, Contact> _byId    = new Dictionary<Guid, Contact>();
    private readonly List<ContactIndex>        _indexes;

    private readonly AddService    _addService;
    private readonly EditService   _editService;
    private readonly DeleteService _deleteService;
    private readonly SearchService _searchService;
    private readonly FilterService _filterService;
    private readonly SaveService   _saveService;
    private readonly LoadService   _loadService;

    public ContactService(IContactRepository repository)
    {
        _indexes = new List<ContactIndex>
        {
            new ContactIndex(c => c.Name),
            new ContactIndex(c => c.Phone),
            new ContactIndex(c => c.Email),
            new ContactIndex(c => c.CreationDate.ToString("yyyy-MM-dd")),
        };

        _addService    = new AddService(_byId, _indexes);
        _editService   = new EditService(_byId, _indexes);
        _deleteService = new DeleteService(_byId, _indexes);
        _searchService = new SearchService(_byId, _indexes);
        _filterService = new FilterService(_byId);
        _saveService   = new SaveService(_byId, repository);
        _loadService   = new LoadService(_byId, _indexes, repository);
    }

    public async Task<Contact> Add(string name, string phone, string email)
        => await _addService.Add(name, phone, email);

    public async Task<bool> Edit(Guid id, string name, string phone, string email)
        => await _editService.Edit(id, name, phone, email);

    public async Task<bool> Delete(Guid id)
        => await _deleteService.Delete(id);

    public async Task<IEnumerable<Contact>> Search(string query)
        => await _searchService.Search(query);

    public async Task<IEnumerable<Contact>> Filter(
        string nameContains = null,
        string emailContains = null,
        string phoneContains = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null)
        => await _filterService.Filter(nameContains, emailContains, phoneContains, createdAfter, createdBefore);

    public async Task Save()
        => await _saveService.Save();

    public async Task Load()
        => await _loadService.Load();

    public async Task<Contact> GetById(Guid id)
    {
        if (_byId.ContainsKey(id))
            return await Task.FromResult(_byId[id]);
        return await Task.FromResult<Contact>(null);
    }

    public async Task<List<Contact>> GetByQuery(string query)
    {
        if (Guid.TryParse(query, out Guid id))
        {
            var single = new List<Contact>();
            if (_byId.ContainsKey(id)) single.Add(_byId[id]);
            return await Task.FromResult(single);
        }

        List<Contact> matches = new List<Contact>();
        foreach (var c in _byId.Values)
            if (c.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                matches.Add(c);

        return await Task.FromResult(matches);
    }

    public async Task<IReadOnlyList<Contact>> GetAll()
        => await Task.FromResult<IReadOnlyList<Contact>>(
            new List<Contact>(_byId.Values).AsReadOnly());

    public int Count => _byId.Count;
}