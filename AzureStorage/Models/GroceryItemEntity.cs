using System;
using Azure;
using Azure.Data.Tables;

namespace AzureStorage.Models
{
    public class GroceryItemEntity : ITableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        ETag ITableEntity.ETag { get; set; }
    }
}