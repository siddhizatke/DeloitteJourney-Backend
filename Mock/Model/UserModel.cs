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
        [BindNever]
        public required string ProfilePictureUrl { get; set; }
       // public  List<string> PhotosUrl { get; set; }

    }
    public class UserUploadDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AboutMe { get; set; }
        public required string AboutMeFormal { get; set; }
        public required IFormFile ProfilePicture { get; set; }
        //public  List<IFormFile> Photos { get; set; }
    }

    public class UserPhoto
    {
        [Key]
        public int PhotoId { get; set; }
        public required string PhotoUrl { get; set; }
    }
    public class UserPhotoDto
    {
        [Key]
        public int PhotoId { get; set; }
        public required IFormFile PhotoUrl { get; set; }
    }
}
