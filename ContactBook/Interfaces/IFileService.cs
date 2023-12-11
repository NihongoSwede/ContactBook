using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Interfaces
{
    public interface IFileService
    {
        bool SaveToFile(string filePath, string content);
        string GetContentFromFile(string Filepath);

        bool EditFileContent(string filepath, string originalContent, string editedContent);
    }
}
