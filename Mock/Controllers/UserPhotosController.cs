using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using static Mock.Exception.ExceptionFilter;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPhotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserPhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Returns a list of user photos (up to 10).
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPhoto>>> GetPhotos()
        {
            return Ok(await _context.UserPhotos
                .Select(p => new { p.PhotoId, p.PhotoBase64 })
                .Take(10)
                .ToListAsync());
        }

        // Uploads a new user photo.
        [HttpPost]
        public async Task<ActionResult<UserPhoto>> PostPhoto([FromForm] UserPhotoDto photoDto)
        {
            if (photoDto == null)
            {
                throw new BadRequestException("Data not entered.");
            }

            if (photoDto.PhotoFile == null)
            {
                throw new BadRequestException("PhotoFile is required.");
            }

            var userPhoto = new UserPhoto();

            using (var ms = new MemoryStream())
            {
                await photoDto.PhotoFile.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                userPhoto.PhotoBase64 = Convert.ToBase64String(fileBytes);
            }

            _context.UserPhotos.Add(userPhoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPhotos), new { id = userPhoto.PhotoId }, userPhoto);
        }

        // Deletes a user photo by ID.
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var userPhoto = await _context.UserPhotos.FindAsync(photoId);
            if (userPhoto == null)
            {
                throw new NotFoundException("User photo not found");
            }

            _context.UserPhotos.Remove(userPhoto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}