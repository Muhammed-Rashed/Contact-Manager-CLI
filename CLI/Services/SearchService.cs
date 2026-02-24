using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SearchService
{
    private readonly Dictionary<Guid, Contact> _byId;
    private readonly List<ContactIndex> _indexes;

    public SearchService(Dictionary<Guid, Contact> byId, List<ContactIndex> indexes)
    {
        _byId = byId;
        _indexes = indexes;
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
}