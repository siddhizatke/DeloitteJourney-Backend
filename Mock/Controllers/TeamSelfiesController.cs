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
using static Mock.Exception.ExceptionFilter;

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

        // Returns all team selfies.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamSelfiesModel>>> GetProjectTeamSelfies()
        {
            return await _context.TeamSelfies.ToListAsync();
        }

        // Returns a team selfie by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamSelfiesModel>> GetProjectTeamSelfie(int id)
        {
            var selfie = await _context.TeamSelfies.FindAsync(id);
            if (selfie == null)
            {
                throw new NotFoundException("Team selfie Data not found");
            }
            return selfie;
        }

        // Creates a new team selfie.
        [HttpPost]
        public async Task<ActionResult<TeamSelfiesModel>> PostProjectTeamSelfie([FromForm] TeamSelfieUploadDto selfieDto)
        {
            var selfie = new TeamSelfiesModel
            {
                TeamselfieDescription = selfieDto.TeamDescription,
                TeamImageBase64 = string.Empty
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

            _context.TeamSelfies.Add(selfie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjectTeamSelfie), new { id = selfie.Id }, selfie);
        }

        // Updates an existing team selfie.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectTeamSelfie(int id, [FromForm] TeamSelfieUploadDto selfieDto)
        {
            if (selfieDto == null)
            {
                throw new BadRequestException("Data not entered.");
            }

            if (id != selfieDto.Id)
            {
                throw new BadRequestException("ID mismatch");
            }

            var selfie = await _context.TeamSelfies.FindAsync(id);
            if (selfie == null)
            {
                throw new NotFoundException("Team selfie Data not found");
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
            else
            {
                throw new BadRequestException("TeamImage is required.");
            }

            _context.Entry(selfie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Deletes a team selfie by ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectTeamSelfie(int id)
        {
            var selfie = await _context.TeamSelfies.FindAsync(id);
            if (selfie == null)
            {
                throw new NotFoundException("Team selfie Data not found");
            }

            _context.TeamSelfies.Remove(selfie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}