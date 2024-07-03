using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using HomeBudget.Components.Users.Services.Interfaces;
using HomeBudget.Identity.Domain.Interfaces;
using HomeBudget.Identity.Domain.Models;

namespace HomeBudget.Identity.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class IdentityController(
        IUsersService usersService,
        IJwtBuilder jwtBuilder,
        IEncryptor encryptor)
        : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] User user,
            [FromQuery(Name = "d")] string destination = "frontend")
        {
            var userIdentity = await usersService.GetUserByEmailAsync(user.Email);

            if (userIdentity == null)
            {
                return NotFound($"User with email: ${user.Email} hasn't been found.");
            }

            if (destination == "backend" && !userIdentity.IsAdmin)
            {
                return BadRequest("Could not authenticate user.");
            }

            var isValid = userIdentity.ValidatePassword(user.Password, encryptor);

            if (!isValid)
            {
                return BadRequest("Could not authenticate user.");
            }

            var token = jwtBuilder.GetToken(userIdentity.Id);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var userIdentity = await usersService.GetUserByEmailAsync(user.Email);

            if (userIdentity != null)
            {
                return BadRequest("User already exists.");
            }

            user.SetPassword(user.Password, encryptor);
            await usersService.RegisterUserAsync(user);

            return Ok();
        }

        [HttpGet("validate")]
        public async Task<IActionResult> Validate(
            [FromQuery(Name = "email")] string email,
            [FromQuery(Name = "token")] string token)
        {
            var userIdentity = await usersService.GetUserByEmailAsync(email);

            if (userIdentity == null)
            {
                return NotFound("User not found.");
            }

            var userId = jwtBuilder.ValidateToken(token);

            if (userId != userIdentity.Id)
            {
                return BadRequest("Invalid token.");
            }

            return Ok(userId);
        }
    }
}
