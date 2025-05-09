using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using Mock.Repository;
using System.Threading.Tasks;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSelfiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public TrainingSelfiesController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingselfieModel>>> GetTrainingFriendsSelfies()
        {
            return await _context.TrainingSelfies.ToListAsync();
        }

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

        [HttpPost]
        public async Task<ActionResult<TrainingselfieModel>> PostTrainingFriendsSelfie([FromForm] TrainingSelfieUploadDto selfieDto)
        {
            var selfie = new TrainingselfieModel
            {
                TrainingDescription = selfieDto.TrainingDescription,
                TrainingImageUrl = string.Empty // Initialize required property
            };

            if (selfieDto.TrainingImage != null)
            {
                // Upload the file and set the URL internally
                selfie.TrainingImageUrl = await _fileService.UploadFileAsync(selfieDto.TrainingImage, "Photos/TrainingSelfie");
            }

            // Ensure the Id is not set to avoid explicit identity insert
            selfie.Id = 0;

            _context.TrainingSelfies.Add(selfie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainingFriendsSelfie), new { id = selfie.Id }, selfie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingFriendsSelfie(int id, [FromForm] TrainingSelfieUploadDto selfieDto)
        {
            if (id != selfieDto.Id)
            {
                return BadRequest();
            }

            var selfie = await _context.TrainingSelfies.FindAsync(id);
            if (selfie == null)
            {
                return NotFound();
            }

            selfie.TrainingDescription = selfieDto.TrainingDescription;

            if (selfieDto.TrainingImage != null)
            {
                // Upload the file and set the URL internally
                selfie.TrainingImageUrl = await _fileService.UploadFileAsync(selfieDto.TrainingImage, "Photos/TrainingSelfie");
            }

            _context.Entry(selfie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingSelfieExists(id))
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
