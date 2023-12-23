using ContactBook.Enums;

namespace ContactBook.Interfaces
{
    public interface IContactServiceResult
    {
        object Result { get; set; }
        ContactBookServiceResultStatus Status { get; set; }
    }
}
