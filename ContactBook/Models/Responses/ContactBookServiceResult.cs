using System.Collections.Generic;
using ContactBook.Enums;
using ContactBook.Interfaces;

namespace ContactBook.Models.Responses
{
    public class ContactBookServiceResult : IContactServiceResult
    {
        public bool Success { get; set; }
        public List<ICustomer>? Result { get; set; }
        public Enums.ContactBookServiceResultStatus Status { get; set; }
        object ? IContactServiceResult.Result { get; set; }
        ContactBookServiceResultStatus IContactServiceResult.Status { get; set; }
    }
}
