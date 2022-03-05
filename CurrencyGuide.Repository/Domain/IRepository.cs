using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyGuide.Storage.Domain
{
	public interface IRepository<T>
	{
		IEnumerable<T> Get(Expression<Func<T, bool>> filter = null);

		T GetById(object id);

		void Insert(T entity);
		void InsertRange(IEnumerable<T> entity);
	}
}
