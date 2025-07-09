using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using UrlKing.Server.DbContext;
using UrlKing.Server.Repository.IRepository;

namespace UrlKing.Server.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		private DbSet<T> _dbSet;

		public Repository(ApplicationDbContext db)
		{
			_db = db;
			_dbSet = _db.Set<T>();
		}

		public async Task AddAsync(T item)
		{
			await _dbSet.AddAsync(item);
		}

		public void Delete(T item)
		{
			_dbSet.Remove(item);
		}

		public void DeleteRange(IEnumerable<T> items)
		{
			_dbSet.RemoveRange(items);
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (includeProperties != null)
			{
				foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}

			return await query.ToListAsync();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (includeProperties != null)
			{
				foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}

			return await query.FirstOrDefaultAsync();
		}
	}
}
