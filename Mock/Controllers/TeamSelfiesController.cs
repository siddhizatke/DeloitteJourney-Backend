using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock.Data;
using Mock.Model;
using Mock.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamSelfiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public TeamSelfiesController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamSelfiesModel>>> GetProjectTeamSelfies()
        {
            return await _context.TeamSelfies.ToListAsync();
        }

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

        [HttpPost]
        public async Task<ActionResult<TeamSelfiesModel>> PostProjectTeamSelfie([FromForm] TeamSelfieUploadDto selfieDto)
        {
            var selfie = new TeamSelfiesModel
            {
                TeamselfieDescription = selfieDto.TeamDescription,
                TeamImageUrl = string.Empty // Initialize required property
            };

            if (selfieDto.TeamImage != null)
            {
                // Upload the file and set the URL internally
                selfie.TeamImageUrl = await _fileService.UploadFileAsync(selfieDto.TeamImage, "Photos/TeamSelfie");
            }

            // Ensure the Id is not set to avoid explicit identity insert
            selfie.Id = 0;

            try
            {
                _context.TeamSelfies.Add(selfie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                throw;
            }

            return CreatedAtAction(nameof(GetProjectTeamSelfie), new { id = selfie.Id }, selfie);
        }

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

            try
            {
                if (selfieDto.TeamImage != null)
                {
                    selfie.TeamImageUrl = await _fileService.UploadFileAsync(selfieDto.TeamImage, "Photos/TeamSelfie");
                }

                _context.Entry(selfie).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error updating team selfie: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }


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

        private bool TeamSelfieExists(int id)
        {
            return _context.TeamSelfies.Any(e => e.Id == id);
        }
    }
}
