using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using AzureStorage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : Controller
    {
        private readonly IBlobStorage _blobstorage;

        public BlobController(IBlobStorage blobstorage)
        {
            _blobstorage = blobstorage;
        }

        [HttpGet]
        [Route("GetFile")]
        public async Task<IActionResult> GetFile()
        {
            // Get all files at the Azure Storage Location and return them
            List<BlobDto>? files = await _blobstorage.ListAsync();

            // Returns an empty array if no files are present at the storage container
            return StatusCode(StatusCodes.Status200OK, files);
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            BlobProperties? response = await _blobstorage.UploadAsync(file);

            // Check if we got an error
            //if (response. == true)
            //{
            //    // We got an error during upload, return an error with details to the client
            //    return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            //}
            //else
            //{
            // Return a success message to the client about successfull upload
            return StatusCode(StatusCodes.Status200OK, response);
            //}
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> Download(string filename)
        {
            BlobDto? file = await _blobstorage.DownloadAsync(filename);

            // Check if file was found
            if (file == null)
            {
                // Was not, return error message to client
                return StatusCode(StatusCodes.Status500InternalServerError, $"File {filename} could not be downloaded.");
            }
            else
            {
                // File was found, return it to client
                return File(file.Content, file.ContentType, file.Name);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string filename)
        {
            BlobResponseDto response = await _blobstorage.DeleteAsync(filename);

            // Check if we got an error
            if (response.Error == true)
            {
                // Return an error message to the client
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            }
            else
            {
                // File has been successfully deleted
                return StatusCode(StatusCodes.Status200OK, response.Status);
            }
        }
    }
}
