using ContactBook.Enums;
using ContactBook.Interfaces;
using ContactBook.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;

public class CustomerService : ICustomerService
{
    private readonly IFileService _fileService;
    private readonly string _filePath;

    private List<ICustomer> _customerList = new List<ICustomer>();

    public CustomerService(IFileService fileService, string filePath = @"c:\Users\mhede\source\repos\ContactBook\customers.json")
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _filePath = filePath;

        // Load customers from file during construction
        LoadCustomerListFromFile();
    }

    public bool AddToList(ICustomer customer)
    {
        try
        {
            // Check if a customer with the same email already exists
            if (_customerList.Any(c => c.Email == customer.Email))
            {
                Debug.WriteLine($"Customer with email {customer.Email} already exists.");
                return false;
            }

            _customerList.Add(customer);
            SaveCustomerListToFile();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding customer to list: {ex.Message}");
            return false;
        }
    }



    public IEnumerable<ICustomer> GetAllFromList()
    {
        try
        {
            if (_customerList != null && _customerList.Count != 0)
            {
                CheckForDuplicates();
                return _customerList!;
            }

            LoadCustomerListFromFile();

            CheckForDuplicates();

            return _customerList!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting all customers from list: {ex.Message}");
            return Enumerable.Empty<ICustomer>();
        }
    }

    private void CheckForDuplicates()
    {
        // Check for duplicates based on email
        var duplicateEmails = new HashSet<string>();
        var distinctCustomers = new List<ICustomer>();

        foreach (var customer in _customerList)
        {
            if (!duplicateEmails.Add(customer.Email))
            {
                Debug.WriteLine($"Duplicate customer with email {customer.Email} found.");
            }
            else
            {
                distinctCustomers.Add(customer);
            }
        }

        // Update _customerList with distinct customers
        _customerList.Clear();
        _customerList.AddRange(distinctCustomers);
    }



    public ContactBookServiceResult DeleteCustomerByEmail(string email)
    {
        ContactBookServiceResult response = new ContactBookServiceResult()!;

        try
        {
            var customerToRemove = _customerList.FirstOrDefault(c => c.Email == email);

            if (customerToRemove != null)
            {
                _customerList.Remove(customerToRemove);
                SaveCustomerListToFile();
                response.Status = ContactBookServiceResultStatus.SUCCEDED;
            }
            else
            {
                response.Status = ContactBookServiceResultStatus.NOT_FOUND;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting customer by email: {ex.Message}");
            response.Status = ContactBookServiceResultStatus.Failed;
        }

        return response;
    }



    public ContactBookServiceResult UpdateCustomer(ICustomer updatedCustomer)
    {
        int index = _customerList.FindIndex(customer => customer.Email == updatedCustomer.Email);

        if (index != -1)
        {
            _customerList[index] = updatedCustomer;
            SaveCustomerListToFile();
            return new ContactBookServiceResult { Status = ContactBookServiceResultStatus.SUCCEDED };
        }
        else
        {
            return new ContactBookServiceResult { Status = ContactBookServiceResultStatus.NOT_FOUND };
        }
    }

    public void PrintCustomerByEmail(string email)
    {
        var customer = _customerList.FirstOrDefault(c => c.Email == email);

        if (customer != null)
        {
            Console.WriteLine($"Name: {customer.FirstName}, {customer.LastName}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine($"Phone: {customer.PhoneNumber}");
            Console.WriteLine($"Address: {customer.Address}");
            Console.WriteLine($"City: {customer.City}");
            Console.WriteLine($"Postal Code: {customer.PostalCode}");
            Console.WriteLine($"Country: {customer.Country}");
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
    }

    public ICustomer CustomerByEmail(string email)
    {
        return _customerList.FirstOrDefault(c => c.Email == email)!;
    }

    public void SaveCustomerListToFile()
    {
        var json = JsonConvert.SerializeObject(_customerList, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
        });

        _fileService.SaveToFile(_filePath, json);
    }

    public void LoadCustomerListFromFile()
    {
        var content = _fileService.GetContentFromFile(_filePath);

        if (!string.IsNullOrEmpty(content))
        {
            try
            {
                var loadedList = JsonConvert.DeserializeObject<List<ICustomer>>(content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                });

                Debug.WriteLine($"Loaded {loadedList!.Count} customers from file.");

                // Clear the existing list before adding loaded items
                _customerList.Clear();

                if (loadedList != null)
                {
                    // Use LINQ to filter out duplicate customers based on email
                    var distinctCustomers = loadedList.GroupBy(c => c.Email).Select(g => g.First()).ToList();
                    _customerList.AddRange(distinctCustomers);

                    Debug.WriteLine($"After filtering, {_customerList.Count} distinct customers remain.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deserializing customer list: {ex.Message}");
            }
        }
    }



}
