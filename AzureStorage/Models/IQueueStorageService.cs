using System; 
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues.Models;

namespace AzureStorage.Models
{
    public interface IQueueStorageService
    {
        void CreateQueueClient(string queueName);
        bool CreateQueue(string queueName);
        Response<SendReceipt> InsertMessage(string queueName, string message);
        Task<Response<PeekedMessage>> PeekMessage(string queueName);
        Task<UpdateReceipt> UpdateMessage(string queueName);
        void DequeueMessage(string queueName);      
        void DeleteQueue(string queueName);

    }
}
