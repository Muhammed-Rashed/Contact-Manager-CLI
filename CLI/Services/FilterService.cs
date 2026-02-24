using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FilterService
{
    private readonly Dictionary<Guid, Contact> _byId;

    public FilterService(Dictionary<Guid, Contact> byId)
    {
        _byId = byId;
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
}