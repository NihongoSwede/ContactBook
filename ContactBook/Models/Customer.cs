using ContactBook.Interfaces;
using System;

public class Customer(
    string firstName,
    string lastName,
    string email,
    string phoneNumber,
    string address,
    string city,
    string postalCode,
    string country) : ICustomer
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Address { get; set; } = address;
    public string City { get; set; } = city;
    public string PostalCode { get; set; } = postalCode;
    public string Country { get; set; } = country;



    public bool Equals(ICustomer other)
    {
        if (other == null)
            return false;

        return Email.Equals(other.Email, StringComparison.OrdinalIgnoreCase);
    }
}
