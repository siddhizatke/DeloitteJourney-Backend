namespace Mock.Repository
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string Photos);
    }
}
