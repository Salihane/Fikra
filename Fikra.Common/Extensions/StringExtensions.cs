using System;
using System.Globalization;
using Fikra.Common.Constants;

namespace Fikra.Common.Extensions
{
	public static class StringExtensions
	{
		public static DateTime BuildDate(this string value)
		{
			var result = value.TryParseToDateTime();
			if (result != DateTime.MinValue) return result;

			if (string.IsNullOrEmpty(value)) return DateTime.MinValue;

			var dateParts = value.Split(Chars.Slash, Chars.Dash, Chars.Space);
			switch (dateParts.Length)
			{
				case 2:
				{
					string str;

					if (dateParts[0].IsYearNumber())
					{
						str = dateParts[1].IsMonthNumber() 
							? $"{DateTime.Now.Day}/{dateParts[1]}/{dateParts[0]}" 
							: $"{dateParts[1]}/{DateTime.Now.Month}/{dateParts[0]}";

						return str.TryParseToDateTime(CultureInfo.InvariantCulture);
					}

					if (dateParts[1].IsYearNumber())
					{
						str = dateParts[0].IsMonthNumber() 
							? $"{DateTime.Now.Day}/{dateParts[0]}/{dateParts[1]}" 
							: $"{dateParts[0]}/{DateTime.Now.Month}/{dateParts[1]}";

						return str.TryParseToDateTime(CultureInfo.InvariantCulture);
					}

					str = $"{dateParts[0]}/{dateParts[1]}/{DateTime.Now.Year}";
					return str.TryParseToDateTime(CultureInfo.InvariantCulture);
				}

				case 1:
				{
					var input = dateParts[0];

					if (input.IsDayNumber())
					{
						return $"{input}/{DateTime.Now.Month}/{DateTime.Now.Year}"
							.TryParseToDateTime();
					}

					if (input.IsMonthNumber())
					{
						return $"{DateTime.Now.Day}/{input}/{DateTime.Now.Year}"
							.TryParseToDateTime();
					}

					if (input.IsYearNumber())
					{
						return $"{DateTime.Now.Day}/{DateTime.Now.Month}/{input}"
							.TryParseToDateTime();
					}

					return DateTime.MinValue;
				}

				default:
					return DateTime.MinValue;
			}
		}
		public static bool IsDayNumber(this string value)
		{
			if (string.IsNullOrEmpty(value)) return false;

			return value
				.TryParseToInt()
				.IsBetween(DateTime.MinValue.Day, DateTime.MaxValue.Day);
		}
		public static bool IsMonthNumber(this string value)
		{
			if (string.IsNullOrEmpty(value)) return false;

			return value
				.TryParseToInt()
				.IsBetween(DateTime.MinValue.Month, DateTime.MaxValue.Month);
		}
		public static bool IsYearNumber(this string value)
		{
			if (string.IsNullOrEmpty(value)) return false;

			return value
				.TryParseToInt()
				.IsBetween(DateTime.MinValue.Year, DateTime.MaxValue.Year);
		}

		public static DateTime TryParseToDateTime(this string value)
		{
			DateTime.TryParse(value, out var result);
			return result;
		}

		public static DateTime TryParseToDateTime(this string value, IFormatProvider provider)
		{
			DateTime.TryParse(value, provider, DateTimeStyles.None, out var result);
			return result;
		}
		public static int TryParseToInt(this string value)
		{
			int.TryParse(value, out var result);
			return result;
		}

		public static bool TryParseToBool(this string value)
		{
			bool.TryParse(value, out var result);
			return result;
		}

		public static bool Contains(this string value, string toCheck, StringComparison comparison)
		{
			return value?.IndexOf(toCheck, comparison) >= 0;
		}

		public static string Truncate(this string value, int maxLength, bool dotsSuffix = true)
		{
			if (string.IsNullOrEmpty(value)) return value;
			if (maxLength <= 0) return value;
			if (value.Length <= maxLength) return value;

			var result = value.Substring(0, maxLength).Trim();

			if (dotsSuffix)
			{
				const string dots = "...";
				var dotsLength = dots.Length;
				var resultLength = result.Length;

				if (resultLength > dotsLength)
					result = result.Substring(0, (resultLength - dotsLength)) + dots;
			}

			return result;
		}

		public static string ReplaceLastOccurrence(this string source, string oldString, string newString)
		{
			if (string.IsNullOrEmpty(source)) return source;

			var index = source.LastIndexOf(oldString, StringComparison.InvariantCultureIgnoreCase);
			if (index < 0) return source;

			var result = source.Remove(index, oldString.Length)
			                   .Insert(index, newString);

			return result;

		}

		public static string ReplaceLastOccurrence(this string source, char oldChar, string newString)
		{
			return source.ReplaceLastOccurrence(oldChar.ToString(), newString);
		}
	}
}
