using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyGuide.Models;
using CurrencyGuide.Models.Interfaces;

namespace CurrencyGuide.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CurrenciesController : ControllerBase
	{
		private readonly ICurrenciesService _currenciesService;

		public CurrenciesController(ICurrenciesService currenciesService)
		{
			_currenciesService = currenciesService;
		}

		[HttpGet]
		public IEnumerable<Currency> Get()
		{
			return _currenciesService.Get();
		}
	}
}
