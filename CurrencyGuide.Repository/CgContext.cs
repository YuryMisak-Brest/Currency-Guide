using System;
using CurrencyGuide.Storage.Domain;
using Microsoft.EntityFrameworkCore;

namespace CurrencyGuide.Storage
{
	public class CgContext : DbContext
	{
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<ExchangeRate> ExchangeRates { get; set; }

		public CgContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{
		}
	}

}
