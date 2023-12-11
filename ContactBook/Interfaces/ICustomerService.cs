using ContactBook.Models.Responses;

namespace ContactBook.Interfaces
{
    public interface ICustomerService
    {
        bool AddToList(ICustomer customer);
        IEnumerable<ICustomer> GetAllFromList();
        ContactBookServiceResult DeleteCustomerByEmail(string email);
       
        ContactBookServiceResult UpdateCustomer(ICustomer updatedCustomer);

        void PrintCustomerByEmail(string email);

        ICustomer CustomerByEmail(string email);

        

    }
}