using ContactBook.Enums;
using ContactBook.Interfaces;

namespace ContactBook.Services
{
    public class ContactBookMenuService : IContactMenuService
    {
        private readonly ICustomerService _customerService;

        public ContactBookMenuService(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                DisplayMenuTitle("Menu Options");

                Console.WriteLine($" {"1.",-3} Add new Customer ");
                Console.WriteLine($" {"2.",-3} View Customer list");
                Console.WriteLine($" {"3.",-3} Delete customer by Email");
                Console.WriteLine($" {"4.",-3} Change customer by Email");
                Console.WriteLine($" {"5.",-3} Show customer by Email");
                Console.WriteLine($" {"6.",-3} Exit customer application");
                Console.WriteLine();
                Console.WriteLine("Enter Menu option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowAddCustomerOption(new Customer("", "", "", "", "", "", "", ""));
                        break;
                    case "2":
                        ShowViewCustomerListOption();
                        break;
                    case "3":
                        DeleteCustomerByEmailOption();
                        break;
                    case "4":
                        ShowChangeCustomerOption();
                        break;
                    case "5":
                        ShowOneCustomerByEmailOption();
                        break;
                    case "6":
                        ShowExitApplicationOption();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Option selected. Press any key to try again");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void ShowExitApplicationOption()
        {
            Console.Clear();
            Console.Write("Are you sure you want to exit to close the program? (y/n)");
            var option = Console.ReadLine() ?? "";

            if (option.Equals("y", StringComparison.CurrentCultureIgnoreCase))
            {
                Environment.Exit(0);
            }
        }

        public void ShowAddCustomerOption(Customer customer)
        {
            DisplayMenuTitle("Add new customer");

            // Get user input for customer details
            Console.Write("First Name: ");
            string firstName = Console.ReadLine()!;

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine()!;

            Console.Write("Email: ");
            string email = Console.ReadLine()!;

            Console.Write("PhoneNumber: ");
            string phoneNumber = Console.ReadLine()!;

            Console.Write("Adress: ");
            string address = Console.ReadLine()!;

            Console.Write("City: ");
            string city = Console.ReadLine()!;

            Console.Write("PostalCode: ");
            string postalCode = Console.ReadLine()!;

            Console.Write("Country: ");
            string country = Console.ReadLine()!;

            // Create a new customer with the provided details
            var newCustomer = new Customer(firstName, lastName, email, phoneNumber, address, city, postalCode, country);

            // Add the customer to the list
            var result = _customerService.AddToList(newCustomer); 

            if (result)
            {
                Console.WriteLine("Customer added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add the customer. Please try again.");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        public void ShowViewCustomerListOption()
        {
            
            DisplayMenuTitle("View Customer List");

            
            var customers = _customerService.GetAllFromList();

            if (customers == null)
            {
                Console.WriteLine("No customers in the list.");
            }
            else
            {
                Console.Clear();
                foreach (var customer in customers)
                {
                    Console.WriteLine($"Name: {customer.FirstName}, {customer.LastName}");
                    Console.WriteLine($"Email: {customer.Email}");
                    Console.WriteLine($"Phone: {customer.PhoneNumber}");
                    Console.WriteLine($"Address: {customer.Address}");
                    Console.WriteLine($"City: {customer.City}");
                    Console.WriteLine($"PostalCode: {customer.PostalCode}");
                    Console.WriteLine($"Country: {customer.Country}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }




        public void DeleteCustomerByEmailOption()
        {
            DisplayMenuTitle("Delete Customer by Email");

            Console.Write("Enter Email to delete: ");
            var emailToDelete = Console.ReadLine();

            var result = _customerService.DeleteCustomerByEmail(emailToDelete!);

            if (result.Status == ContactBookServiceResultStatus.SUCCEDED)
            {
                Console.WriteLine("Customer deleted successfully!");
            }
            else if (result.Status == ContactBookServiceResultStatus.NOT_FOUND)
            {
                Console.WriteLine("Customer with the specified Email not found.");
            }
            else
            {
                Console.WriteLine("Failed to delete the customer. Please try again.");
            }
        }

        public void ShowChangeCustomerOption()
        {
            DisplayMenuTitle("Change Customer Information");

            Console.Write("Enter Email of the customer to change: ");
            var emailToChange = Console.ReadLine();

            var existingCustomer = _customerService.GetAllFromList()?.FirstOrDefault(c => c.Email == emailToChange);

            if (existingCustomer != null)
            {
                Console.WriteLine($"Current Information for Email {emailToChange}:");
                Console.WriteLine($"Name: {existingCustomer.FirstName} {existingCustomer.LastName}, Email: {existingCustomer.Email}, Phone: {existingCustomer.PhoneNumber}, Address: {existingCustomer.Address}, City: {existingCustomer.City}, PostalCode: {existingCustomer.PostalCode}, Country: {existingCustomer.Country}");

                Console.WriteLine();

                Console.Write("Enter new First Name (leave blank to keep existing): ");
                string newFirstName = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newFirstName))
                {
                    existingCustomer.FirstName = newFirstName;
                }

                Console.Write("Enter new Last Name (leave blank to keep existing): ");
                string newLastName = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newLastName))
                {
                    existingCustomer.LastName = newLastName;
                }

                Console.Write("Enter new Email (leave blank to keep existing): ");
                string newEmail = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newEmail))
                {
                    existingCustomer.Email = newEmail;
                }

                Console.Write("Enter new Phone Number (leave blank to keep existing): ");
                string newPhoneNumber = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newPhoneNumber))
                {
                    existingCustomer.PhoneNumber = newPhoneNumber;
                }

                Console.Write("Enter new Address (leave blank to keep existing): ");
                string newAddress = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newAddress))
                {
                    existingCustomer.Address = newAddress;
                }

                Console.Write("Enter new City (leave blank to keep existing): ");
                string newCity = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newCity))
                {
                    existingCustomer.City = newCity;
                }

                Console.Write("Enter new Postal Code (leave blank to keep existing): ");
                string newPostalCode = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(newPostalCode))
                {
                    existingCustomer.PostalCode = newPostalCode;
                }

                var updateResult = _customerService.UpdateCustomer(existingCustomer);

                if (updateResult.Status == ContactBookServiceResultStatus.SUCCEDED)
                {
                    Console.WriteLine("Customer information updated successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to update the customer. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Customer with the specified Email not found.");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        public void ShowOneCustomerByEmailOption()
        {
            DisplayMenuTitle("Print Customer by Email");

            Console.Write("Enter Email to print: ");
            var emailToPrint = Console.ReadLine()!;
            Console.Clear();

            Console.WriteLine("");
            _customerService.PrintCustomerByEmail(emailToPrint);

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }

        private static void DisplayMenuTitle(string title)
        {
            Console.Clear();
            Console.WriteLine($"## {title} ##");
            Console.WriteLine();
        }

        
    }
}
