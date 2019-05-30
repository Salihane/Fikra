using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fikra.Common.Extensions
{
    public static class TypeExtensions
    {
	    private const BindingFlags DefaultFlags = BindingFlags.Public |
	                                              BindingFlags.Instance |
	                                              BindingFlags.IgnoreCase;
	    public static bool IsSimpleType(this Type type)
	    {
		    return
			    type.GetTypeInfo().IsValueType ||
			    type.GetTypeInfo().IsPrimitive ||
			    new[]
			    {
				    typeof(string),
				    typeof(decimal),
				    typeof(DateTime),
				    typeof(DateTimeOffset),
				    typeof(TimeSpan),
				    typeof(Guid)
			    }.Contains(type) ||
			    Convert.GetTypeCode(type) != TypeCode.Object;
	    }

		public static bool HasProperties(this Type type, params string[] properties)
		{
			if (properties == null || properties.Length == 0)
				return true;

			return properties
				.Select(field => field.Trim())
				.Select(propName => type.GetProperty(propName, DefaultFlags))
				.All(propInfo => propInfo != null);
		}

		public static IEnumerable<PropertyInfo> GetPropertiesExt(this Type type, params string[] propertyNames)
		{
			return type.GetPropertiesExt(DefaultFlags, propertyNames);
		}

		public static IEnumerable<PropertyInfo> GetPropertiesExt(this Type type,
			BindingFlags flags, params string[] propertyNames)
		{
			var properties = new List<PropertyInfo>();

			var noPropertyNames = propertyNames == null || propertyNames.Length == 0;
			if (noPropertyNames)
			{
				properties =new List<PropertyInfo>(type.GetProperties(DefaultFlags));
			}
			else
			{
				properties.AddRange(propertyNames
					.Select(propertyName => propertyName.Trim())
					.Select(propertyName => type.GetProperty(propertyName, flags))
					.Where(propertyInfo => propertyInfo != null));
			}


			return properties;
		}
		public static IEnumerable<string> GetSimplePropertyNames(this Type type,
			BindingFlags flags = DefaultFlags)
		{
			var properties = type.GetProperties(flags);

			return (from propInfo in properties
				where propInfo.PropertyType.IsSimpleType()
				select propInfo.Name);
		}
	}
}
