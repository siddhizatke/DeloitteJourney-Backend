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
    public class UserPhotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public UserPhotosController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        // GET: api/userphoto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPhoto>>> GetPhotos()
        {
            return await _context.UserPhotos.ToListAsync();
        }

        // POST: api/userphoto
        [HttpPost]
        public async Task<ActionResult<UserPhoto>> PostPhoto([FromForm] UserPhotoDto photo)
        {
            
            var userphoto = new UserPhoto
            {
                PhotoUrl = string.Empty, // Initialize required property
            };

            if (userphoto.PhotoUrl != null)
            {
                userphoto.PhotoUrl = await _fileService.UploadFileAsync(photo.PhotoUrl, "Photos/UserPhotos");
            }
            

            _context.UserPhotos.Add(userphoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPhotos), new { id = photo.PhotoId }, photo);
        }

        // DELETE: api/userphoto/{photoId}
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
