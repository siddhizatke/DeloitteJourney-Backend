using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSelfiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainingSelfiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TrainingSelfies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingselfieModel>>> GetTrainingFriendsSelfies()
        {
            return await _context.TrainingSelfies.ToListAsync();
        }

        // GET: api/TrainingSelfies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingselfieModel>> GetTrainingFriendsSelfie(int id)
        {
            var selfie = await _context.TrainingSelfies.FindAsync(id);
            if (selfie == null)
            {
                return NotFound();
            }
            return selfie;
        }

        // POST: api/TrainingSelfies
        [HttpPost]
        public async Task<ActionResult<TrainingselfieModel>> PostTrainingFriendsSelfie([FromForm] TrainingSelfieUploadDto selfieDto)
        {
            var selfie = new TrainingselfieModel
            {
                TrainingDescription = selfieDto.TrainingDescription,
                TrainingImageBase64 = string.Empty // Initialize required property
            };

            if (selfieDto.TrainingImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    await selfieDto.TrainingImage.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    selfie.TrainingImageBase64 = Convert.ToBase64String(fileBytes);
                }
            }

            selfie.Id = 0;

            _context.TrainingSelfies.Add(selfie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainingFriendsSelfie), new { id = selfie.Id }, selfie);
        }

        // PUT: api/TrainingSelfies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingFriendsSelfie(int id, [FromForm] TrainingSelfieUploadDto selfieDto)
        {
            if (id != selfieDto.Id)
                return BadRequest();

            var selfie = await _context.TrainingSelfies.FindAsync(id);
            if (selfie == null)
                return NotFound();

            selfie.TrainingDescription = selfieDto.TrainingDescription;

            if (selfieDto.TrainingImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    await selfieDto.TrainingImage.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    selfie.TrainingImageBase64 = Convert.ToBase64String(fileBytes);
                }
            }

            _context.Entry(selfie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/TrainingSelfies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingFriendsSelfie(int id)
        {
            var selfie = await _context.TrainingSelfies.FindAsync(id);
            if (selfie == null)
            {
                return NotFound();
            }

            _context.TrainingSelfies.Remove(selfie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingSelfieExists(int id)
        {
            return _context.TrainingSelfies.Any(e => e.Id == id);
        }
    }
}
