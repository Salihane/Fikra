using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Extensions
{
    public static class BooleanExtensions
    {
	    public static bool ToBool(this bool? source)
	    {
		    return source ?? false;
	    }
    }
}
