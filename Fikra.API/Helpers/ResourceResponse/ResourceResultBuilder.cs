using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Fikra.API.Helpers.ResourceResponse
{
    public class ResourceResultBuilder : IResourceResultBuilder
    {
	    private readonly ResourceResult _resourceResult;

	    public ResourceResultBuilder()
	    {
		    _resourceResult = new ResourceResult
		    {
			    ResponseHeaders = new Dictionary<string, StringValues>()
		    };
	    }

	    public ResourceResult Build()
	    {
		    return _resourceResult;
	    }

	    public IResourceResultBuilder WithMessage(string message)
	    {
		    _resourceResult.Message = message;
		    return this;
	    }

	    public IResourceResultBuilder WithResultObject(object resultObj)
	    {
		    _resourceResult.ResultObject = resultObj;
		    return this;
	    }

	    public IResourceResultBuilder WithResponseHeader(string key, StringValues values)
	    {
		    _resourceResult.ResponseHeaders.Add(key, values);
		    return this;
	    }

	    public IResourceResultBuilder WithStatusCode(int statusCode)
	    {
		    _resourceResult.StatusCode = statusCode;
		    return this;
	    }
    }
}
