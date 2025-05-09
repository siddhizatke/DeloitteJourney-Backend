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

        public UserController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

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

        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUser([FromForm] UserUploadDto userDto)
        {
            var user = new UserModel
            {
                Name = userDto.Name,
                AboutMe = userDto.AboutMe,
                AboutMeFormal= userDto.AboutMeFormal,
                ProfilePictureUrl = string.Empty, // Initialize required property
                //PhotosUrl = new List<string>()    // Initialize required property
            };

            if (userDto.ProfilePicture != null)
            {
                user.ProfilePictureUrl = await _fileService.UploadFileAsync(userDto.ProfilePicture, "Photos");
            }

            //if (userDto.Photos != null && userDto.Photos.Count > 0)
            //{
            //    foreach (var photo in userDto.Photos)
            //    {
            //        if (photo != null) // Ensure photo is not null
            //        {
            //            var photoUrl = await _fileService.UploadFileAsync(photo, "Photos");
            //            user.PhotosUrl.Add(photoUrl);
            //        }
            //    }
            //}

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromForm] UserUploadDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            if (userDto == null)
            {
                return BadRequest("User data cannot be null.");
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
                user.ProfilePictureUrl = await _fileService.UploadFileAsync(userDto.ProfilePicture, "Photos");
            }

            //if (userDto.Photos != null && userDto.Photos.Count > 0)
            //{
            //    user.PhotosUrl = new List<string>();
            //    foreach (var photo in userDto.Photos)
            //    {
            //        var photoUrl = await _fileService.UploadFileAsync(photo, "Photos");
            //        user.PhotosUrl.Add(photoUrl);
            //    }
            //}

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}