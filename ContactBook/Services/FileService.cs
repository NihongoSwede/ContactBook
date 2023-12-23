using ContactBook.Interfaces;
using System.Diagnostics;

namespace ContactBook.Services
{
    public class FileService : IFileService
    {
        /// <summary>
        /// This here is used to retrieve content from file 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns> FilePath</returns>
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

        /// <summary>
        /// This function here saves the content to the file 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <returns>Saved file</returns>
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

        /// <summary>
        /// This function here edits the file content 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="originalContent"></param>
        /// <param name="editedContent"></param>
        /// <returns>Editedfile content and saves it to the current file</returns>
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
