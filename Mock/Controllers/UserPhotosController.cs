using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using Mock.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPhotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        // Constructor to initialize the UserPhotosController with database context and file service
        public UserPhotosController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: api/userphoto
        // Retrieves a list of all user photos from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPhoto>>> GetPhotos()
        {
            return await _context.UserPhotos.ToListAsync();
        }

        // POST: api/userphoto
        // Adds a new user photo to the database
        [HttpPost]
        public async Task<ActionResult<UserPhoto>> PostPhoto([FromForm] UserPhotoDto photo)
        {
            var userphoto = new UserPhoto
            {
                PhotoUrl = string.Empty, // Initialize required property
            };

            if (photo.PhotoUrl != null)
            {
                userphoto.PhotoUrl = await _fileService.UploadFileAsync(photo.PhotoUrl, "Photos/UserPhotos");
            }

            _context.UserPhotos.Add(userphoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPhotos), new { id = photo.PhotoId }, photo);
        }

        // DELETE: api/userphoto/{photoId}
        // Deletes a user photo from the database by ID
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var userPhoto = await _context.UserPhotos.FindAsync(photoId);
            if (userPhoto == null)
            {
                return NotFound();
            }

            _context.UserPhotos.Remove(userPhoto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
