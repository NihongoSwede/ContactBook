using ContactBook.Enums;
using ContactBook.Interfaces;
using ContactBook.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class CustomerService : ICustomerService
{
    private readonly IFileService _fileService;
    private readonly string _filePath;
    private List<ICustomer> _customerList;

    public CustomerService(IFileService fileService, string filePath = @"c:\Users\mhede\source\repos\ContactBook\customers.json")
    {
        _fileService = fileService;
        _filePath = filePath;
        LoadCustomerListFromFile();
    }

    public void SaveCustomerListToFile()
    {
        var json = JsonConvert.SerializeObject(_customerList, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
        });

        _fileService.SaveToFile(_filePath, json);
    }

    public bool AddToList(ICustomer customer)
    {
        if (customer == null)
        {
            Debug.WriteLine("Customer is null.");
            return false;
        }

        if (_customerList.Contains(customer))
        {
            Debug.WriteLine($"Customer with email {customer.Email} already exists.");
            return false;
        }

        _customerList.Add(customer);
        SaveCustomerListToFile();
        return true;
    }

    public IEnumerable<ICustomer> GetAllFromList()
    {
        
        return _customerList;
    }

    public ContactBookServiceResult DeleteCustomerByEmail(string email)
    {
        var response = new ContactBookServiceResult();
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

        return response;
    }

    public ContactBookServiceResult UpdateCustomer(ICustomer updatedCustomer)
    {
        var response = new ContactBookServiceResult();
        int index = _customerList.FindIndex(customer => customer.Email == updatedCustomer.Email);

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

    public void LoadCustomerListFromFile()
    {
        var content = _fileService.GetContentFromFile(_filePath);

        try
        {
            var loadedList = JsonConvert.DeserializeObject<List<ICustomer>>(content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
            });

            _customerList = loadedList?.GroupBy(c => c.Email).Select(g => g.First()).ToList() ?? [];
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deserializing customer list: {ex.Message}");
            _customerList = [];
        }
    }
}
