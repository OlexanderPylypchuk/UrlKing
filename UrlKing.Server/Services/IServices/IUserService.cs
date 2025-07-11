using UrlKing.Server.Models.Dtos;

namespace UrlKing.Server.Services.IServices
{
	public interface IUserService
	{
		Task<IEnumerable<UserDto>> GetUsersAsync();
		Task<UserDto> GetUserAsync(string username);
		bool IsUniqueUser(string username);
		Task<TokenDto> Login(LoginRequestDto loginRequestDto);
		Task<TokenDto> Register(RegistrationRequestDto registrationRequestDto);
		Task<UserDto> AssignUserToRole(RegistrationRequestDto registrationRequestDto);
	}
}
