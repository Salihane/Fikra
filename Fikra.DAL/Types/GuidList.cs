using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.Types
{
    public class GuidList : ITableValued
    {
		public string Name => $"[dbo].[{nameof(GuidList)}]";
		public string Body => $@"CREATE TYPE {Name}
								AS TABLE
								(
									GUID uniqueidentifier
								)";
    }
}
