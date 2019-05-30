using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Extensions
{
    public static class GenericExtensions
    {
	    public static ExpandoObject ShapeData<TSource>(this TSource source, params string[] propertyNames)
	    {
		    if (source == null)
			    throw new ArgumentNullException(nameof(source));

		    var sourceType = typeof(TSource);
		    var propertyInfos = sourceType.GetPropertiesExt(propertyNames);
		    return source.ShapeData(propertyInfos);
	    }

	    public static ExpandoObject ShapeData<TSource>(this TSource source, IEnumerable<PropertyInfo> propertyInfos)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			var expandoObject = new ExpandoObject();
			foreach (var propertyInfo in propertyInfos)
			{
				var propertyValue = propertyInfo.GetValue(source);
				((IDictionary<string, object>)expandoObject).Add(propertyInfo.Name, propertyValue);
			}

			return expandoObject;
		}
    }
}
