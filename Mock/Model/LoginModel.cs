using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mock.Model
{
    public class LoginModel
    {
        [Key]
        public required int Id { get; set; }
        public required string UserName { get; set; }
        [PasswordPropertyText]
        public required string Password { get; set; }
        public required string Roles { get; set; }

    }


    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
