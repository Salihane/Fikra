using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.API.Helpers
{
    public static class LinkDtoBuilder
    {
		public static LinkDto CreateLink(IUrlHelper urlHelper, string routeName, string linkRelation, string actionMethod, object linkParams)
		{
			return new LinkDto(urlHelper.Link(routeName, linkParams), linkRelation, actionMethod);
		}
	}
}
