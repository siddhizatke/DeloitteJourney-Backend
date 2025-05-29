using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using Mock.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Mock.Exception.ExceptionFilter;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public UserController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // Returns all users.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Returns a user by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new NotFoundException("User data not found");
            }

            return user;
        }

        // Creates a new user.
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUser([FromForm] UserUploadDto userDto)
        {
            if (userDto == null)
            {
                throw new BadRequestException("Data not entered.");
            }

            var user = new UserModel
            {
                Name = userDto.Name,
                AboutMe = userDto.AboutMe,
                AboutMeFormal = userDto.AboutMeFormal,
                ProfilePictureBase64 = null,
            };

            if (userDto.ProfilePicture != null)
            {
                using (var ms = new MemoryStream())
                {
                    await userDto.ProfilePicture.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    user.ProfilePictureBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                throw new BadRequestException("ProfilePicture is required.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Updates an existing user.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromForm] UserUploadDto userDto)
        {
            if (userDto == null)
            {
                throw new BadRequestException("Data not entered.");
            }

            if (id != userDto.Id)
            {
                throw new BadRequestException("User ID mismatch.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User data not found");
            }

            user.Name = userDto.Name;
            user.AboutMe = userDto.AboutMe;
            user.AboutMeFormal = userDto.AboutMeFormal;

            if (userDto.ProfilePicture != null)
            {
                using (var ms = new MemoryStream())
                {
                    await userDto.ProfilePicture.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    user.ProfilePictureBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                throw new BadRequestException("ProfilePicture is required.");
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Deletes a user by ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User data not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Checks if a user exists by ID.
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}