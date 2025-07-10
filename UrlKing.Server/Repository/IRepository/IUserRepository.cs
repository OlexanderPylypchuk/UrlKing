using UrlKing.Server.Models;

namespace UrlKing.Server.Repository.IRepository
{
	public interface IUserRepository : IRepository<ApplicationUser>
	{
		void Update(ApplicationUser user);
	}
}
