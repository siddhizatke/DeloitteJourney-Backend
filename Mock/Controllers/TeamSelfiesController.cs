using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamSelfiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeamSelfiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TeamSelfies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamSelfiesModel>>> GetProjectTeamSelfies()
        {
            return await _context.TeamSelfies.ToListAsync();
        }

        // GET: api/TeamSelfies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamSelfiesModel>> GetProjectTeamSelfie(int id)
        {
            var selfie = await _context.TeamSelfies.FindAsync(id);
            if (selfie == null)
            {
                return NotFound();
            }
            return selfie;
        }

        // POST: api/TeamSelfies
        [HttpPost]
        public async Task<ActionResult<TeamSelfiesModel>> PostProjectTeamSelfie([FromForm] TeamSelfieUploadDto selfieDto)
        {
            var selfie = new TeamSelfiesModel
            {
                TeamselfieDescription = selfieDto.TeamDescription,
                TeamImageBase64 = string.Empty // Initialize required property
            };

            if (selfieDto.TeamImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    await selfieDto.TeamImage.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    selfie.TeamImageBase64 = Convert.ToBase64String(fileBytes);
                }
            }

            selfie.Id = 0;

            try
            {
                _context.TeamSelfies.Add(selfie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return CreatedAtAction(nameof(GetProjectTeamSelfie), new { id = selfie.Id }, selfie);
        }

        // PUT: api/TeamSelfies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectTeamSelfie(int id, [FromForm] TeamSelfieUploadDto selfieDto)
        {
            if (id != selfieDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var selfie = await _context.TeamSelfies.FindAsync(id);
            if (selfie == null)
            {
                return NotFound("Team selfie not found");
            }

            selfie.TeamselfieDescription = selfieDto.TeamDescription;

            if (selfieDto.TeamImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    await selfieDto.TeamImage.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    selfie.TeamImageBase64 = "data:image/jpeg;base64," + Convert.ToBase64String(fileBytes);
                }
            }

            _context.Entry(selfie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/TeamSelfies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectTeamSelfie(int id)
        {
            var selfie = await _context.TeamSelfies.FindAsync(id);
            if (selfie == null)
            {
                return NotFound();
            }

            _context.TeamSelfies.Remove(selfie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Checks if a team selfie exists in the database by ID
        private bool TeamSelfieExists(int id)
        {
            return _context.TeamSelfies.Any(e => e.Id == id);
        }
    }
}
