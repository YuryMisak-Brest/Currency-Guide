using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CurrencyGuide.Storage;
using Microsoft.EntityFrameworkCore;

namespace CurrencyGuide.Storage.Domain
{
	public class Repository<T> : IRepository<T> where T: class
	{
		private readonly CgContext _context;
		private readonly DbSet<T> _dbSet;

		public Repository(CgContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}
			return query.ToList();
		}

		public T GetById(object id)
		{
			return _dbSet.Find(id);
		}

		public void Insert(T entity)
		{
			_dbSet.Add(entity);
			_context.SaveChanges();
		}
		public void InsertRange(IEnumerable<T> entity)
		{
			_dbSet.AddRange(entity);
			_context.SaveChanges();
		}
	}
}
