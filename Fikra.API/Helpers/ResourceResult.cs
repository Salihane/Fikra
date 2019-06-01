using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Fikra.API.Helpers
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
