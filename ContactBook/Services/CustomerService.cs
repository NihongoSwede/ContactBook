using System;
using System.Collections.Generic;
using ContactBook.Enums;
using ContactBook.Interfaces;
using ContactBook.Models;
using ContactBook.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using ContactBook.Services;

namespace ContactBook.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IFileService _fileService;
        private readonly string _filePath;

        private List<ICustomer> _customerList = new List<ICustomer>();

        public CustomerService(IFileService fileService, string filePath = @"c:\Users\mhede\source\repos\ContactBook\customers.json")
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _filePath = filePath;
        }

        public bool AddToList(ICustomer customer)
        {
            try
            {
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
                if (_customerList == null || !_customerList.Any())
                {
                    LoadCustomerListFromFile();
                }

                return _customerList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting all customers from list: {ex.Message}");
                return Enumerable.Empty<ICustomer>();
            }
        }

        public ContactBookServiceResult DeleteCustomerByEmail(string email)
        {
            ContactBookServiceResult response = new ContactBookServiceResult();

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
                Console.WriteLine($"Name: {customer.FirstName}, {customer.LastName}, Email: {customer.Email}, Phone: {customer.PhoneNumber}, Adress: {customer.Address}, City: {customer.City}, Postalcode: {customer.PostalCode}, Country: {customer.Country}");
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

        private void SaveCustomerListToFile()
        {
            var json = JsonConvert.SerializeObject(_customerList, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
            });

            _fileService.SaveToFile(_filePath, json);
        }

        private void LoadCustomerListFromFile()
        {
            var content = _fileService.GetContentFromFile(_filePath);

            if (!string.IsNullOrEmpty(content))
            {
                _customerList = JsonConvert.DeserializeObject<List<ICustomer>>(content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                }) ?? new List<ICustomer>();
            }
        }
    }
}
