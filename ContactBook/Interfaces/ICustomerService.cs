using ContactBook.Models.Responses;
using System.Collections.Generic;

namespace ContactBook.Interfaces
{
    public interface ICustomerService
    {
        bool Add(ICustomer customer);
        IEnumerable<ICustomer> GetAll();

        ContactBookServiceResult DeleteByEmail(string email);

        ContactBookServiceResult Update(ICustomer updatedCustomer);

        void PrintByEmail(string email);
    }
}
