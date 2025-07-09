using UrlKing.Server.DbContext;
using UrlKing.Server.Models;
using UrlKing.Server.Repository.IRepository;

namespace UrlKing.Server.Repository
{
	public class ShortenedUrlRepository : Repository<ShortenedUrl>, IShortenedUrlRepository
	{
		private readonly ApplicationDbContext _db;

		public ShortenedUrlRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
			_db = applicationDbContext;
		}

		public void Update(ShortenedUrl shortenedUrl)
		{
			_db.ShortenedUrls.Update(shortenedUrl);
		}
	}
}
