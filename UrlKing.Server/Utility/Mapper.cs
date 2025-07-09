using AutoMapper;
using UrlKing.Server.Models;
using UrlKing.Server.Models.Dtos;

namespace UrlKing.Server.Utility
{
	public class MapperConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mapping = new MapperConfiguration(config =>
			{
				config.CreateMap<ShortenedUrl, ShortenedUrlDto>().ReverseMap();
				config.CreateMap<AboutContent, AboutContentDto>().ReverseMap();
			}, null);
			return mapping;
		}
	}
}
