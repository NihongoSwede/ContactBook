using System;
using System.IO;
using ContactBook.Enums;
using ContactBook.Interfaces;
using ContactBook.Models;
using ContactBook.Services;
using Moq;
using Xunit;

namespace ContactBook.Tests
{
    public class ContactBookMenuService_Tests : IDisposable
    {
        private readonly StringWriter consoleOutput;
        private readonly TextWriter originalConsoleOutput;
        private readonly TextReader originalConsoleInput;

        public StringWriter ConsoleOutput => ConsoleOutput1;

        public TextWriter OriginalConsoleOutput => OriginalConsoleOutput1;

        public TextReader OriginalConsoleInput => OriginalConsoleInput1;

        public StringWriter ConsoleOutput1 => consoleOutput;

        public TextWriter OriginalConsoleOutput1 => originalConsoleOutput;

        public TextReader OriginalConsoleInput1 => originalConsoleInput;

        public ContactBookMenuService_Tests()
        {
            consoleOutput = new StringWriter();
            originalConsoleOutput = Console.Out;
            originalConsoleInput = Console.In;
            Console.SetOut(consoleOutput);
        }

        [Fact]
        public void ShowExitApplicationOption_ExitConfirmed_ShouldExitApplication()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var menuService = new ContactBookMenuService(customerServiceMock.Object);

            using (var sr = new StringReader("y\n"))
            {
                // Redirect standard input
                Console.SetIn(sr);

                // Act
                menuService.ShowExitApplicationOption();
            }

            // Assert
            Assert.Equal("Are you sure you want to exit to close the program? (y/n)", ConsoleOutput.ToString().Trim());
        }

        [Fact]
        public void ShowAddCustomerOption_ValidCustomer_ShouldAddCustomerSuccessfully()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var menuService = new ContactBookMenuService(customerServiceMock.Object);

            using (var sr = new StringReader("John\nDoe\njohn@example.com\n123-456-7890\n123 Main St\nCity\n12345\nCountry\n"))
            {
                // Redirect standard input
                Console.SetIn(sr);

                // Act
                menuService.ShowAddCustomerOption(new Customer("", "", "", "", "", "", "", ""));

                // Assert
                Assert.Equal("Customer added successfully!", ConsoleOutput.ToString().Trim());
                customerServiceMock.Verify(cs => cs.AddToList(It.IsAny<Customer>()), Times.Once);
            }
        }

        [Fact]
        public void ShowAddCustomerOption_InvalidCustomer_ShouldDisplayErrorMessage()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var menuService = new ContactBookMenuService(customerServiceMock.Object);

            using (var sr = new StringReader("\n\ninvalid-email\n\n"))
            {
                // Redirect standard input
                Console.SetIn(sr);

                // Act
                menuService.ShowAddCustomerOption(new Customer("", "", "", "", "", "", "", ""));

                // Assert
                Assert.Contains("Failed to add the customer. Please try again.", ConsoleOutput.ToString().Trim());
                customerServiceMock.Verify(cs => cs.AddToList(It.IsAny<Customer>()), Times.Never);
            }
        }

        [Fact]
        public void ShowViewCustomerListOption_NoCustomers_ShouldDisplayNoCustomersMessage()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(cs => cs.GetAllFromList()).Returns(new Customer[0]);
            var menuService = new ContactBookMenuService(customerServiceMock.Object);

            // Act
            menuService.ShowViewCustomerListOption();

            // Assert
            Assert.Contains("No customers in the list.", ConsoleOutput.ToString().Trim());
        }

        // Add more test cases as needed...

        public void Dispose()
        {
            // Reset standard output and input after each test
            Console.SetOut(OriginalConsoleOutput);
            Console.SetIn(OriginalConsoleInput);
            ConsoleOutput.Dispose();
        }
    }
}
