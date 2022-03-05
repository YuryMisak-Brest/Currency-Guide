using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyGuide.Models.Interfaces
{
	public interface ICurrenciesService
	{
		IEnumerable<Currency> Get();
	}
}
