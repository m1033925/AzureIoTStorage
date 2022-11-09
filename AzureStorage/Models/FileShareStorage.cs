using System; 
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;

namespace AzureStorage.Models
{
    public class FileShareStorage : IFileStorage
    {
        public IConfiguration configuration;

        public FileShareStorage(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        //-------------------------------------------------
        // Create a file share
        //-------------------------------------------------
        public async Task<bool> CreateShareAsync(string shareName)
        {
            var azureStorageAccount = new CloudStorageAccount(new StorageCredentials(
                configuration.GetConnectionString("StorageAccountName"),
                configuration.GetConnectionString("StorageAccountKey")), true);

            var fileShare = azureStorageAccount.CreateCloudFileClient().GetShareReference("testfileshare");
            return await fileShare.CreateIfNotExistsAsync();

        }

        //-------------------------------------------------
        // Create a file in root directory
        //-------------------------------------------------
        public void CreateFileinRootDirectoryAsync(string shareName)
        {
            var azureStorageAccount = new CloudStorageAccount(new StorageCredentials(
                configuration.GetConnectionString("StorageAccountName"),
                configuration.GetConnectionString("StorageAccountKey")), true);

            var fileShare = azureStorageAccount.CreateCloudFileClient().GetShareReference(shareName);
            fileShare.CreateIfNotExistsAsync();

            var rootDir = fileShare.GetRootDirectoryReference();
            rootDir.GetFileReference("myfirsttestfile").UploadTextAsync("Test");

        }


        //-------------------------------------------------
        // Upload a file in file share
        //-------------------------------------------------
 
        public async Task<bool> UploadFile()
        {

            string connectionString = configuration.GetConnectionString("StorageConnectionString");


            var azureStorageAccount = new CloudStorageAccount(
                new StorageCredentials(configuration.GetConnectionString("StorageAccountName"),
                configuration.GetConnectionString("StorageAccountKey")), true);


            var fileShareName = configuration.GetConnectionString("fileShareName");



            var folderName = "directory1";

            var fileName = "Testfile.txt";

            var localFilePath = @"C:/Users/vmadmin/Downloads/testfile.txt";



            ShareClient share = new ShareClient(connectionString, fileShareName);



            var directory = share.GetDirectoryClient(folderName);



            var file = directory.GetFileClient(fileName);

            using FileStream stream = File.OpenRead(localFilePath);

            file.Create(stream.Length);

            var response = file.UploadRange(new HttpRange(0, stream.Length),stream);
            
            return true;

        }



        public FileResultSegment GetAllFilesFromDirectory(string fileShareName, string directoryName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("StorageConnectionString"));
            CloudFileClient cloudFileClient = cloudStorageAccount.CreateCloudFileClient();
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory rootDirectory = cloudFileShare.GetRootDirectoryReference();
            CloudFileDirectory fileDirectory = rootDirectory.GetDirectoryReference(directoryName);

            List<IListFileItem> results = new List<IListFileItem>();
            FileContinuationToken token = null;
            FileResultSegment resultSegment = null;
            do
            {
                resultSegment = fileDirectory.ListFilesAndDirectoriesSegmentedAsync(token).GetAwaiter().GetResult();
                token = resultSegment.ContinuationToken;
            }
            while (token != null);
            return resultSegment;
        }

        public async Task DeleteAllAsync(string shareName, string directoryName)
        {
            ShareClient shareClient = new ShareClient(configuration.GetConnectionString("StorageConnectionString"), shareName);
            ShareDirectoryClient dirClient = shareClient.GetDirectoryClient(directoryName);
            Pageable<ShareFileItem> shareFileItems = dirClient.GetFilesAndDirectories();

            foreach (ShareFileItem item in shareFileItems)
            {
                if (item.IsDirectory)
                {
                    var subDir = dirClient.GetSubdirectoryClient(item.Name);
                    await subDir.DeleteAsync();
                }
                else
                {
                    await dirClient.DeleteFileAsync(item.Name);
                }
            }

            await dirClient.DeleteAsync();

        }
    }
}
