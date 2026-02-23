var service = new ContactService();

var handlers = new Dictionary<string, IContactHandler>
{
    { "1", new AddContactHandler(service) },
    { "2", new EditContactHandler(service) },
    { "3", new DeleteContactHandler(service) },
    { "4", new ViewContactHandler(service) },
    { "5", new ListContactsHandler(service) },
    { "6", new SearchContactHandler(service) },
    { "7", new FilterContactHandler(service) },
    { "8", new SaveContactsHandler() },
};

new MainMenu(handlers).Run();