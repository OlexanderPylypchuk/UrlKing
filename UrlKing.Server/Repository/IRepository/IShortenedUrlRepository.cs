using UrlKing.Server.Models;

namespace UrlKing.Server.Repository.IRepository
{
	public interface IShortenedUrlRepository : IRepository<ShortenedUrl>
	{
		void Update(ShortenedUrl shortenedUrl);
	}
}
