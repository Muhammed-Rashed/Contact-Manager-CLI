using System;

public class Contact
{
	public Guid Id { get; private set; }
	public string Name { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
	public DateTime CreationDate { get; private set; }

	public Contact(string name, string phone, string email)
	{
		Id = Guid.NewGuid();
		Name = name;
		Phone = phone;
		Email = email;
		CreationDate = DateTime.Now;
	}
}