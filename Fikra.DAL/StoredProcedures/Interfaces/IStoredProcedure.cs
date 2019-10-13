using System.Collections.Generic;
using System.Data.SqlClient;
using Fikra.DAL.Interfaces;

namespace Fikra.DAL.StoredProcedures.Interfaces
{
    public interface IStoredProcedure : IDbObject
    {
	    string Name { get; }
	    string Body { get; }
		string SqlQuery { get; }
		ICollection<SqlParameter> SqlParameters { get; set; }
    }
}
