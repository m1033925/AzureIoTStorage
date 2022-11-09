using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ITableStorageService _storageService;
        public ItemsController(ITableStorageService storageService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        [HttpGet]
        [Route("GetEntityAsync")]
        public async Task<IActionResult> GetEntityAsync([FromQuery] string category, string id)
        {
            return Ok(await _storageService.GetEntityAsync(category, id));
        }

        [HttpPost]
        [Route("PostAsync")]
        public async Task<IActionResult> PostAsync(GroceryItemEntity entity)
        {
            entity.PartitionKey = entity.Category;

            string Id = Guid.NewGuid().ToString();
            entity.Id = Id;
            entity.RowKey = Id;

            var createdEntity = await _storageService.UpsertEntityAsync(entity);
            return Ok(createdEntity); ;
        }

        [HttpPut]
        [Route("PutAsync")]
        public async Task<IActionResult> PutAsync(GroceryItemEntity entity)
        {
            entity.PartitionKey = entity.Category;
            entity.RowKey = entity.Id;

            await _storageService.UpsertEntityAsync(entity);
            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string category, string id)
        {
            await _storageService.DeleteEntityAsync(category, id);
            return NoContent();
        }

        [HttpDelete]
        [Route("dropTable")]
        public async Task<IActionResult> DropAzureStorageTable(string TableName)
        {
            await _storageService.DropAzureStorageTable(TableName);
            return NoContent();
        }
    }
}



