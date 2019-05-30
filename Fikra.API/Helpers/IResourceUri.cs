using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Common.Helpers;
using Fikra.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.API.Helpers
{
	public interface IResourceUri<T, K> where T : IEntity<K> where K : IEquatable<K>
	{
		string CreateResourceUri(IResourceParameters<T, K> resourceParams,
			ResourceUriType uriType,
			IUrlHelper urlHelper,
			string urlName);

	}
}
