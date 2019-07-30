using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace Fikra.API.Extensions
{
    public static class HttpResponseExtensions
    {
	    public static void AddHeaders(this Microsoft.AspNetCore.Http.HttpResponse response, Dictionary<string, StringValues> headers)
	    {
		    if (headers == null || !headers.Any()) return;
		    foreach (var (key, value) in headers)
		    {
			    response.Headers.Add(key, value);
		    }
	    }
    }
}
