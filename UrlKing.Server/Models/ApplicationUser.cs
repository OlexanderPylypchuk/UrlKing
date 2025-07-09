using Microsoft.AspNetCore.Identity;

namespace UrlKing.Server.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}
