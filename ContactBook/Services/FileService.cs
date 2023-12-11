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
                if (File.Exists(filepath))
                {
                    return File.ReadAllText(filepath);
                }
                else
                {
                    Debug.WriteLine($"File not found: {filepath}");
                }
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine($"File not found: {ex.Message}");
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"IO error reading file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred reading file: {ex.Message}");
            }

            return null!;
        }

        public bool SaveToFile(string filePath, string content)
        {
            try
            {
                var directoryPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath!);
                }

                File.WriteAllText(filePath, content);

                return true;
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"IO error saving file: {ex.Message}");
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

                if (fileContent == null)
                {
                    Debug.WriteLine($"File content is null. Cannot perform edit.");
                    return false;
                }

                fileContent = fileContent.Replace(originalContent, editedContent, StringComparison.OrdinalIgnoreCase);

                SaveToFile(filepath, fileContent);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error editing file content: {ex.Message}");
                return false;
            }
        }
    }
}
