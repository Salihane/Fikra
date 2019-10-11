using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.StoredProcedures
{
    public interface IStoredProcedure
    {
	    string Name { get; }
	    string Body { get; }
		string SqlQuery { get; }
		ICollection<SqlParameter> SqlParameters { get; set; }
    }
}
