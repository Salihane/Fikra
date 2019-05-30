using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.API.Helpers
{
    public class ResourceParameterDto
    {
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public string Filter { get; set; }
		public string Search { get; set; }
		public string Fields { get; set; }
	}
}
