using ContactBook.Interfaces;
using System;

public class Customer : ICustomer
{
    public Customer(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string address,
        string city,
        string postalCode,
        string country)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        City = city;
        PostalCode = postalCode;
        Country = country;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }

   

    public bool Equals(ICustomer other)
    {
        if (other == null)
            return false;

        return Email.Equals(other.Email, StringComparison.OrdinalIgnoreCase);
    }
}
