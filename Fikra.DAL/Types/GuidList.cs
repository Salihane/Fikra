using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.DAL.Extensions;
using Fikra.DAL.Helpers;
using Fikra.DAL.Types.Interfaces;

namespace Fikra.DAL.Types
{
	public class GuidList : ITableValued
	{
		public string Name => DbUtility.GetDbObjectName<GuidList>();
		public DataTable DataTable { get; }
		public ICollection<DataColumn> Columns { get; }

		public string Body => $@"{Constants.CreateType}{Name}{Constants.AsTable}
								(
									{GetColumnsDescription()}
								)";

		public GuidList()
		{
			Columns = new List<DataColumn>
			{
				new DataColumn
				{
					Name = "GUID",
					Specification = $"{DbType.Guid.Translate()} " +
									$"{Constants.NotNull}"
				}
			};

			DataTable = new DataTable();
			foreach (var dataColumn in Columns)
			{
				DataTable.Columns.Add(dataColumn.Name);
			}
		}

		private string GetColumnsDescription()
		{
			var builder = new StringBuilder();
			foreach (var dataColumn in Columns)
			{
				builder.Append($"{dataColumn.Name} {dataColumn.Specification}{Chars.Comma}{Environment.NewLine}");
			}

			return builder.ToString().ReplaceLastOccurrence(Chars.Comma, string.Empty);
		}
	}
}
