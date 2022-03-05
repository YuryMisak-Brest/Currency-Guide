using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyGuide.Models
{
	public class ConversionRate
	{
		public int CurrencyFrom { get; set; } 
		public int CurrencyTo { get; set; }
		public DateTime? Date { get; set; }
		public decimal Rate { get; set; }
	}
}
