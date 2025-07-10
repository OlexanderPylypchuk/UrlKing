namespace UrlKing.Server.Models.Dtos
{
	public class ShortenedUrlDto
	{
		public string? Code { get; set; }
		public string LongUrl { get; set; }
		public string? ShortUrl { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? UserId { get; set; }
	}
}
