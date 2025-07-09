using Microsoft.AspNetCore.Identity;
using UrlKing.Server.DbContext;
using UrlKing.Server.Repository.IRepository;

namespace UrlKing.Server.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UnitOfWork(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> rolemanager, SignInManager<IdentityUser> signInManager)
		{
			_db = applicationDbContext;
			_userManager = userManager;
			_roleManager = rolemanager;
			_signInManager = signInManager;
			UserRepository = new UserRepository(_db, _userManager, _roleManager, _signInManager);
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
