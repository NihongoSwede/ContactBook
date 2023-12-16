using ContactBook.Interfaces;
using System.Diagnostics;

namespace ContactBook.Services
{
    public class FileService : IFileService
    {

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
