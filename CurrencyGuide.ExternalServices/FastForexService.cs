using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using CurrencyGuide.Models;
using CurrencyGuide.Models.Interfaces;

namespace CurrencyGuide.ExternalServices
{
	public class FastForexService : IExternalCurrencyService
	{
		private readonly string _apiKey;
		private readonly string _apiUrl;
		public FastForexService(string apiUrl, string apiKey)
		{
			_apiUrl = apiUrl;
			_apiKey = apiKey;
		}

		public IEnumerable<Models.Currency> GetCurrencies()
		{
			var searchUrl = $"{_apiUrl}/currencies?api_key={_apiKey}";
			string json;
			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(searchUrl);
			}
			var result = JsonSerializer.Deserialize<Currency>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			return result?.Currencies.Select(x => new Models.Currency {Iso = x.Key, Name = x.Value});
		}

		public IEnumerable<Models.ExchangeRate> GetExchangeRates(DateTime date)
		{
			var isHistorical = date.Date < DateTime.Today;
			var searchUrl = isHistorical ? $"{_apiUrl}/historical?date={date.ToString("yyyy-MM-dd")}&from=EUR&api_key={_apiKey}" : $"{_apiUrl}/fetch-all?from=EUR&api_key={_apiKey}";

			string json;
			using (WebClient wc = new WebClient())
			{
				json = wc.DownloadString(searchUrl);
			}
			var result = JsonSerializer.Deserialize<Rates>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			return result?.Results.Select(x => new Models.ExchangeRate() { CurrencyIso  = x.Key, Date = result.Updated ?? result.Date, Rate = x.Value}).ToList();
		}
	}
}
