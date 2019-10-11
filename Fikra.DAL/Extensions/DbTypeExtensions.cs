using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.Extensions
{
    public static class DbTypeExtensions
    {
	    public static string Translate(this DbType dbType)
	    {
		    switch (dbType)
		    {
				case DbType.Guid: return Constants.DbTypeConstants.UniqueIdentifier;
				default: return string.Empty;
		    }
	    }
        
    }
}
