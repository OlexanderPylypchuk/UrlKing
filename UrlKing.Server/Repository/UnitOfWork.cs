using Microsoft.AspNetCore.Identity;
using UrlKing.Server.DbContext;
using UrlKing.Server.Repository.IRepository;

namespace UrlKing.Server.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;

		public UnitOfWork(ApplicationDbContext applicationDbContext)
		{
			_db = applicationDbContext;
			UserRepository = new UserRepository(_db);
			ShortenedUrlRepository = new ShortenedUrlRepository(_db);
		}

		public IUserRepository UserRepository { get; private set; }
		public IShortenedUrlRepository ShortenedUrlRepository { get; private set; }
		public IAboutContentRepository AboutContentRepository { get; private set; }

		public async Task SaveAsync()
		{
			await _db.SaveChangesAsync();
		}
	}
}
