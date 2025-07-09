using UrlKing.Server.Models;

namespace UrlKing.Server.Repository.IRepository
{
	public interface IUserRepository : IRepository<ApplicationUser>
	{
		void Update(ApplicationUser user);
		Task<bool> Login(string username, string password);
		Task<bool> Register(string username, string password, string name, string role);
		Task Logout();
	}
}
