using Mock.Repository;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileService> _logger;

    // Constructor to initialize environment and logger dependencies
    public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    // Uploads a file to the specified folder and returns its relative path
    public async Task<string> UploadFileAsync(IFormFile file, string folderName)
    {
        if (file == null || string.IsNullOrEmpty(folderName))
        {
            _logger.LogError("File or folder cannot be null");
            throw new ArgumentNullException(nameof(file), "File or folder cannot be null");
        }

        try
        {
            var uploadsFolderPath = Path.Combine(_environment.ContentRootPath, folderName);
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation($"File uploaded successfully: {filePath}");

            return $"/{folderName}/{uniqueFileName}".Replace("\\", "/");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            throw;
        }
    }
}
