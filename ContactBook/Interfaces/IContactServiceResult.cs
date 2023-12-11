using ContactBook.Enums;
using ContactBook.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Interfaces
{
    public interface IContactServiceResult
    {
        object Result { get; set; }
        ContactBookServiceResultStatus Status { get; set; }
    }
}
