using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.Types
{
	public interface ITableValued
	{
		string Name { get; }
		string Body { get; }
		DataTable DataTable { get; }
		ICollection<DataColumn> Columns { get; }
	}
}
