using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {

        private readonly IQueueStorageService _queueStorageService;
        public QueueController(IQueueStorageService queueStorageService)
        {
            _queueStorageService = queueStorageService ?? throw new ArgumentNullException(nameof(queueStorageService));
        }



        [HttpGet]
        [Route("CreateQueueClient")]
        public IActionResult CreateQueueClient([FromQuery] string queueName)
        {
            _queueStorageService.CreateQueueClient(queueName);
            return Ok("Success");
        }

        [HttpPut]
        [Route("CreateQueue")]
        public IActionResult CreateQueue(string queueName)
        {
            string result;
            if (_queueStorageService.CreateQueue(queueName))
                result = "Successefully created queue";
            else
                result = "There was an issue while creating queue!";

            return Ok(result);
        }

        [HttpPut]
        [Route("InsertMessage")]
        public IActionResult InsertMessage(string queueName, string message)
        {
            return Ok(_queueStorageService.InsertMessage(queueName, message));

        }

        [HttpPost]
        [Route("PeekMessage")]
        public async Task<IActionResult> PeekMessage(string queueName)
        {
            return Ok(await _queueStorageService.PeekMessage(queueName));

        }

        [HttpPost]
        [Route("UpdateMessage")]
        public async Task<IActionResult> UpdateMessage(string queueName)
        {
            return Ok(await _queueStorageService.UpdateMessage(queueName));
        }

        [HttpPost]
        [Route("DequeueMessage")]
        public IActionResult DequeueMessage(string queueName)
        {
            _queueStorageService.DequeueMessage(queueName);
            return Ok("Success");
        }

      
        [HttpPost]
        [Route("DeleteQueue")]
        public IActionResult DeleteQueue(string queueName)
        {
            _queueStorageService.DeleteQueue(queueName);
            return Ok("Success");
        }
    }
}