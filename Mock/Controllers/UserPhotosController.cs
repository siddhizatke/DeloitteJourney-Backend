using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        // GET: api/userphotos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPhoto>>> GetPhotos()
        {

            var photos = await _context.UserPhotos
        .Select(p => new { p.PhotoId, p.PhotoBase64 })
        .Take(10)
        .ToListAsync();
            return Ok(photos);
        }

        // POST: api/userphotos
        [HttpPost]
        public async Task<ActionResult<UserPhoto>> PostPhoto([FromForm] UserPhotoDto photoDto)
        {
            var userPhoto = new UserPhoto();

            if (photoDto.PhotoFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    await photoDto.PhotoFile.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    userPhoto.PhotoBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                userPhoto.PhotoBase64 = string.Empty;
            }

            _context.UserPhotos.Add(userPhoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPhotos), new { id = userPhoto.PhotoId }, userPhoto);
        }

        // DELETE: api/userphotos/{photoId}
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
