using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlKing.Server.Repository.IRepository;

namespace UrlKing.Server.Controllers
{
	[Route("api/url")]
	[ApiController]
	public class UrlController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public UrlController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
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
	}
}
