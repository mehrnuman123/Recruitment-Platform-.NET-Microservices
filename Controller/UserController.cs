using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserDbContext _context;
    private readonly ILogger<UserController> _logger;

    public UserController(UserDbContext context, ILogger<UserController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (user == null)
        {
            _logger.LogError("CreateUser: User Object is Null");
            return BadRequest(new { message = "User object is null" });
        }

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {user.Id} created successfully");
            return CreatedAtAction(nameof(CreateUser), new { data = user });
        }
        catch (Exception exp)
        {
            _logger.LogError($"{exp}");
            return StatusCode(500, new { message = "Internal Server Error " });
        }
    }


}