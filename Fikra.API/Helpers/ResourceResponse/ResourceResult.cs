using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Fikra.API.Helpers.ResourceResponse
{
	public class ResourceResult
	{
		public int StatusCode { get; set; }
		public object ResultObject { get; set; }
		public string Message { get; set; }
		public Dictionary<string, StringValues> ResponseHeaders { get; set; }

		public ResourceResult()
		{
			ResponseHeaders = new Dictionary<string, StringValues>();
		}
	}
}
