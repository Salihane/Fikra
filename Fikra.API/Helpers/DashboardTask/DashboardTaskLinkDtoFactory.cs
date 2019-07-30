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
	public class DashboardTaskLinkDtoFactory : ILinkDtoFactory<Model.Entities.DashboardTask, Guid>
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

			return new LinkDto(href, paginationPage, ActionMethods.Get);
		}

		public IEnumerable<LinkDto> CreateNavigationLinksForEntity(bool hasNext, bool hasPrevious, object linkValues)
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
				ActionNames.Tasks.CreateDashboardTask,
				LinkRelations.Task.CreateTask,
				ActionMethods.Post,
				linkValues);

			links.Add(postLink);

			return links;
		}

		public IEnumerable<LinkDto> CreateCrudLinksForEntity(Guid entityId, string entityFields = null)
		{
			dynamic linkValues = new ExpandoObject();
			linkValues.Id = entityId;

			if (!string.IsNullOrEmpty(entityFields))
				linkValues.Fields = entityFields;

			var getTaskLink = LinkDtoBuilder.CreateLink(_urlHelper, ActionNames.Tasks.GetDashboardTaskById,
LinkRelations.Task.GetTask, ActionMethods.Get, new { id = entityId.ToString() });

			var deleteTaskLink = LinkDtoBuilder.CreateLink(_urlHelper, ActionNames.Tasks.DeleteTask,
				LinkRelations.Task.DeleteTask, ActionMethods.Delete, new { id = entityId });

			var putTaskLink = LinkDtoBuilder.CreateLink(_urlHelper, ActionNames.Tasks.UpdateTask,
				LinkRelations.Task.UpdateTask, ActionMethods.Put, new { id = entityId });

			var patchTaskLink = LinkDtoBuilder.CreateLink(_urlHelper, ActionNames.Tasks.PatchTask,
				LinkRelations.Task.PathTask, ActionMethods.Patch, new { id = entityId });

			linkValues = new ExpandoObject();
			linkValues.TaskId = entityId;
			var taskCommentsLink = LinkDtoBuilder.CreateLink(_urlHelper, ActionNames.Comments.GetTaskComments,
				LinkRelations.Task.GetComments, ActionMethods.Get, new { id = entityId });

			return new List<LinkDto>
			{
				getTaskLink,
				deleteTaskLink,
				putTaskLink,
				patchTaskLink,
				taskCommentsLink
			};
		}
	}
}
