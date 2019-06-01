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
		public static LinkDto CreateLink(IUrlHelper urlHelper, string linkName, string actionMethod, params object[] linkParams)
		{
			var linkRelation = GetLinkRelation(actionMethod);
			return new LinkDto(urlHelper.Link(linkName, linkParams), linkRelation, actionMethod);
		}

		private static string GetLinkRelation(string actionMethod)
		{
			switch (actionMethod)
			{
				case ActionMethods.Get: return LinkRelations.Self;
				case ActionMethods.Post: return LinkRelations.Create;
				case ActionMethods.Delete: return LinkRelations.Delete;
				case ActionMethods.Put: return LinkRelations.Update;
				case ActionMethods.Patch: return LinkRelations.Patch;
			}

			return string.Empty;
		}
	}
}
