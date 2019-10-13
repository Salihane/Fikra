using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL.Interfaces;
using Fikra.DAL.StoredProcedures;
using Fikra.DAL.StoredProcedures.Interfaces;
using Fikra.DAL.Types;
using Fikra.DAL.Types.Interfaces;
using Fikra.DAL.Views.Interfaces;
using Remotion.Linq.Clauses;

namespace Fikra.DAL.Helpers
{
    public static class DbUtility
    {
	    public static string GetDbObjectName<T>() where T : IDbObject
	    {
		    return $"{Constants.DbSchema}.[{typeof(T).Name}]";
		}
	}
}
