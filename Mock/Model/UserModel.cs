using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Mock.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AboutMe { get; set; }
        public required string AboutMeFormal { get; set; }
        public string? ProfilePictureBase64 { get; set; }


    }
    public class UserUploadDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AboutMe { get; set; }
        public required string AboutMeFormal { get; set; }
        public IFormFile? ProfilePicture { get; set; }

    }

    public class UserPhoto
    {
        [Key]
        public int PhotoId { get; set; }
        public  string? PhotoBase64 { get; set; }
    }
    public class UserPhotoDto
    {
        [Key]
        public int PhotoId { get; set; }
        public required IFormFile PhotoFile { get; set; }
    }
}
