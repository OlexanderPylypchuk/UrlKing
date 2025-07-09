using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UrlKing.Server.Models
{
	public class ShortenedUrl
	{
		[Key]
		public string Code { get; set; }
		[Required]
		public string LongUrl { get; set; }
		[ValidateNever]
		public string ShortUrl { get; set; }
		public DateTime CreatedDate { get; set; }
		[ForeignKey("ApplicationUser")]
		public string UserId { get; set; }
		[ValidateNever, DisplayName("CreatedBy")]
		public ApplicationUser ApplicationUser { get; set; }
	}
}
