using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.API.Helpers
{
    public class LinkDto
    {
		public string Href { get; set; }
		public string Method { get; set; }
		public string Rel { get; set; }

		public LinkDto(string href, string rel, string method)
		{
			Href = href;
			Rel = rel;
			Method = method;
		}
	}
}
