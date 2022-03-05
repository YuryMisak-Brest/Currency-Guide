using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyGuide.Models;

namespace CurrencyGuide.Models.Interfaces
{
	public interface IExternalCurrencyService
	{
		public IEnumerable<Currency> GetCurrencies();
		public IEnumerable<ExchangeRate> GetExchangeRates(DateTime date);
	}
}
