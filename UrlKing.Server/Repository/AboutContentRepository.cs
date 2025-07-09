using UrlKing.Server.DbContext;
using UrlKing.Server.Models;
using UrlKing.Server.Repository.IRepository;

namespace UrlKing.Server.Repository
{
	public class AboutContentRepository:Repository<AboutContent>, IAboutContentRepository
	{
		private readonly ApplicationDbContext _db;

		public AboutContentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
			_db = applicationDbContext;
		}

		public void Update(AboutContent aboutContent)
		{
			_db.Abouts.Update(aboutContent);
		}
	}
}
