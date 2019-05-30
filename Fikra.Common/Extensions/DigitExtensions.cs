using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Common.Extensions
{
	public static class DigitExtensions
	{
		public static bool IsBetween(this int value, int left, int right)
		{
			return value >= left && value <= right;
		}
	}
}
