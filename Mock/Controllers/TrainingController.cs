using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor 
        public TrainingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Training
        [HttpGet]
        public async Task<IActionResult> GetTrainingActivities()
        {
            var activities = await _context.TrainingActivities.ToListAsync();
            return Ok(activities);
        }

        // GET: api/Training/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingActivity(int id)
        {
            var trainingActivity = await _context.TrainingActivities.FindAsync(id);

            if (trainingActivity == null)
            {
                return NotFound();
            }

            return Ok(trainingActivity);
        }

        // POST: api/Training
        [HttpPost]
        public async Task<IActionResult> PostTrainingActivity([FromBody] TrainingActivityModel trainingActivity)
        {
            if (trainingActivity == null)
            {
                return BadRequest();
            }
            trainingActivity.Id = 0;

            _context.TrainingActivities.Add(trainingActivity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainingActivity), new { id = trainingActivity.Id }, trainingActivity);
        }

        // DELETE: api/Training/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var activities = await _context.TrainingActivities.FindAsync(id);
            if (activities == null)
            {
                return NotFound();
            }

            _context.TrainingActivities.Remove(activities);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
