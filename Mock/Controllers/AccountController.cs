using Microsoft.AspNetCore.Mvc;
using Mock.Data;
using Mock.Model;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Registers a new user with a role based on registration source.
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegisterDto dto,
        [FromHeader(Name = "X-Registration-Source")] string registrationSource = null)
    {
        if (_context.Login.Any(u => u.UserName.ToLower() == dto.Username.ToLower()))
            return BadRequest(new { message = "Username already exists" });

        string role = registrationSource == "Admin" ? "Owner" : "Viewer";

        var user = new LoginModel
        {
            Id = 0,
            UserName = dto.Username,
            Password = dto.Password, // store plain password (NOT RECOMMENDED)
            Roles = role
        };

        _context.Login.Add(user);
        _context.SaveChanges();
        return Ok(new { message = "Registration successful", role = user.Roles });
    }

    // Authenticates a user and returns login result.
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string username, [FromQuery] string password)
    {
        var user = _context.Login
            .SingleOrDefault(u => u.UserName.ToLower() == username.ToLower());

        if (user == null)
        {
            Console.WriteLine($"User not found: {username}");
            return Unauthorized(new { message = "Invalid username or password" });
        }

        if (user.Password != password)
        {
            Console.WriteLine($"Password mismatch for user: {username}");
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(new { message = "Login successful", username = user.UserName, role = user.Roles });
    }
}