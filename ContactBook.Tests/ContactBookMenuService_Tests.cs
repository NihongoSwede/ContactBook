using ContactBook.Interfaces;
using ContactBook.Services;
using Moq;

namespace ContactBook.Tests
{
    public class ContactBookMenuService_Tests
    {
        [Fact]
        public void ShowExitApplicationOption_ExitConfirmed_ShouldExitApplication()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var menuService = new ContactBookMenuService(customerServiceMock.Object);

            using (var sw = new StringWriter())
            {
                // Redirect standard output
                Console.SetOut(sw);

                using (var sr = new StringReader("y\n"))
                {
                    // Redirect standard input
                    Console.SetIn(sr);

                    // Act
                    menuService.ShowExitApplicationOption();
                }

                // Assert
                Assert.Equal("Are you sure you want to exit to close the program? (y/n)", sw.ToString().Trim());
            }
        }

        [Fact]
        public void ShowAddCustomerOption_ValidCustomer_ShouldAddCustomerSuccessfully()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var menuService = new ContactBookMenuService(customerServiceMock.Object);

            using (var sw = new StringWriter())
            using (var sr = new StringReader("John\nDoe\njohn@example.com\n123-456-7890\n123 Main St\nCity\n12345\nCountry\n"))
            {
                // Redirect standard output
                Console.SetOut(sw);

                // Redirect standard input
                Console.SetIn(sr);

                // Act
                menuService.ShowAddCustomerOption(new Customer("", "", "", "", "", "", "", ""));

                // Assert
                Assert.Equal("Customer added successfully!", sw.ToString().Trim());
                customerServiceMock.Verify(cs => cs.AddToList(It.IsAny<Customer>()), Times.Once);
            }
        }

        [Fact]
        public void ShowAddCustomerOption_InvalidCustomer_ShouldDisplayErrorMessage()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            var menuService = new ContactBookMenuService(customerServiceMock.Object);

            using (var sw = new StringWriter())
            using (var sr = new StringReader("\n\ninvalid-email\n\n"))
            {
                // Redirect standard output
                Console.SetOut(sw);

                // Redirect standard input
                Console.SetIn(sr);

                // Act
                menuService.ShowAddCustomerOption(new Customer("", "", "", "", "", "", "", ""));

                // Assert
                Assert.Contains("Failed to add the customer. Please try again.", sw.ToString().Trim());
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

            using (var sw = new StringWriter())
            {
                // Redirect standard output
                Console.SetOut(sw);

                // Act
                menuService.ShowViewCustomerListOption();

                // Assert
                Assert.Contains("No customers in the list.", sw.ToString().Trim());
            }
        }
    }
}
