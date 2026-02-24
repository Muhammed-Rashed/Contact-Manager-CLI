using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

IContactRepository repository = new JsonContactRepository("contacts.json");
IContactService service = new ContactService(repository);
IContactSelector selector = new ContactSelector(service);

await service.Load();

var handlers = new Dictionary<string, IContactHandler>
{
    { "1", new AddContactHandler(service)              },
    { "2", new EditContactHandler(service, selector)   },
    { "3", new DeleteContactHandler(service, selector) },
    { "4", new ViewContactHandler(service)             },
    { "5", new ListContactsHandler(service)            },
    { "6", new SearchContactHandler(service)           },
    { "7", new FilterContactHandler(service)           },
    { "8", new SaveContactsHandler(service)            },
};

new MainMenu(handlers, service).Run();