using AutoMapper;
using CurrencyGuide.Models;

namespace CurrencyGuide.Service
{
	public class CgProfile : Profile
	{
		public CgProfile()
		{
			CreateMap<Currency, Storage.Domain.Currency>().ReverseMap();
			CreateMap<ExchangeRate, Storage.Domain.ExchangeRate>().ReverseMap();
		}
	}
}
