using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace UrlKing.Server.Models
{
	public class AboutContent
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime SavedTime { get; set; }
		[ForeignKey("ApplicationUser")]
		public string UserId { get; set; }
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
	}
}
