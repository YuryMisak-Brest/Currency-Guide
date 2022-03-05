using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyGuide.ExternalServices
{
	public class Rates
	{
		public string Base { get; set; }
		public Dictionary<string, decimal> Results { get; set; }
		public DateTime? Date { get; set; }
		[JsonConverter(typeof(DateTimeConverter))]
		public DateTime? Updated { get; set; }
	}
}
