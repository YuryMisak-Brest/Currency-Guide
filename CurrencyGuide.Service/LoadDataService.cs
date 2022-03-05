using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CurrencyGuide.Models.Interfaces;
using CurrencyGuide.Service.Utilities;
using CurrencyGuide.Storage.Domain;

namespace CurrencyGuide.Service
{
	public class LoadDataService : ILoadDataService
	{
		private readonly IExternalCurrencyService _ecService; 
		private readonly IRepository<Currency> _currencyRepository;
		private readonly IRepository<ExchangeRate> _exchangeRateRepository;
		private readonly IMapper _mapper;

		public LoadDataService(IExternalCurrencyService ecService, IRepository<Currency> currencyRepository, IRepository<ExchangeRate> exchangeRateRepository, IMapper mapper)
		{
			_ecService = ecService;
			_currencyRepository = currencyRepository;
			_mapper = mapper;
			_exchangeRateRepository = exchangeRateRepository;
		}
		public void Setup(DateTime startDate)
		{
			var currencies = _mapper.Map<IEnumerable<Currency>>(_ecService.GetCurrencies()).ToList();
			_currencyRepository.InsertRange(currencies);
			var currencyIds= currencies.ToDictionary(x => x.Iso, y => y.Id);
			foreach (var date in startDate.EveryDayTillToday())
			{
				var rates = _ecService.GetExchangeRates(date);
				
				foreach (var rate in rates)
				{
					rate.CurrencyId = currencyIds[rate.CurrencyIso];
				}
				var adaptedRates= _mapper.Map<IEnumerable<ExchangeRate>>(rates);

				_exchangeRateRepository.InsertRange(adaptedRates);
			}

		}

		public void LoadOneDayData(DateTime date)
		{
			try
			{
				var currencies = _mapper.Map<IEnumerable<Currency>>(_ecService.GetCurrencies());
				var existingCurrencies = _currencyRepository.Get().ToDictionary(x => x.Iso, y => y.Id);

				var newCurrencies = currencies.Where(x => !existingCurrencies.ContainsKey(x.Iso)).ToList();
				if (newCurrencies.Any())
				{
					_currencyRepository.InsertRange(newCurrencies);
					existingCurrencies = existingCurrencies.Concat(newCurrencies.ToDictionary(x => x.Iso, y => y.Id))
						.ToDictionary(x => x.Key, y => y.Value);
				}


				var rates = _ecService.GetExchangeRates(date);
				foreach (var rate in rates)
				{
					rate.CurrencyId = existingCurrencies[rate.CurrencyIso];
				}

				var adaptedRates = _mapper.Map<IEnumerable<ExchangeRate>>(rates);

				_exchangeRateRepository.InsertRange(adaptedRates);
			}
			catch (Exception)
			{
				//logging should be here, but not for the sandbox project
				throw new Exception("Error loading currency rates. Please try again later");
			}
		}
	}
}
