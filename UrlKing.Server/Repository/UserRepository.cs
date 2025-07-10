using Microsoft.AspNetCore.Identity;
using UrlKing.Server.DbContext;
using UrlKing.Server.Models;
using UrlKing.Server.Repository.IRepository;
using UrlKing.Server.Utility;

namespace UrlKing.Server.Repository
{
	public class UserRepository : Repository<ApplicationUser>, IUserRepository
	{
		private readonly ApplicationDbContext _db;

		public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
		{
			_db = applicationDbContext;
		}

		public void Update(ApplicationUser user)
		{
			_db.Users.Update(user);
		}
	}
}
