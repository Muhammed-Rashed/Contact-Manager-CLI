public interface IContactService
{
	Contact Add(string name, string phone, string email);
	Contact GetById(Guid id);
	Contact GetByQuery(string query);
	IReadOnlyList<Contact> GetAll();
	bool Edit(Guid id, string name, string phone, string email);
	bool Delete(Guid id);
	IEnumerable<Contact> Search(string query);
	IEnumerable<Contact> Filter(string? nameContains, string? emailContains, string? phoneContains, DateTime? createdAfter, DateTime? createdBefore);
	int Count { get; }
}