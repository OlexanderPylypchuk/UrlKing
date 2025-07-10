using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlKing.Server.Models.Dtos;
using UrlKing.Server.Services.IServices;
using UrlKing.Server.Utility;

namespace UrlKing.Server.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;

		public AuthController(IUserService userRepository)
		{
			_userService = userRepository;
		}

		[Authorize(Roles = SD.RoleAdmin)]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var users = await _userService.GetUsersAsync();
				if (users == null)
				{
					throw new Exception("Unexpected result - no users found");
				}
				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{username}")]
		public async Task<IActionResult> Get(string username)
		{
			try
			{
				var user = await _userService.GetUserAsync(username);
				if (user == null)
				{
					throw new Exception("No user with this username");
				}
				return Ok(user);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
		{
			var tokendto = await _userService.Login(model);
			if (tokendto == null || string.IsNullOrEmpty(tokendto.AccessToken))
			{
				return BadRequest("Username or password is incorrect");
			}
			return Ok(tokendto);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
		{
			bool isNameUnique = _userService.IsUniqueUser(model.UserName);
			if (!isNameUnique)
			{
				return BadRequest("Username already exists");
			}
			var user = await _userService.Register(model);
			if (user == null)
			{
				return BadRequest("Error while registering");
			}
			return Ok();
		}

		[Authorize(Roles = SD.RoleAdmin)]
		[HttpPost("assignToRole")]
		public async Task<IActionResult> AssignUserToRole([FromBody] RegistrationRequestDto model)
		{
			var user = await _userService.AssignUserToRole(model);
			if (user == null)
			{
				return BadRequest("Error while assigning role");
			}
			return Ok();
		}
	}
}
