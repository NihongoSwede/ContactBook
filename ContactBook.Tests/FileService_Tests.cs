using ContactBook.Interfaces;
using ContactBook.Services;
using System;
using Xunit;

namespace ContactBook.Tests
{
    public class FileService_Tests
    {

        [Fact]
        public void EditFileContentShould_ReturnTrue_IfContentEditedSuccessfully()
        {
            // Arrange
            IFileService fileService = new FileService();
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
    }
}
