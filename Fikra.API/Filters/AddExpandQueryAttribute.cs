using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using SBHLib.Extensions;

namespace Fikra.API.Filters
{
  public class AddExpandQueryAttribute : ActionFilterAttribute
  {
    private const string ODataExpandOption = "$expand";
    private readonly string[] _propertiesToExpand;

    public AddExpandQueryAttribute(params string[] propertiesToExpand)
    {
      _propertiesToExpand = propertiesToExpand;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
      base.OnActionExecuting(context);

      var request = context.HttpContext.Request;
      var mediaType = request.Headers[HttpHeader.Accept];
      if (mediaType == CustomMediaTypes.HateoasJson)
      {
        var store = request.Query.ToDictionary();
        store.Add(ODataExpandOption, _propertiesToExpand.ToCsvString());

        request.QueryString = QueryString.Create(store);
      }

      base.OnActionExecuting(context);
    }
  }
}