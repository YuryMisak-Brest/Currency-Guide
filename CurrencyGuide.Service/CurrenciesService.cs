using System;
using System.Collections.Generic;
using AutoMapper;
using CurrencyGuide.Models.Interfaces;
using CurrencyGuide.Storage.Domain;
using Currency = CurrencyGuide.Models.Currency;

namespace CurrencyGuide.Service
{
	public class CurrenciesService : ICurrenciesService
	{
		private readonly IRepository<Storage.Domain.Currency> _currencyRepository;
		private readonly IMapper _mapper;

		public CurrenciesService(IRepository<Storage.Domain.Currency> currencyRepository, IMapper mapper)
		{
			_currencyRepository = currencyRepository;
			_mapper = mapper;
		}
		public IEnumerable<Currency> Get()
		{
			return _mapper.Map<IEnumerable<Currency>>(_currencyRepository.Get());
		}
	}
}
