using ContactBook.Interfaces;
using System.Diagnostics;

namespace ContactBook.Services
{
    public class FileService : IFileService
    {
        //This here is used to retrieve content from file 
        public string GetContentFromFile(string filepath)
        {

            try
            {
                return File.ReadAllText(filepath);
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine($"{ex.GetType().Name} reading file: {ex.Message}");
                throw;
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"{ex.GetType().Name} reading file: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred reading file: {ex.Message}");
                throw;
            }
        }

        //This function here saves the content to the file 
        public bool SaveToFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                return true;
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"{ex.GetType().Name} saving file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred saving file: {ex.Message}");
            }

            return false;
        }

        //This function here edits the file content 
        public bool EditFileContent(string filepath, string originalContent, string editedContent)
        {
            try
            {
                string fileContent = GetContentFromFile(filepath);
                fileContent = fileContent.Replace(originalContent, editedContent, StringComparison.OrdinalIgnoreCase);
                SaveToFile(filepath, fileContent);
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine($"File not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error editing file content: {ex.Message}");
            }

            return false;
        }



    }
}
