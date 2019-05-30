using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Extensions
{
    public static class CollectionExtensions
    {
		/// <summary>
		/// Adds the given item to the collection if it doesn't exist yet in the collection
		/// </summary>
		/// <param name="source">Source collection</param>
		/// <param name="item">The item to be added to the collection</param>
		/// <returns>Returns true if the item did not exist and is added, otherwise returns false</returns>
		public static bool AddIfNotIn(this ICollection<string> source, string item)
		{
			var itemNotIn = source != null && !source.Contains(item);
			if (!itemNotIn) return false;

			source.Add(item);
			return true;
		}
		
		public static string ToCommaSeparatedString(this ICollection<string> source)
		{
			return source == null 
				? null 
				: string.Join(Constants.Chars.Comma, source);
		}

		public static string[] ToStringArray(this ICollection<string> source)
		{
			return source?.ToArray();
		}
	}
}
