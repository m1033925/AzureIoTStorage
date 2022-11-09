using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares;
using AzureStorage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.File;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileShareController : Controller
    {
        private readonly IFileStorage _storageService;
        public FileShareController(IFileStorage storageService)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        [HttpPost]
        [Route("CreateShareAsync")]
        public async Task<IActionResult> CreateShareAsync([FromQuery] string shareName)
        {
            return Ok(await _storageService.CreateShareAsync(shareName));
        }

        [HttpPost]
        [Route("CreateFileinRootDirectoryAsync")]
        public IActionResult CreateFileinRootDirectoryAsync([FromQuery] string shareName)
        {
            _storageService.CreateFileinRootDirectoryAsync(shareName);
            return Ok("Success");
        }

        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile()
        {
            Task<bool> response = _storageService.UploadFile();
            return Ok(response);
        }

        [HttpGet]
        [Route("GetAllFilesFromDirectory")]
        public IActionResult GetAllFilesFromDirectory([FromQuery] string shareName, string directoryName)
        {

            return Ok(_storageService.GetAllFilesFromDirectory(shareName, directoryName));

        }

        [HttpGet]
        [Route("DeleteAllFilesAsync")]
        public IActionResult DeleteAllFilesAsync([FromQuery] string shareName, string directoryName)
        {

            _storageService.DeleteAllAsync(shareName, directoryName);
            return Ok("Success");
        }
    }
}
