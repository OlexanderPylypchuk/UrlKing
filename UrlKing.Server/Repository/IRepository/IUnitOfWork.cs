namespace UrlKing.Server.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IUserRepository UserRepository { get; }
		IShortenedUrlRepository ShortenedUrlRepository { get; }
		IAboutContentRepository AboutContentRepository { get; }

		Task SaveAsync();
	}
}
