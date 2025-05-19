using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using Mock.Repository;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        // Constructor to initialize the UserController with database context and file service
        public UserController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: api/User
        // Retrieves a list of all users from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/{id}
        // Retrieves a specific user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        // Adds a new user to the database
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUser([FromForm] UserUploadDto userDto)
        {
            // Fix for CS9035: Ensure 'ProfilePictureUrl' is set in the object initializer
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

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/User/{id}
        // Updates an existing user in the database
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromForm] UserUploadDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
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

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/User/{id}
        // Deletes a user from the database by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Checks if a user exists in the database by ID
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
