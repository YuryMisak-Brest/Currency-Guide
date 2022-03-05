using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using CurrencyGuide.Models.Interfaces;
using CurrencyGuide.Service;
using CurrencyGuide.Storage.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CurrencyGuide.Tests
{
	[TestClass]
	public class ConversionRateServiceTest
	{
		[TestMethod]
		[ExpectedException(typeof(DataException), "Conversion rate not found")]
		public void LackOfExchangeRatesTest()
		{
			var repoMock = new Mock<IRepository<ExchangeRate>>();
			var esMock = new Mock<ILoadDataService>();
			repoMock.Setup(x => x.Get(It.IsAny<Expression<Func<ExchangeRate, bool>>>())).Returns(new List<ExchangeRate>());
			esMock.Setup(x => x.LoadOneDayData(It.IsAny<DateTime>()));
			IConversionRatesService service = new ConversionRateService(repoMock.Object, esMock.Object);
			service.GetConversionRate(1, 2, DateTime.Today);
		}

		[TestMethod]
		public void ReturnsExchangeRatesTest()
		{
			var repoMock = new Mock<IRepository<ExchangeRate>>();
			var esMock = new Mock<ILoadDataService>();
			IList<ExchangeRate> rates = new List<ExchangeRate>
				{new() {CurrencyId = 1, Date = DateTime.Today, Rate = 1}, new() {CurrencyId = 2, Date = DateTime.Today, Rate = 2}};
			repoMock.Setup(x => x.Get(It.IsAny<Expression<Func<ExchangeRate, bool>>>())).Returns(rates);
			IConversionRatesService service = new ConversionRateService(repoMock.Object, esMock.Object);
			var rate = service.GetConversionRate(1, 2, DateTime.Today);
			Assert.AreEqual(DateTime.Today, rate.Date );
			Assert.AreEqual(1, rate.CurrencyFrom);
			Assert.AreEqual(2, rate.CurrencyTo);
			Assert.AreEqual(2, rate.Rate);
		}

		[TestMethod]
		public void ReturnsExchangeRatesFiltersByDateTest()
		{
			var repoMock = new Mock<IRepository<ExchangeRate>>();
			var esMock = new Mock<ILoadDataService>();
			IList<ExchangeRate> rates = new List<ExchangeRate>
			{
				new() {CurrencyId = 1, Date = DateTime.Today, Rate = 1}, new() {CurrencyId = 2, Date = DateTime.Today, Rate = 2},
				new() {CurrencyId = 1, Date = DateTime.Today.AddDays(-1), Rate = 1}, new() {CurrencyId = 2, Date = DateTime.Today.AddDays(-1), Rate = 3}
			};
			repoMock.Setup(x => x.Get(It.IsAny<Expression<Func<ExchangeRate, bool>>>())).Returns(rates.Where(x => x.Date == DateTime.Today));
			IConversionRatesService service = new ConversionRateService(repoMock.Object, esMock.Object);
			var rate = service.GetConversionRate(1, 2, DateTime.Today);
			Assert.AreEqual(DateTime.Today, rate.Date);
			Assert.AreEqual(1, rate.CurrencyFrom);
			Assert.AreEqual(2, rate.CurrencyTo);
			Assert.AreEqual(2, rate.Rate);
			repoMock.Setup(x => x.Get(It.IsAny<Expression<Func<ExchangeRate, bool>>>())).Returns(rates.Where(x => x.Date == DateTime.Today.AddDays(-1)));
			var rateYesterday = service.GetConversionRate(1, 2, DateTime.Today.AddDays(-1));
			Assert.AreEqual(DateTime.Today.AddDays(-1), rateYesterday.Date);
			Assert.AreEqual(1, rateYesterday.CurrencyFrom);
			Assert.AreEqual(2, rateYesterday.CurrencyTo);
			Assert.AreEqual(3, rateYesterday.Rate);
		}

		[TestMethod]
		public void ReturnsExchangeRatesAfterDownloadTest()
		{
			var repoMock = new Mock<IRepository<ExchangeRate>>();
			var esMock = new Mock<ILoadDataService>();
			IList<ExchangeRate> rates = new List<ExchangeRate>();
			repoMock.Setup(x => x.Get(It.IsAny<Expression<Func<ExchangeRate, bool>>>())).Returns(rates);
			esMock.Setup(x => x.LoadOneDayData(It.IsAny<DateTime>())).Callback((DateTime date) => { rates.Add(new() { CurrencyId = 1, Date = DateTime.Today, Rate = 1 }); rates.Add(new() { CurrencyId = 2, Date = DateTime.Today, Rate = 2 }); });
			IConversionRatesService service = new ConversionRateService(repoMock.Object, esMock.Object);
			var rate = service.GetConversionRate(1, 2, DateTime.Today);
			Assert.AreEqual(DateTime.Today, rate.Date);
			Assert.AreEqual(1, rate.CurrencyFrom);
			Assert.AreEqual(2, rate.CurrencyTo);
			Assert.AreEqual(2, rate.Rate);
		}
	}
}
