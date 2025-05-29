using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;

using static Mock.Exception.ExceptionFilter;

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

        // Returns all training selfies.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingselfieModel>>> GetTrainingFriendsSelfies()
        {
            return await _context.TrainingSelfies.ToListAsync();
        }

        // Returns a training selfie by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingselfieModel>> GetTrainingFriendsSelfie(int id)
        {
            var selfie = await _context.TrainingSelfies.FindAsync(id);
            if (selfie == null)
            {
                throw new NotFoundException("Training selfie Data not found");
            }
            return selfie;
        }

        // Creates a new training selfie.
        [HttpPost]
        public async Task<ActionResult<TrainingselfieModel>> PostTrainingFriendsSelfie([FromForm] TrainingSelfieUploadDto selfieDto)
        {
            if (selfieDto == null)
            {
                throw new BadRequestException("Data not entered.");
            }

            var selfie = new TrainingselfieModel
            {
                TrainingDescription = selfieDto.TrainingDescription,
                TrainingImageBase64 = string.Empty
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
            else
            {
                throw new BadRequestException("TrainingImage is required.");
            }

            selfie.Id = 0;

            _context.TrainingSelfies.Add(selfie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainingFriendsSelfie), new { id = selfie.Id }, selfie);
        }

        // Updates an existing training selfie.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingFriendsSelfie(int id, [FromForm] TrainingSelfieUploadDto selfieDto)
        {
            if (selfieDto == null)
            {
                throw new BadRequestException("Data not entered.");
            }

            if (id != selfieDto.Id)
            {
                throw new BadRequestException("ID mismatch");
            }

            var selfie = await _context.TrainingSelfies.FindAsync(id);
            if (selfie == null)
            {
                throw new NotFoundException("Training selfie Data not found");
            }

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
            else
            {
                throw new BadRequestException("TrainingImage is required.");
            }

            _context.Entry(selfie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Deletes a training selfie by ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingFriendsSelfie(int id)
        {
            var selfie = await _context.TrainingSelfies.FindAsync(id);
            if (selfie == null)
            {
                throw new NotFoundException("Training selfie Data not found");
            }

            _context.TrainingSelfies.Remove(selfie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Checks if a training selfie exists by ID.
        private bool TrainingSelfieExists(int id)
        {
            return _context.TrainingSelfies.Any(e => e.Id == id);
        }
    }
}