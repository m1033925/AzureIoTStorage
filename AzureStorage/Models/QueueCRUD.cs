using System;
using System.Configuration;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureStorage.Models;
using Microsoft.Extensions.Configuration;

namespace AzureStorage.Models
{
    public class QueueCRUD : IQueueStorageService
    {

        public IConfiguration configuration;
        public QueueCRUD(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        //-------------------------------------------------
        // Create the queue service client
        //-------------------------------------------------
        public void CreateQueueClient(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = configuration.GetConnectionString("StorageConnectionString");

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);
        }


        //-------------------------------------------------
        // Create a message queue
        //-------------------------------------------------
        public bool CreateQueue(string queueName)
        {
            try
            {
                // Get the connection string from app settings
                string connectionString = configuration.GetConnectionString("StorageConnectionString");

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                // Create the queue
                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    //Console.WriteLine($"Queue created: '{queueClient.Name}'");
                    return true;
                }
                else
                {
                    //Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\n\n");
                Console.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                return false;
            }
        }

        //-------------------------------------------------
        // Insert a message into a queue
        //-------------------------------------------------
        public Response<SendReceipt> InsertMessage(string queueName, string message)
        {
            Response<SendReceipt> result = null;
            // Get the connection string from app settings
            string connectionString = configuration.GetConnectionString("StorageConnectionString");

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                result = queueClient.SendMessage(message);
            }

            return result;
        }

        //-------------------------------------------------
        // Peek at a message in the queue
        //-------------------------------------------------
        public async Task<Response<PeekedMessage>> PeekMessage(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = configuration.GetConnectionString("StorageConnectionString");

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            Response<PeekedMessage> peekedMessage = null;
            if (queueClient.Exists())
            {
                // Peek at the next message
                peekedMessage = await queueClient.PeekMessageAsync();

                // Display the message
                //Console.WriteLine($"Peeked message: '{peekedMessage[0].Body}'");
            }
            return peekedMessage;
        }

        //-------------------------------------------------
        // Update an existing message in the queue
        //-------------------------------------------------
        public async Task<UpdateReceipt> UpdateMessage(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = configuration.GetConnectionString("StorageConnectionString");

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            UpdateReceipt updateReceipt = null;
            if (queueClient.Exists())
            {
                // Get the message from the queue
                QueueMessage[] message = queueClient.ReceiveMessages();

                // Update the message contents
                updateReceipt = await queueClient.UpdateMessageAsync(message[0].MessageId,
                        message[0].PopReceipt,
                        "Updated Welcome to queue",
                        TimeSpan.FromSeconds(1)  // Make it invisible for another 60 seconds
                    );
            }

            return updateReceipt;
        }

        //-------------------------------------------------
        // Process and remove a message from the queue
        //-------------------------------------------------
        public void DequeueMessage(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = configuration.GetConnectionString("StorageConnectionString");

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Get the next message
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                // Process (i.e. print) the message in less than 30 seconds
                Console.WriteLine($"Dequeued message: '{retrievedMessage[0].Body}'");

                // Delete the message
                queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
            }
        }

        //-------------------------------------------------
        // Delete the queue
        //-------------------------------------------------
        public void DeleteQueue(string queueName)
        {
            // Get the connection string from app settings
            string connectionString = configuration.GetConnectionString("StorageConnectionString");

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                // Delete the queue
                queueClient.Delete();
            }

            Console.WriteLine($"Queue deleted: '{queueClient.Name}'");
        }
    }
}
