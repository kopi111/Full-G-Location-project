using GLocation.Domain.DTOs;
using GLocation.Domain.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GLocation.WebApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : Controller
    {
        private string errorMessage = string.Empty;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        [HttpPost("otp/generate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser(CreateUser request)
        {
            try
            {
                _logger.LogInformation("Creating user: {request}", JsonSerializer.Serialize(request));

                // Validate request data
                if (!IsValidCreateUserRequest(request))
                {
                    _logger.LogWarning("CreateUser request failed validation.");
                    return BadRequest("Invalid user data provided.");
                }

                // Check if email already exists
                var existingUser = await _userService.GetUserByEmail(request.email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Email already exists: {email}", request.email);
                    return Conflict($"User with email {request.email} already exists.");
                }

                // Create user
                var newUser = await _userService.CreateUser(request);

                _logger.LogInformation("User created successfully: {userId}", newUser.Id);
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating user: {message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }

        // Validation method
        private bool IsValidCreateUserRequest(CreateUser request)
        {
            if (string.IsNullOrWhiteSpace(request.email) ||
                string.IsNullOrWhiteSpace(request.password) ||
                string.IsNullOrWhiteSpace(request.firstName) ||
                string.IsNullOrWhiteSpace(request.lastName) ||
                string.IsNullOrWhiteSpace(request.phone))
                return false;

            // Validate email format
            if (!Regex.IsMatch(request.email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return false;

            // Validate password length (example: min 8 chars)
            if (request.password.Length < 8)
                return false;

            // Validate phone number (digits only, length between 7-15)
            if (!Regex.IsMatch(request.phone, @"^\d{7,15}$"))
                return false;

            // Validate address object
            if (request.address == null ||
                string.IsNullOrWhiteSpace(request.address.Street) ||
                string.IsNullOrWhiteSpace(request.address.City))
                return false;

            return true;
        }


    }
}
