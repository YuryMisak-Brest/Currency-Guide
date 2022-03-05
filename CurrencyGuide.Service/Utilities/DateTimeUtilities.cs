using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyGuide.Service.Utilities
{
	public static class DateTimeUtilities
	{
		public static IEnumerable<DateTime> EveryDayTillToday(this DateTime from)
		{
			for (var day = from.Date; day.Date <= DateTime.Today.Date; day = day.AddDays(1))
				yield return day;
		}
	}
}
