using System;
using System.Data;
using System.Linq;
using CurrencyGuide.Models;
using CurrencyGuide.Models.Interfaces;
using CurrencyGuide.Storage.Domain;
using ExchangeRate = CurrencyGuide.Storage.Domain.ExchangeRate;

namespace CurrencyGuide.Service
{
	public class ConversionRateService : IConversionRatesService
	{
		private readonly IRepository<ExchangeRate> _repository;
		private readonly ILoadDataService _loadDataService;

		public ConversionRateService(IRepository<ExchangeRate> repository, ILoadDataService loadDataService)
		{
			_repository = repository;
			_loadDataService = loadDataService;
		}
		public ConversionRate GetConversionRate(int currencyFrom, int currencyTo, DateTime? date)
		{
			var rate = GetRatesFromStorage(currencyFrom, currencyTo, date);
			if (rate == null)
			{
				_loadDataService.LoadOneDayData(date ?? DateTime.UtcNow);
			}
			rate = GetRatesFromStorage(currencyFrom, currencyTo, date);
			if (rate == null)
			{
				throw new DataException("Conversion rate not found");
			}
			return new ConversionRate
			{
				CurrencyFrom = currencyFrom, CurrencyTo = currencyTo, Date = date, Rate = rate.Value
			};
		}

		private decimal? GetRatesFromStorage(int currencyFrom, int currencyTo, DateTime? date)
		{
			var rateDate = (date ?? DateTime.Today);
			var rates = _repository.Get(x => (x.CurrencyId == currencyFrom || x.CurrencyId == currencyTo) && x.Date.Date == rateDate.Date).ToList();
			var rateFrom = rates.Find(x => x.CurrencyId == currencyFrom);
			var rateTo = rates.Find(x => x.CurrencyId == currencyTo);
			if (rateTo == null || rateFrom == null)
				return null;
			return rateTo.Rate / rateFrom.Rate;

		}
	}
}
