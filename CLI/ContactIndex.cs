using System;
using System.Collections.Generic;

public class ContactIndex
{
    private readonly Dictionary<string, List<Contact>> _index = new Dictionary<string, List<Contact>>();
    private readonly Func<Contact, string> _keySelector;

    public ContactIndex(Func<Contact, string> keySelector)
    {
        _keySelector = keySelector;
    }

    public void Add(Contact contact)
    {
        string key = _keySelector(contact).ToLower();
        if (!_index.ContainsKey(key))
            _index[key] = new List<Contact>();
        _index[key].Add(contact);
    }

    public void Remove(Contact contact)
    {
        string key = _keySelector(contact).ToLower();
        if (_index.ContainsKey(key))
            _index[key].Remove(contact);
    }

    public IEnumerable<Contact> Search(string query)
    {
        string key = query.ToLower();
        var results = new List<Contact>();
        foreach (var kvp in _index)
            if (kvp.Key.Contains(key))
                foreach (var c in kvp.Value)
                    results.Add(c);
        return results;
    }
}