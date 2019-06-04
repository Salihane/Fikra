using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.API.Helpers
{
    public static class ResourceMessages
    {
	    public const string InvalidResourceFields = "Resource does not have the requested field(s)";
	    public const string ResourceNotFound = "Resource not found";
	    public static string ResourceWithIdNotFound = "{0} with id {1} not found";
	    public const string PostedResourceIsNull = "Posted resource is null";
	    public const string PostedResourceExistsAlready = "Posted resource exists already";
    }
}
