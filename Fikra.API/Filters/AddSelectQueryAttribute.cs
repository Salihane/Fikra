using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace Fikra.API.Filters
{
	public class AddSelectQueryAttribute : ActionFilterAttribute
	{
		private const string ODataSelectOption = "$select";
		private readonly string _selectValue;

		public AddSelectQueryAttribute(string select)
		{
			_selectValue = select;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			var request = context.HttpContext.Request;
			var query = request.Query;
			var selectQuery = query[ODataSelectOption];
			var shit = selectQuery.Count > 0 ? $"{selectQuery}," : string.Empty;
			request.QueryString = QueryString
				.Create(ODataSelectOption, $"{shit}{_selectValue}");

			base.OnActionExecuting(context);
		}
	}
}
