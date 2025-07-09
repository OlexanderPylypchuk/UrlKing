using UrlKing.Server.Models;

namespace UrlKing.Server.Repository.IRepository
{
	public interface IAboutContentRepository:IRepository<AboutContent>
	{
		void Update(AboutContent aboutContent);
	}
}
