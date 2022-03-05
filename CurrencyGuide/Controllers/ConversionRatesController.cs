using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyGuide.Models;
using CurrencyGuide.Models.Interfaces;

namespace CurrencyGuide.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConversionRatesController : ControllerBase
	{
		private readonly IConversionRatesService _conversionRatesService;

		public ConversionRatesController(IConversionRatesService conversionRatesService)
		{
			_conversionRatesService = conversionRatesService;
		}

		[HttpGet]
		public ConversionRate Get(int currencyFrom, int currencyTo, DateTime? date)
		{
			return _conversionRatesService.GetConversionRate(currencyFrom, currencyTo, date);
		}
	}
}
