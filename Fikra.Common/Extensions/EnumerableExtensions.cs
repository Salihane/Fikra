using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Extensions
{
    public static class EnumerableExtensions
    {
	    public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, params string[] fields)
	    {
		    if (source == null)
			    throw new ArgumentNullException(nameof(source));

		    var sourceType = typeof(TSource);
		    var propertyInfos = sourceType.GetPropertiesExt(fields).ToList();

		    foreach (var item in source)
		    {
			    yield return item.ShapeData(propertyInfos);
		    }
	    }
	}
}
