using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using HomeBudget.Components.Users.Services.Interfaces;
using HomeBudget.Core.Models;
using HomeBudget.Identity.Api.Models;
using HomeBudget.Identity.Domain.Interfaces;
using HomeBudget.Identity.Domain.Models;

namespace HomeBudget.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(
        IUsersService usersService,
        IJwtBuilder jwtBuilder,
        IEncryptor encryptor)
        : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            [FromQuery(Name = "d")] string destination = "frontend")
        {
            var userIdentity = await usersService.GetUserByEmailAsync(request.Email);

            if (userIdentity == null)
            {
                return NotFound($"User with email: {request.Email} hasn't been found.");
            }

            if (destination == "backend" && !userIdentity.IsAdmin)
            {
                return BadRequest($"Could not authenticate user '{request.Email}'.");
            }

            var isValid = userIdentity.ValidatePassword(request.Password, encryptor);

            if (!isValid)
            {
                return BadRequest($"Could not authenticate user {request.Email}.");
            }

            var token = jwtBuilder.GetToken(userIdentity.Key.ToString());

            return Ok(Result<string>.Succeeded(token));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userForRegistration = new User
            {
                Email = request.Email,
                Password = request.Password,
            };

            var userIdentity = await usersService.GetUserByEmailAsync(userForRegistration.Email);

            if (userIdentity != null)
            {
                return BadRequest($"The user '{userForRegistration.Email}' already exists.");
            }

            userForRegistration.SetPassword(userForRegistration.Password, encryptor);
            await usersService.RegisterUserAsync(userForRegistration);

            var response = new Result<Guid>(
                userForRegistration.Key,
                $"The user: '{userForRegistration.Email}' has been registered",
                true);

            return Ok(response);
        }

        [HttpGet("validate")]
        public async Task<IActionResult> Validate(
            [FromQuery(Name = "email")] string email,
            [FromQuery(Name = "token")] string token)
        {
            var userIdentity = await usersService.GetUserByEmailAsync(email);

            if (userIdentity == null)
            {
                return NotFound($"User: '{email}' not found.");
            }

            var userId = jwtBuilder.ValidateToken(token);

            if (!string.Equals(userId, userIdentity.Key.ToString()))
            {
                return BadRequest($"Invalid token. '{email}'");
            }

            return Ok(Result<string>.Succeeded(userId));
        }
    }
}
