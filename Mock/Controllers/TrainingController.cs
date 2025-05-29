using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using System.Threading.Tasks;
using static Mock.Exception.ExceptionFilter;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Returns all training activities.
        [HttpGet]
        public async Task<IActionResult> GetTrainingActivities()
        {
            return Ok(await _context.TrainingActivities.ToListAsync());
        }

        // Returns a training activity by ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingActivity(int id)
        {
            var trainingActivity = await _context.TrainingActivities.FindAsync(id);

            if (trainingActivity == null)
            {
                throw new NotFoundException("Training activity data not found");
            }

            return Ok(trainingActivity);
        }

        // Creates a new training activity.
        [HttpPost]
        public async Task<IActionResult> PostTrainingActivity([FromBody] TrainingActivityModel trainingActivity)
        {
            if (trainingActivity == null)
            {
                throw new BadRequestException("Data not entered.");
            }
            trainingActivity.Id = 0;

            _context.TrainingActivities.Add(trainingActivity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainingActivity), new { id = trainingActivity.Id }, trainingActivity);
        }

        // Deletes a training activity by ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var activities = await _context.TrainingActivities.FindAsync(id);
            if (activities == null)
            {
                throw new NotFoundException("Training activity data not found");
            }

            _context.TrainingActivities.Remove(activities);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}