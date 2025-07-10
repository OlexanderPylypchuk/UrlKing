namespace UrlKing.Server.Models.Dtos
{
	public class UserDto
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string UserName { get; set; }
		public IEnumerable<string>? Roles { get; set; }
	}
}
