using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyGuide.Models
{
	public class ExchangeRate
	{
		public int Id { get; set; }
		public int CurrencyId { get; set; }
		public string CurrencyIso { get; set; }
		public decimal Rate { get; set; }
		public DateTime? Date { get; set; }
	}
}
