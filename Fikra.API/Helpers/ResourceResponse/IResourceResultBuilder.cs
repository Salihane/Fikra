using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Fikra.API.Helpers.ResourceResponse
{
	public interface IResourceResultBuilder
	{
		ResourceResult Build();
		IResourceResultBuilder WithMessage(string message);
		IResourceResultBuilder WithResultObject(object resultObj);
		IResourceResultBuilder WithResponseHeader(string key, StringValues values);
		IResourceResultBuilder WithStatusCode(int statusCode);

	}
}
