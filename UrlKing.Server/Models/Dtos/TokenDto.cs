﻿namespace UrlKing.Server.Models.Dtos
{
	public class TokenDto
	{
		public string AccessToken { get; set; }
		public UserDto User { get; set; }
	}
}
