﻿using ContactBook.Enums;
using ContactBook.Interfaces;
using ContactBook.Models.Responses;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class CustomerService : ICustomerService
{
    public readonly IFileService _fileService;
    public readonly string _filePath;
    public List<ICustomer> _customerList;

    // This is my initialization of the variables that are used to create the filepath 

    public CustomerService(IFileService fileService, string filePath = @"C:\Users\mhede\source\repos\ContactBook\customers.json")
    {
        _fileService = fileService;
        _filePath = filePath;

        LoadCustomerListFromFile();
    }

    //Function that helps to save any changes made to the data  

    public void SaveCustomerListToFile()
    {
        var json = JsonConvert.SerializeObject(_customerList, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
        });

        _fileService.SaveToFile(_filePath, json);
    }

    // This function helps to load the data 
    public void LoadCustomerListFromFile()
    {
        var content = _fileService.GetContentFromFile(_filePath);

        try
        {
            var loadedList = JsonConvert.DeserializeObject<List<ICustomer>>(content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
            });

            _customerList = loadedList?.GroupBy(c => c.Email).SelectMany(g => g.Take(1)).ToList() ?? [];
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deserializing customer list: {ex.Message}");
            _customerList = [];
        }
    }

    //Function that are related to the service itself 
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

    // This function gets all the data as a readable list 
    public IEnumerable<ICustomer> GetAllFromList()
    {
        
        return new ReadOnlyCollection<ICustomer>(_customerList);
    }

    // This function here deletes the data basd on an input from email 
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

    // This function updates the user based on input 
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

    // This function prints out the user based on email 
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

}
