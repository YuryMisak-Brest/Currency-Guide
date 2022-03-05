using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CurrencyGuide.Filters
{
	public class ExceptionFilter : IActionFilter
	{
		public int Order => int.MaxValue - 10;

		public void OnActionExecuting(ActionExecutingContext context) { }

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Exception != null)
			{
				context.Result = new ObjectResult(context.Exception.Message)
				{
					StatusCode = 400
				};

				context.ExceptionHandled = true;
			}
		}
	}
}
