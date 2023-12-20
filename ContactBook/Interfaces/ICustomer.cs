namespace ContactBook.Interfaces
{
    public interface ICustomer
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string PhoneNumber { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string PostalCode { get; set; }
        string Country { get; set; }
    }
}