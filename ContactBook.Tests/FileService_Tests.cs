using ContactBook.Interfaces;
using ContactBook.Services;
using Moq;

namespace ContactBook.Tests
{
    public class FileService_Tests
    {

        [Fact]
        public void EditFileContentShould_ReturnTrue_IfContentEditedSuccessfully()
        {
            // Arrange
            IFileService fileService = new FileService()!;
            string filepath = @"C:\Users\mhede\source\repos\ContactBook\test.txt";
            string originalContent = "Test Content";
            string editedContent = "Edited Content";


            // Act 
            bool editResult = fileService.EditFileContent(filepath, originalContent, editedContent);

            // Assert 
            Assert.True(editResult);

            // Additional Check: Verify that the content has been changed
            string updatedContent = fileService.GetContentFromFile(filepath);
            Assert.Equal(editedContent, updatedContent);
        }


        [Fact]
        public void LoadCustomerListFromFileShould_NotContainDuplicateUsers()
        {
            // Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns("JSON_CONTENT_WITH_DUPLICATE_USER");

            var customerService = new CustomerService(mockFileService.Object);

            // Act
            customerService.LoadCustomerListFromFile();

            // Assert
            IEnumerable<ICustomer> loadedCustomers = customerService.GetAllFromList();

            // Check for null loadedCustomers
            Assert.NotNull(loadedCustomers);

            // Check for duplicates based on email
            if (loadedCustomers != null)
            {
                var distinctCustomers = new HashSet<ICustomer>(loadedCustomers);
                Assert.Equal(loadedCustomers.Count(), distinctCustomers.Count);
            }
            else
            {
                Assert.True(false, "Loaded customers list is null.");
            }
        }

        [Fact]
        public void EditFileContentShould_ReturnFalse_IfFileNotFound()
        {
            // Arrange
            IFileService fileService = new FileService();
            string filepath = @"Nonexistent\File\Path.txt";
            string originalContent = "Test Content";
            string editedContent = "Edited Content";

            // Act 
            bool editResult = fileService.EditFileContent(filepath, originalContent, editedContent);

            // Assert 
            Assert.False(editResult);
        }

        [Fact]
        public void LoadCustomerListFromFileShould_ReturnEmptyList_IfFileContentIsNullOrEmpty()
        {
            // Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(string.Empty);

            var customerService = new CustomerService(mockFileService.Object);

            // Act
            customerService.LoadCustomerListFromFile();

            // Assert
            IEnumerable<ICustomer> loadedCustomers = customerService.GetAllFromList();

            Assert.NotNull(loadedCustomers);
            Assert.Empty(loadedCustomers);
        }

       


    }

}
