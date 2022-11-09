namespace AzureStorage.Models
{
    public interface ITableStorageService
    {
        Task<GroceryItemEntity> GetEntityAsync(string category, string id);
        Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity entity);
        Task DeleteEntityAsync(string category, string id);
        Task<bool> DropAzureStorageTable(string TableName);
    }
}
