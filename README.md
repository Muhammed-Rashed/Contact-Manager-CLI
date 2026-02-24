# Contact-Manager-CLI

A Simple C# program that helps in Managing the contacts of the user for effective communication using a **modern contact indexing system**

[TOC]

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or any C# compatible IDE

## How to Run

### Option 1: Visual Studio

1. Clone or download the repository
2. Open the `.sln` file in Visual Studio 2022
3. Right-click the project - **Properties** - **Application** - make sure Target Framework is `.NET 8.0`
4. Place `contacts.json` in the project folder and set it to **Copy Always**:
   - Right-click `contacts.json` in Solution Explorer
   - Properties --> Copy to Output Directory --> **Copy Always**
5. Press `Ctrl+F5` to run

### Option 2: Command Line

```bash
git clone https://github.com/Muhammed-Rashed/Contact-Manager-CLI.git
cd Contact-Manager-CLI/CLI
dotnet run
```

---

## Features

| Option | Feature        | Description                                 |
| ------ | -------------- | ------------------------------------------- |
| 1      | Add Contact    | Add a new contact with Name, Phone, Email   |
| 2      | Edit Contact   | Search for a contact and edit their details |
| 3      | Delete Contact | Search for a contact and delete them        |
| 4      | View Contacts  | Show full details of all contacts           |
| 5      | List Contacts  | Show ID and Name of all contacts            |
| 6      | Search         | Search across all fields with one query     |
| 7      | Filter         | Filter by Name, Email, Phone, or Date range |
| 8      | Save           | Save all contacts to contacts.json          |
| 9      | Exit           | Exit the application                        |

---

## Data Storage

Contacts are stored in `contacts.json` next to the `.exe` file. The file is loaded on startup and saved when the user selects **Save (8)**.

Each contact contains:

```json
{
  "Id": "(000-abx-xxx-xxx)",
  "Name": "John Doe",
  "Phone": "01012345678",
  "Email": "john@gmail.com",
  "CreationDate": "2026-02-24T14:30:00"
}
```

---

## Architecture

The system is built around **OOP** and **SOLID** principles:

- **Single Responsibility**: each class has one job
- **Open/Closed**: adding a new searchable field requires only one new line in `ContactService`
- **Liskov Substitution**: all handlers are interchangeable via `IContactHandler`
- **Interface Segregation**: `IContactService`, `IContactRepository`, `IContactSelector` are separate contracts
- **Dependency Inversion**: all classes depend on interfaces, not concrete implementations

### UML diagram

