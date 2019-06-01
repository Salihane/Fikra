using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Common.Constants;
using Fikra.Common.Helpers;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.API.Helpers.DashboardTask
{
	public class DashboardTaskLinkDtoFactory : ILinkDtoFactory<Model.Entities.DashboardTask>
	{
		private readonly IResourceParameters<Model.Entities.DashboardTask, Guid> _dashboardTaskParameters;
		private readonly IResourceUri<Model.Entities.DashboardTask, Guid> _dashboardTaskUri;
		private readonly IUrlHelper _urlHelper;

		public DashboardTaskLinkDtoFactory(
			IResourceParameters<Model.Entities.DashboardTask, Guid> dashboardTaskParameters,
			IResourceUri<Model.Entities.DashboardTask, Guid> dashboardTaskUri,
			IUrlHelper urlHelper)
		{
			_dashboardTaskParameters = dashboardTaskParameters;
			_dashboardTaskUri = dashboardTaskUri;
			_urlHelper = urlHelper;
		}

		public LinkDto CreateGetEntityLink(ResourceUriType resourceUriType)
		{
			var paginationPage = PaginationPages.CurrentPage;
			switch (resourceUriType)
			{
				case ResourceUriType.Next:
					paginationPage = PaginationPages.NextPage;
					break;
				case ResourceUriType.Previous:
					paginationPage = PaginationPages.PreviousPage;
					break;
			}

			var href = _dashboardTaskUri.CreateResourceUri(
				_dashboardTaskParameters, resourceUriType,
				_urlHelper, ActionNames.Tasks.GetDashboardTasks);

			return new LinkDto( href, paginationPage, ActionMethods.Get);
		}

		public IEnumerable<LinkDto> CreateNavigationLinksForEntity(bool hasNext, bool hasPrevious, params object[] linkValues)
		{
			var links = new List<LinkDto>
			{
				CreateGetEntityLink(ResourceUriType.Current)
			};

			if (hasNext)
				links.Add(CreateGetEntityLink(ResourceUriType.Next));
			if (hasPrevious)
				links.Add(CreateGetEntityLink(ResourceUriType.Previous));

			var postLink = LinkDtoBuilder.CreateLink(
				_urlHelper, 
				LinkRelations.Task.CreateDashboardTask,  
				ActionMethods.Get,
				new { dashboardId = linkValues.Single()});

			links.Add(postLink);

			return links;
		}
	}
}
