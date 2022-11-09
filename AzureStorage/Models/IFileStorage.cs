using System; 
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares;
using Microsoft.WindowsAzure.Storage.File;

namespace AzureStorage.Models
{
    public interface IFileStorage
    {
        Task<bool> CreateShareAsync(string shareName);
        void CreateFileinRootDirectoryAsync(string shareName);
        Task<bool> UploadFile();
        FileResultSegment GetAllFilesFromDirectory(string fileShareName, string directoryName);
        Task DeleteAllAsync(string shareName, string directoryName);
    }
}