``` mermaid
classDiagram

    class Contact {
        +Guid Id
        +string Name
        +string Phone
        +string Email
        +DateTime CreationDate
        +Contact()
        +Contact(name, phone, email)
    }

    class ContactIndex {
        -Dictionary~string,List~Contact~~ _index
        -Func~Contact,string~ _keySelector
        +ContactIndex(keySelector)
        +Add(contact) void
        +Remove(contact) void
        +Search(query) IEnumerable~Contact~
    }

    class IContactRepository {
        <<interface>>
        +Save(contacts) Task
        +Load() Task~List~Contact~~
    }

    class JsonContactRepository {
        -string filePath
        +JsonContactRepository(path)
        +Save(contacts) Task
        +Load() Task~List~Contact~~
    }

    class IContactService {
        <<interface>>
        +Load() Task
        +Save() Task
        +Add(name, phone, email) Task~Contact~
        +GetById(id) Task~Contact~
        +GetByQuery(query) Task~List~Contact~~
        +GetAll() Task~IReadOnlyList~Contact~~
        +Edit(id, name, phone, email) Task~bool~
        +Delete(id) Task~bool~
        +Search(query) Task~IEnumerable~Contact~~
        +Filter(name, email, phone, after, before) Task~IEnumerable~Contact~~
        +Count int
    }

    class AddService {
        -Dictionary~Guid,Contact~ _byId
        -List~ContactIndex~ _indexes
        +AddService(byId, indexes)
        +Add(name, phone, email) Task~Contact~
    }

    class EditService {
        -Dictionary~Guid,Contact~ _byId
        -List~ContactIndex~ _indexes
        +EditService(byId, indexes)
        +Edit(id, name, phone, email) Task~bool~
    }

    class DeleteService {
        -Dictionary~Guid,Contact~ _byId
        -List~ContactIndex~ _indexes
        +DeleteService(byId, indexes)
        +Delete(id) Task~bool~
    }

    class SearchService {
        -Dictionary~Guid,Contact~ _byId
        -List~ContactIndex~ _indexes
        +SearchService(byId, indexes)
        +Search(query) Task~IEnumerable~Contact~~
    }

    class FilterService {
        -Dictionary~Guid,Contact~ _byId
        +FilterService(byId)
        +Filter(name, email, phone, after, before) Task~IEnumerable~Contact~~
    }

    class SaveService {
        -Dictionary~Guid,Contact~ _byId
        -IContactRepository _repository
        +SaveService(byId, repository)
        +Save() Task
    }

    class LoadService {
        -Dictionary~Guid,Contact~ _byId
        -List~ContactIndex~ _indexes
        -IContactRepository _repository
        +LoadService(byId, indexes, repository)
        +Load() Task
    }

    class ContactService {
        -Dictionary~Guid,Contact~ _byId
        -List~ContactIndex~ _indexes
        -AddService _addService
        -EditService _editService
        -DeleteService _deleteService
        -SearchService _searchService
        -FilterService _filterService
        -SaveService _saveService
        -LoadService _loadService
        +ContactService(repository)
        +Load() Task
        +Save() Task
        +Add(name, phone, email) Task~Contact~
        +GetById(id) Task~Contact~
        +GetByQuery(query) Task~List~Contact~~
        +GetAll() Task~IReadOnlyList~Contact~~
        +Edit(id, name, phone, email) Task~bool~
        +Delete(id) Task~bool~
        +Search(query) Task~IEnumerable~Contact~~
        +Filter(name, email, phone, after, before) Task~IEnumerable~Contact~~
        +Count int
    }

    class IContactSelector {
        <<interface>>
        +Select(action) Task~Contact~
    }

    class ContactSelector {
        -IContactService _service
        +ContactSelector(service)
        +Select(action) Task~Contact~
    }

    class IContactHandler {
        <<interface>>
        +Handle() void
    }

    class BaseHandler {
        <<abstract>>
        #SectionHeader(title) void
        #PrintContactCard(contact) void
        #DisplayError(message) void
        #PromptRequired(label) string
        #PromptDecimal(label) decimal
        #Pause() void
    }

    class AddContactHandler {
        -IContactService _service
        +AddContactHandler(service)
        +Handle() void
    }

    class EditContactHandler {
        -IContactService _service
        -IContactSelector _selector
        +EditContactHandler(service, selector)
        +Handle() void
    }

    class DeleteContactHandler {
        -IContactService _service
        -IContactSelector _selector
        +DeleteContactHandler(service, selector)
        +Handle() void
    }

    class ViewContactHandler {
        -IContactService _service
        +ViewContactHandler(service)
        +Handle() void
    }

    class ListContactsHandler {
        -IContactService _service
        +ListContactsHandler(service)
        +Handle() void
    }

    class SearchContactHandler {
        -IContactService _service
        +SearchContactHandler(service)
        +Handle() void
    }

    class FilterContactHandler {
        -IContactService _service
        +FilterContactHandler(service)
        +Handle() void
    }

    class SaveContactsHandler {
        -IContactService _service
        +SaveContactsHandler(service)
        +Handle() void
    }

    class MainMenu {
        -Dictionary~string,IContactHandler~ _handlers
        -IContactService _service
        +MainMenu(handlers, service)
        +Run() void
        -Display() void
        -DisplayWelcomeBanner() void
    }

    class Program {
        +Main(args) void$
    }

    IContactRepository <|.. JsonContactRepository 
    IContactService <|.. ContactService 
    IContactSelector <|.. ContactSelector 
    IContactHandler <|.. BaseHandler 
    BaseHandler <|-- AddContactHandler 
    BaseHandler <|-- EditContactHandler 
    BaseHandler <|-- DeleteContactHandler 
    BaseHandler <|-- ViewContactHandler 
    BaseHandler <|-- ListContactsHandler 
    BaseHandler <|-- SearchContactHandler 
    BaseHandler <|-- FilterContactHandler 
    BaseHandler <|-- SaveContactsHandler 

    ContactService --> AddService 
    ContactService --> EditService 
    ContactService --> DeleteService 
    ContactService --> SearchService 
    ContactService --> FilterService 
    ContactService --> SaveService 
    ContactService --> LoadService 
    ContactService --> ContactIndex 
    ContactService --> IContactRepository 

    AddService --> ContactIndex 
    EditService --> ContactIndex 
    DeleteService --> ContactIndex 
    SearchService --> ContactIndex 

    SaveService --> IContactRepository 
    LoadService --> IContactRepository 
    LoadService --> ContactIndex 

    ContactSelector --> IContactService 
    EditContactHandler --> IContactService 
    EditContactHandler --> IContactSelector 
    DeleteContactHandler --> IContactService 
    DeleteContactHandler --> IContactSelector 
    AddContactHandler --> IContactService 
    ViewContactHandler --> IContactService 
    ListContactsHandler --> IContactService 
    SearchContactHandler --> IContactService 
    FilterContactHandler --> IContactService 
    SaveContactsHandler --> IContactService 

    MainMenu --> IContactHandler 
    MainMenu --> IContactService 

    Program --> MainMenu 
    Program --> IContactService 
    Program --> IContactRepository 
    Program --> IContactSelector 

    ContactService --> Contact 
    JsonContactRepository --> Contact 
```

### Search Performance

Instead of scanning all contacts linearly, the system maintains a `List<ContactIndex>` where each index is a hash table keyed by a contact field. Search checks all indexes and merges results using a `HashSet` to avoid duplicates.

| Operation | Complexity                       |
| --------- | -------------------------------- |
| Add       | O(1)                             |
| GetById   | O(1)                             |
| Search    | O(n) where n = total data points |
| Delete    | O(1)                             |
| Edit      | O(1)                             |

---

## Notes

- Data is only persisted when you choose **Save (8)** from the menu
- If no `contacts.json` exists the app starts with an empty list
- The app uses `async/await` throughout to avoid blocking operations
  