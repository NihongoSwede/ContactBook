using ContactBook.Enums;
using ContactBook.Interfaces;
using ContactBook.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;


public class CustomerService : ICustomerService
{
    /// <summary>
    /// The handling of the list is run to package and unpackage it as JSON
    /// </summary>
    
    public readonly IFileService _fileService;
    public readonly string _filePath;
    public List<ICustomer>? _customerList = [];

    /// <summary>
    /// This is my initialization of the variables that are used to create the filepath 
    /// </summary>
    /// <param name="fileService"></param>
    /// <param name="filePath"></param>

    public CustomerService(IFileService fileService, string filePath = @"C:\Users\mhede\source\repos\ContactBook\customers.json")
    {
        _fileService = fileService;
        _filePath = filePath;

        LoadCustomerListFromFile();

    }

    /// <summary>
    /// Function that helps to save any changes made to the data  
    /// </summary>

    public void SaveCustomerListToFile()
    {
        var json = JsonConvert.SerializeObject(_customerList, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
        });

        _fileService.SaveToFile(_filePath, json);
    }

    /// <summary>
    /// This function helps to load the data 
    /// </summary>
    public void LoadCustomerListFromFile()
    {
        try
        {
            var content = _fileService.GetContentFromFile(_filePath);
            _customerList = JsonConvert.DeserializeObject<List<ICustomer>>(content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
            }) ?? [];
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deserializing customer list: {ex.Message}");
            _customerList = [];
        }
    }


    /// <summary>
    /// Function that are related to the service itself 
    /// </summary>
    /// <param name="customer"></param>
    /// <returns> Customer to the list</returns>
    public bool AddToList(ICustomer customer)
    {
        if (customer == null)
        {
            Debug.WriteLine("Customer is null.");
            return false;
        }

        if (_customerList!.Contains(customer))
        {
            Debug.WriteLine($"Customer with email {customer.Email} already exists.");
            return false;
        }

        _customerList.Add(customer);
        SaveCustomerListToFile();
        return true;
    }

    /// <summary>
    /// This function gets all the data as a list 
    /// </summary>
    /// <returns> _customerList</returns>
    public IEnumerable<ICustomer> GetAllFromList()
    {
        
        return _customerList!;

    }

    /// <summary>
    /// This function here deletes the data basd on an input from email 
    /// </summary>
    /// <param name="email"></param>
    /// <returns>the new list with a deleted user</returns>
    public ContactBookServiceResult DeleteCustomerByEmail(string email)
    {
        var response = new ContactBookServiceResult();
        var customerToRemove = _customerList!.FirstOrDefault(c => c.Email == email);

        if (customerToRemove != null)
        {
            _customerList!.Remove(customerToRemove);
            SaveCustomerListToFile();
            response.Status = ContactBookServiceResultStatus.SUCCEDED;
        }
        else
        {
            response.Status = ContactBookServiceResultStatus.NOT_FOUND;
        }

        return response;
    }

    /// <summary>
    /// This function updates the user based on input 
    /// </summary>
    /// <param name="updatedCustomer"></param>
    /// <returns>the updated user to the current list</returns>
    public ContactBookServiceResult UpdateCustomer(ICustomer updatedCustomer)
    {
        var response = new ContactBookServiceResult();
        int index = _customerList!.FindIndex(customer => customer.Email == updatedCustomer.Email);

        if (index != -1)
        {
            _customerList[index] = updatedCustomer;
            SaveCustomerListToFile();
            response.Status = ContactBookServiceResultStatus.SUCCEDED;
        }
        else
        {
            response.Status = ContactBookServiceResultStatus.NOT_FOUND;
        }

        return response;
    }

    /// <summary>
    /// This function prints out the user based on email 
    /// </summary>
    /// <param name="email"></param>
    public void PrintCustomerByEmail(string email)
    {
        var customer = _customerList!.FirstOrDefault(c => c.Email == email);

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

}
