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
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserRepository(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> rolemanager, SignInManager<IdentityUser> signInManager) : base(applicationDbContext)
		{
			_db = applicationDbContext;
			_userManager = userManager;
			_roleManager = rolemanager;
			_signInManager = signInManager;
		}

		public async Task<bool> Login(string username, string password)
		{
			var result = await _signInManager.PasswordSignInAsync(username, password, true, false);
			return result.Succeeded;
		}

		public async Task Logout()
		{
			await _signInManager.SignOutAsync();
		}

		public async Task<bool> Register(string username, string password, string name, string role)
		{
			var usernameUnique = _db.Users.FirstOrDefault(u => u.UserName == username) == null;
			if (usernameUnique && (role == SD.RoleCustomer || role == SD.RoleAdmin))
			{
				var user = new ApplicationUser()
				{
					Name = name,
					UserName = username,
					Email = username,
					NormalizedEmail = username.ToUpper(),
				};

				await _userManager.CreateAsync(user, password);
				if (await _roleManager.RoleExistsAsync(role))
				{
					await _userManager.AddToRoleAsync(user, role);
				}
				else
				{
					await _userManager.AddToRoleAsync(user, SD.RoleCustomer);
				}
				return true;
			}
			return false;
		}

		public void Update(ApplicationUser user)
		{
			_db.Users.Update(user);
		}
	}
}
