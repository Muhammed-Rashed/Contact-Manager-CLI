using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class JsonContactRepository : IContactRepository
{
    private readonly string filePath;

    public JsonContactRepository(string path)
    {
        filePath = path;
    }

    public async Task Save(IEnumerable<Contact> contacts)
    {
        string json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<List<Contact>> Load()
    {
        if (!File.Exists(filePath))
            return new List<Contact>();

        string json = await File.ReadAllTextAsync(filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<Contact>();

        return JsonSerializer.Deserialize<List<Contact>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}