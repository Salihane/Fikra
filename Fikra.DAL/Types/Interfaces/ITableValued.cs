using System.Collections.Generic;
using System.Data;
using Fikra.DAL.Interfaces;

namespace Fikra.DAL.Types.Interfaces
{
	public interface ITableValued : IDbObject
	{
		string Name { get; }
		string Body { get; }
		DataTable DataTable { get; }
		ICollection<DataColumn> Columns { get; }
	}
}
