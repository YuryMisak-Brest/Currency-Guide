using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyGuide.Storage.Domain
{
	public class ExchangeRate
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int CurrencyId { get; set; }
		public decimal Rate { get; set; }
		public DateTime Date { get; set; }
	}
}
