using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyGuide.Models.Interfaces
{
	public interface IConversionRatesService
	{
		ConversionRate GetConversionRate(int currencyFrom, int currencyTo, DateTime? date);
	}
}
