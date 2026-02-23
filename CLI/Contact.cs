using System;
using System.Text.Json.Serialization;

public class Contact
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime CreationDate { get; set; }

    public Contact() { }

    public Contact(string name, string phone, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Phone = phone;
        Email = email;
        CreationDate = DateTime.Now;
    }
}