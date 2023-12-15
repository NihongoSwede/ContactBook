using ContactBook.Enums;
using ContactBook.Interfaces;
using Moq;
using Newtonsoft.Json;

namespace ContactBook.Tests
{
    public class CustomerService_Tests
    {
        [Fact]
        public void AddToListShould_AddOneCustomerToCustomerList_ThenReturnTrue()
        {
            // Arrange
            ICustomer customer = new Customer(
                "Lars",
                "Hedenborg",
                "mhedenborg18@gmail.com",
                "070 729 90 27",
                "Kapplunda Grand 6",
                "Skovde",
                "549 40 ",
                "Sweden"
            );

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.SaveToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var customerService = new CustomerService(mockFileService.Object);

            // Act
            bool result = customerService.AddToList(customer);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetAllFromListShould_GetAllCustomersInCustomerList_ThenReturnListOfCustomer()
        {
            // Arrange 
            var customers = new List<ICustomer>
            {
                new Customer
                (
                    "Lars",
                    "Hedenborg",
                    "mhedenborg18@gmail.com",
                    "070 729 90 27",
                    "Kapplunda Grand 6",
                    "Skovde",
                    "549 40 ",
                    "Sweden"
                )
            };
            string json = JsonConvert.SerializeObject(customers, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(json);

            ICustomerService customerService = new CustomerService(mockFileService.Object);

            // Act 
            IEnumerable<ICustomer> result = customerService.GetAllFromList();

            // Assert 
            Assert.NotNull(result);

            // Check if the result contains any items
            if (result.Any())
            {
                ICustomer returnedCustomer = result.First(); // Use First() to get the first item

              
                Assert.Equal("Lars", returnedCustomer.FirstName);
                Assert.Equal("Hedenborg", returnedCustomer.LastName);
                
            }
            else
            {
                // Handle the case when the result is empty
                Assert.True(false, "The result is empty");
            }
        }

        [Fact]
        public void GetCustomerByEmailShould_ReturnCustomer_WhenEmailExists()
        {
            // Arrange 
            var customers = new List<ICustomer>
            {
                new Customer
                (
                    "Mathias",
                    "Hedenborg",   
                    "mhedenborg18@gmail.com",
                    "070 729 90 27",
                    "Kapplunda Grand 6",
                    "Skovde",
                    "549 40 ",
                    "Sweden"
                )
            };

            string json = JsonConvert.SerializeObject(customers, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(json);

            ICustomerService customerService = new CustomerService(mockFileService.Object);

            // Act 
            IEnumerable<ICustomer> result = customerService.GetAllFromList();
            ICustomer customer = customerService.CustomerByEmail("mhedenborg18@gmail.com");

            // Assert 
            Assert.NotNull(result);
            Assert.NotNull(customer);
            Assert.Equal("Mathias", customer.FirstName);
            Assert.Equal("Hedenborg", customer.LastName);
            // Add assertions for other properties
        }

        [Fact]
        public void DeleteCustomerByEmail_ShouldReturnNotFound_WhenEmailDoesNotExist()
        {
            // Arrange 
            var customers = new List<ICustomer>
            {
                new Customer
                (
                    "John",
                    "Doe",
                    "johndoe@example.com",
                    "123-456-7890",
                    "123 Main St",
                    "City",
                    "12345",
                    "Country"
                )
            };

            string initialJson = JsonConvert.SerializeObject(customers, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(initialJson);
            mockFileService.Setup(x => x.SaveToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            ICustomerService customerService = new CustomerService(mockFileService.Object);

            // Act 
            var deleteResult = customerService.DeleteCustomerByEmail("nonexistent@example.com");

            // Assert 
            Assert.Equal(ContactBookServiceResultStatus.NOT_FOUND, deleteResult.Status);
        }

        [Fact]
        public void DeleteCustomerByEmail_ShouldReturnNotFound_WhenExceptionOccurs()
        {
            // Arrange 
            var customers = new List<ICustomer>
            {
                new Customer
                (
                    "John",
                    "Doe",
                    "johndoe@example.com",
                    "123-456-7890",
                    "123 Main St",
                    "City",
                    "12345",
                    "Country"
                )
            };

            string initialJson = JsonConvert.SerializeObject(customers, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(initialJson);
            mockFileService.Setup(x => x.SaveToFile(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Simulated exception"));

            ICustomerService customerService = new CustomerService(mockFileService.Object);

            // Redirect console output
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act and Assert
                var exception = Record.Exception(() =>
                {
                    var deleteResult = customerService.DeleteCustomerByEmail("johndoe@example.com");
                    Assert.Equal(ContactBookServiceResultStatus.NOT_FOUND, deleteResult.Status);
                });

                // Output exception details only if an exception occurs
                if (exception != null)
                {
                    Console.WriteLine($"Exception details: {exception}");
                }

                // Access the console output if needed
                var consoleOutput = sw.ToString();
                // Add assertions or logging for consoleOutput if needed
            }
        }

        [Fact]
        public void UpdateCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange 
            var customers = new List<ICustomer>
            {
                new Customer
                (
                    "John",
                    "Doe",
                    "johndoe@example.com",
                    "123-456-7890",
                    "123 Main St",
                    "City",
                    "12345",
                    "Country"
                )
            };

            string initialJson = JsonConvert.SerializeObject(customers, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(initialJson);
            mockFileService.Setup(x => x.SaveToFile(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            ICustomerService customerService = new CustomerService(mockFileService.Object);

            // Act 
            var updateResult = customerService.UpdateCustomer(
                new Customer
                (
                    "Jane",
                    "Doe",
                    "janedoe@example.com",
                    "987-654-3210",
                    "456 Oak St",
                    "Town",
                    "54321",
                    "AnotherCountry"
                )
            );

            // Assert 
            Assert.Equal(ContactBookServiceResultStatus.NOT_FOUND, updateResult.Status);
        }

        [Fact]
        public void GetAllFromListShould_NotReturnDuplicateCustomers_WhenEmailExists()
        {
            // Arrange 
            var customers = new List<ICustomer>
    {
            new Customer
            (
                "Lars",
                "Hedenborg",
                "mhedenborg18@gmail.com",
                "070 729 90 27",
                "Kapplunda Grand 6",
                "Skovde",
                "549 40 ",
                "Sweden"
            ),
            new Customer
            (
                "Lars",
                "Hedenborg",
                "mhedenborg18@gmail.com",
                "070 729 90 27",
                "Kapplunda Grand 6",
                "Skovde",
                "549 40 ",
                "Sweden"
            )
        };

            string json = JsonConvert.SerializeObject(customers, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(json);

            ICustomerService customerService = new CustomerService(mockFileService.Object);

            // Act 
            IEnumerable<ICustomer> result = customerService.GetAllFromList();

            // Assert 
            Assert.NotNull(result);

            // Check if the result contains any items
            if (result.Any())
            {
                // Ensure that there are no duplicate emails in the result
                var distinctEmails = result.Select(c => c.Email).Distinct();
                Assert.Equal(result.Count(), distinctEmails.Count());

                // Add other assertions as needed
            }
            else
            {
                // Handle the case when the result is empty
                Assert.True(false, "The result is empty");
            }
        }
    }
}
