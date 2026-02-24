using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

string path = Path.GetFullPath("contacts.json");
Console.WriteLine($"Looking for: {path}");
Console.WriteLine($"File exists: {File.Exists(path)}");

IContactRepository repository = new JsonContactRepository("contacts.json");
IContactService service = new ContactService(repository);

await service.Load();

Console.WriteLine($"Contacts loaded: {service.Count}");

var handlers = new Dictionary<string, IContactHandler>
{
    { "1", new AddContactHandler(service)    },
    { "2", new EditContactHandler(service)   },
    { "3", new DeleteContactHandler(service) },
    { "4", new ViewContactHandler(service)   },
    { "5", new ListContactsHandler(service)  },
    { "6", new SearchContactHandler(service) },
    { "7", new FilterContactHandler(service) },
    { "8", new SaveContactsHandler(service)          },
};

new MainMenu(handlers).Run();