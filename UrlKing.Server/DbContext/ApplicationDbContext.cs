using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlKing.Server.Models;

namespace UrlKing.Server.DbContext
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
		public DbSet<AboutContent> Abouts { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
