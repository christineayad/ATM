using ATMAPI.Data;
using ATMAPI.DTO;
using ATMAPI.Model;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
      
        public UserController(ApplicationDBContext context)
        {
            _context=context;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromQuery] Registerdto registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = await _context.Users.AnyAsync(u => u.Email == registerRequest.Email);
            if (existingUser)
                return BadRequest("Email is already in use.");
            var user = new User
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Birthdate=registerRequest.Birthdate,
                //_PasswordHasher.HashPassword(null, registerRequest.Password),
                UserCategoryId =registerRequest.UserCategoryId
                
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var operationHestory=new UserOperationHestory
         {
             UserId = user.UserId,
             OperationId=1,
             
             OperationDateTime = DateTime.UtcNow
         };
            _context.UserOperationHestories.Add(operationHestory);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var user =  _context.Users.ToList();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Logindto loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginRequest.UserName && u.Password == loginRequest.Password);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            var operationHestory = new UserOperationHestory
            {
                UserId = user.UserId,
                OperationId = 2,

                OperationDateTime = DateTime.UtcNow
            };
            _context.UserOperationHestories.Add(operationHestory);
            await _context.SaveChangesAsync();
            if (!StoreLogin.loggedInUsers.Contains(user.UserId))
                StoreLogin.loggedInUsers.Add(user.UserId);
            return Ok(new { message = "Login successful", userId = user.UserId });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromQuery] int id)
        {
            if (StoreLogin.loggedInUsers.Contains(id))
            {
                StoreLogin.loggedInUsers.Remove(id);
                var operationHestory = new UserOperationHestory
                {
                    UserId = id,
                    OperationId = 9,

                    OperationDateTime = DateTime.UtcNow
                };
                _context.UserOperationHestories.Add(operationHestory);
                _context.SaveChangesAsync();
                return Ok("User logged out.");
            }


            return BadRequest("User not logged in.");
        }

    }
}
