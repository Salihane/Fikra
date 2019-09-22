using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL.StoredProcedures;

namespace Fikra.DAL.Helpers
{
    public static class StoredProcedureUtility
    {
	    public static string GetParametersSignature(IStoredProcedure storedProcedure)
	    {
			if (storedProcedure?.SqlParameters?.Count == 0) return null;

			var signatureBuilder = new StringBuilder();
			foreach (var parameter in storedProcedure.SqlParameters)
			{
				signatureBuilder.Append($"{parameter.ParameterName}, ");
			}
			//var keys = storedProcedure.Parameters.Keys.ToList();
			//var signatureBuilder =new StringBuilder();
			//foreach (var key in keys)
			//{
			//	signatureBuilder.Append($"{key}, ");
			//}

			// Remove last ', '
			var signature = signatureBuilder.ToString();
			signature = signature.TrimEnd().Substring(0, signature.Length - 2);

			return signature;
	    }
    }
}
