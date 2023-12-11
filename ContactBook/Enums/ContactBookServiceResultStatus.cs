using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Enums
{
    public enum ContactBookServiceResultStatus
    {
        Failed = 0,
        SUCCEDED = 1,
        ALREADY_EXISTS = 2,
        NOT_FOUND = 3,
        UPPDATED = 4,
        DELETED = 5,
        NotFound = 6,
        Error = 7
    }
}
