using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlKing.Server.Models;
using UrlKing.Server.Repository.IRepository;
using UrlKing.Server.Utility;

namespace UrlKing.Server.Controllers
{
	[Route("api/url")]
	[ApiController]
	public class UrlController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private Random _random;

		public UrlController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_random = new Random();
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUrls()
		{
			try
			{
				var urls = await _unitOfWork.ShortenedUrlRepository.GetAllAsync();

				if (urls == null || !urls.Any())
				{
					return NotFound("No URLs found.");
				}

				return Ok(urls);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving URLs: {ex.Message}");
			}
		}

		[HttpGet("{code}")]
		public async Task<IActionResult> GetUrlByCode(string code)
		{
			try
			{
				var url = await _unitOfWork.ShortenedUrlRepository.GetAsync(u => u.Code == code);

				if (url == null)
				{
					return NotFound($"URL with code '{code}' not found.");
				}

				return Ok(url);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving URL: {ex.Message}");
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreateUrl(string url)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(url))
				{
					return BadRequest("URL cannot be empty.");
				}

				if (!Uri.TryCreate(url, UriKind.Absolute, out var _))
				{
					return BadRequest("Url is not valid!");
				}

				if (await _unitOfWork.ShortenedUrlRepository.GetAsync(u => u.LongUrl == url) != null)
				{
					return BadRequest("Url was already shortened!");
				}

				var code = await GenerateShortUrlCode(url);

				var shortenedUrl = new ShortenedUrl
				{
					Code = code,
					LongUrl = url,
					ShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/shorturl/{code}", 
					CreatedDate = DateTime.UtcNow,
					UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) 
				};

				await _unitOfWork.ShortenedUrlRepository.AddAsync(shortenedUrl);
				await _unitOfWork.SaveAsync();

				return CreatedAtAction(nameof(GetUrlByCode), new { code = shortenedUrl.Code }, shortenedUrl);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating URL: {ex.Message}");
			}
		}

		private async Task<string> GenerateShortUrlCode(string url)
		{
			var urls = await _unitOfWork.ShortenedUrlRepository.GetAllAsync();

			while (true)
			{
				char[] chars = new char[SD.MAXCHARACTERS];

				for (int i = 0; i < SD.MAXCHARACTERS; i++)
				{
					chars[i] = SD.allCharacters[_random.Next(SD.allCharacters.Length)];
				}

				string code = new string(chars);

				if (!urls.Any(u => u.Code == code))
				{
					return code;
				}
			}
		}

		[Authorize]
		[HttpDelete("{code}")]
		public async Task<IActionResult> DeleteUrl(string code)
		{
			try
			{
				var url = await _unitOfWork.ShortenedUrlRepository.GetAsync(u => u.Code == code);

				if (url == null)
				{
					return NotFound($"URL with code '{code}' not found.");
				}

				if(url.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier) || !User.IsInRole(SD.RoleAdmin))
				{
					return Forbid("You do not have permission to delete this URL.");
				}

				_unitOfWork.ShortenedUrlRepository.Delete(url);
				await _unitOfWork.SaveAsync();

				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting URL: {ex.Message}");
			}
		}
	}
}
