﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Extensions
{
    public static class QueryableExtensions
    {
	    public static IQueryable<T> SearchFor<T>(this IQueryable<T> source, string searchQuery)
	    {
		    var entityProperties = typeof(T).GetSimplePropertyNames();
		    return source
			    .Where(x => entityProperties
					.Any(y => x.GetType().GetTypeInfo().GetProperty(y).GetValue(x) != null && // todo: improve this query (heavy use of reflection)
				              x.GetType().GetTypeInfo().GetProperty(y).GetValue(x).ToString()
					              .Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase)));
		}
    }
}
