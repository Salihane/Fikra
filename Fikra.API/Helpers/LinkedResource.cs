using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.API.Helpers
{
    public class LinkedResource
    {
	    public IEnumerable<IDictionary<string, object>> Value { get; set; }
	    public IEnumerable<LinkDto> Links { get; set; }
    }
}
