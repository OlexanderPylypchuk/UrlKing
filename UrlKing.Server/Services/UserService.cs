﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlKing.Server.DbContext;
using UrlKing.Server.Models.Dtos;
using UrlKing.Server.Models;
using UrlKing.Server.Utility;
using Microsoft.EntityFrameworkCore;
using UrlKing.Server.Services.IServices;

namespace UrlKing.Server.Services
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;
		private string secretKey;
		private string issuer;
		private string audience;

		public UserService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper)
		{
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
			secretKey = configuration.GetValue<string>("ApiSettings:Secret");
			issuer = configuration.GetValue<string>("ApiSettings:Issuer");
			audience = configuration.GetValue<string>("ApiSettings:Audience");
			_mapper = mapper;
		}

		public bool IsUniqueUser(string username)
		{
			var user = _db.Users.FirstOrDefault(u => u.UserName == username);
			if (user == null)
			{
				return true;
			}
			return false;
		}

		public async Task<TokenDto> Login(LoginRequestDto loginRequestDto)
		{
			var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

			bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

			if (user == null || !isValid)
			{
				return new TokenDto
				{
					AccessToken = "",
				};
			}

			var tokenid = $"JWI{Guid.NewGuid()}";
			var accessToken = await GetAcessTokenAsync(user, tokenid);


			TokenDto tokendtoDto = new TokenDto()
			{
				AccessToken = accessToken,
				User = _mapper.Map<UserDto>(user),
			};
			return tokendtoDto;
		}

		private async Task<string> GetAcessTokenAsync(ApplicationUser applicationUser, string tokenid)
		{
			var roles = await _userManager.GetRolesAsync(applicationUser);
			var tokenhandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secretKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name,applicationUser.Id.ToString()),
					new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
					new Claim(JwtRegisteredClaimNames.Jti, tokenid),
					new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id)
				}),
				Expires = DateTime.UtcNow.AddHours(2),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
				Issuer = issuer,
				Audience = audience
			};

			var token = tokenhandler.CreateToken(tokenDescriptor);
			return tokenhandler.WriteToken(token);
		}

		public async Task<UserDto> Register(RegistrationRequestDto registrationRequestDto)
		{
			ApplicationUser user = new()
			{
				Name = registrationRequestDto.Name,
				Email = registrationRequestDto.UserName,
				NormalizedEmail = registrationRequestDto.UserName.ToUpper(),
				UserName = registrationRequestDto.UserName,
			};
			try
			{
				var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
				if (result.Succeeded)
				{
					if (!await _roleManager.RoleExistsAsync(SD.RoleUser))
					{
						await _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin));
						await _roleManager.CreateAsync(new IdentityRole(SD.RoleUser));
					}
					await _userManager.AddToRoleAsync(user, SD.RoleUser);
					var userToReturn = _db.Users.FirstOrDefault(u => u.UserName == registrationRequestDto.UserName);
					return _mapper.Map<UserDto>(userToReturn);
				}
			}
			catch (Exception ex)
			{

			}
			return null;
		}
		public async Task<UserDto> AssignUserToRole(RegistrationRequestDto registrationRequestDto)
		{
			try
			{
				var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == registrationRequestDto.UserName);
				if (user != null)
				{
					if (!await _roleManager.RoleExistsAsync(registrationRequestDto.Role))
					{
						await _roleManager.CreateAsync(new IdentityRole(registrationRequestDto.Role));
					}

					var roles = await _userManager.GetRolesAsync(user);
					await _userManager.RemoveFromRolesAsync(user, roles);

					await _userManager.AddToRoleAsync(user, registrationRequestDto.Role);

					var userToReturn = _db.Users.FirstOrDefault(u => u.UserName == registrationRequestDto.UserName);
					return _mapper.Map<UserDto>(userToReturn);
				}
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public async Task<IEnumerable<UserDto>> GetUsersAsync()
		{
			var users = await _db.Users.ToListAsync();
			var result = new List<UserDto>();

			foreach (var user in users)
			{
				var userDto = _mapper.Map<UserDto>(user);
				userDto.Roles = await _userManager.GetRolesAsync(user);
				result.Add(userDto);
			}

			return result;
		}


		public async Task<UserDto> GetUserAsync(string username)
		{
			var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
			var result = _mapper.Map<UserDto>(user);
			result.Roles = await _userManager.GetRolesAsync(user);
			return result;
		}
	}
}
