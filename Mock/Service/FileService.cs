using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mock.Repository
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || string.IsNullOrEmpty(folderName))
            {
                _logger.LogError("File or folder cannot be null");
                throw new ArgumentNullException(nameof(file), "File or folder cannot be null");
            }

            try
            {
                // Adjust the path to reference the 'photos/teamselfie' directory
                var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, "Photos");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // Use original file name without GUID
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation($"File uploaded successfully: {filePath}");
                return $"{fileName}"; // Return relative path
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                throw; // Re-throw the exception after logging it
            }
        }
    }
}
