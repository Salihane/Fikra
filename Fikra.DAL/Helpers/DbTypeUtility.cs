using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.Helpers
{
	public static class DbTypeUtility
	{
		public static string Translate(DbType dbType)
		{
			switch (dbType)
			{
				case DbType.Guid: return "uniqueidentifier";
				default: return string.Empty;
			}
		}
	}
}
