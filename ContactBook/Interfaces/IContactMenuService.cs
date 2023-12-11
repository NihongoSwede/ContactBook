namespace ContactBook.Interfaces
{
    public interface IContactMenuService
    {
        void ShowMainMenu();
        void ShowExitApplicationOption();
        void ShowAddCustomerOption(Customer customer);
        void ShowViewCustomerListOption();
        void DeleteCustomerByEmailOption();
        void ShowChangeCustomerOption();
        void ShowOneCustomerByEmailOption();
    }
}
