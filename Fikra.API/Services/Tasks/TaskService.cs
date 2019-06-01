using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace Fikra.API.Services.Tasks
{
	public class TaskService : IDataService
	{
		private readonly IRepository<DashboardTask, Guid> _dashboardTasksRepo;
		private readonly IResourceParameters<DashboardTask, Guid> _dashboardTaskParameters;
		private readonly IFikraMapper<DashboardTask, DashboardTaskDto> _dashboardTaskMapper;
		private readonly ILinkDtoFactory<Model.Entities.DashboardTask> _dashboardTaskLinkDtoFactory;
		private readonly IResourceUri<DashboardTask, Guid> _dashboardTaskUri;
		private readonly IUrlHelper _urlHelper;
		private readonly ILinkDtoFactoryOld _linkDtoFactoryOld;
		private readonly IMapper _mapper;

		public TaskService(
			IRepository<DashboardTask, Guid> dashboardTasksRepo,
			IResourceParameters<DashboardTask, Guid> dashboardTaskParameters,
			IResourceUri<DashboardTask, Guid> dashboardTaskUri,
			IMapper mapper, IUrlHelper urlHelper, ILinkDtoFactoryOld linkDtoFactoryOld,
			IFikraMapper<DashboardTask, DashboardTaskDto> dashboardTaskMapper,
			ILinkDtoFactory<DashboardTask> dashboardTaskLinkDtoFactory)
		{
			_dashboardTasksRepo = dashboardTasksRepo;
			_dashboardTaskParameters = dashboardTaskParameters;
			_mapper = mapper;
			_urlHelper = urlHelper;
			_linkDtoFactoryOld = linkDtoFactoryOld;
			_dashboardTaskMapper = dashboardTaskMapper;
			_dashboardTaskLinkDtoFactory = dashboardTaskLinkDtoFactory;
			_dashboardTaskUri = dashboardTaskUri;
		}

		public async Task<ResourceResult> GetTasksByDashboardId(
			int dashboardId,
			DashboardTaskResourceParametersDto resourceParameters,
			string mediaType)
		{
			// TODO: next is to test code below line by line and fix/refactor where needed
			_mapper.Map(resourceParameters, _dashboardTaskParameters);

			ICollection<string> fields = null;
			var isRequestForSpeceficFields = (_dashboardTaskParameters?.Fields.Any()).ToBool();

			if (isRequestForSpeceficFields)
			{
				// todo: test _dashboardTaskParameters = null
				var fieldsDoNotExist = !typeof(DashboardTaskDto).HasProperties(_dashboardTaskParameters?.Fields?.ToArray());
				if (fieldsDoNotExist)
				{
					return new ResourceResult
					{
						Message = ResourceMessages.InvalidResourceFields,
						ResultObject = null,
						StatusCode = StatusCodes.Status400BadRequest
					};
				}

				fields = _dashboardTaskParameters?.Fields;
			}

			var taskEntities = await _dashboardTasksRepo
				.SearchForAsync(x => x.DashboardId == dashboardId, _dashboardTaskParameters);

			var noTasksFound = taskEntities == null || taskEntities.Count == 0;
			if (noTasksFound)
			{
				return new ResourceResult
				{
					Message = ResourceMessages.ResourceNotFound,
					ResultObject = null,
					StatusCode = StatusCodes.Status404NotFound
				};
			}

			var resourceResult = new ResourceResult();

			if (mediaType == CustomMediaTypes.HateoasJson)
			{
				var paginationMetaData = new
				{
					totalItems = taskEntities.TotalItems,
					pageSize = taskEntities.PageSize,
					currentPage = taskEntities.CurrentPage,
					totalPages = taskEntities.TotalPages
				};

				resourceResult.ResponseHeaders.Add(HttpHeader.Pagination,
					Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

				var dashboardTaskDtos = await _dashboardTaskMapper.ToViewModelAsync(taskEntities);

				var tasksLinks = _dashboardTaskLinkDtoFactory.CreateNavigationLinksForEntity(
					taskEntities.HasNext, taskEntities.HasPrevious, new {dashboardId});

				// The Id is needed to generate the links
				var isIdNotInFields = fields.AddIfNotIn(PropertyKeys.Id);
				var shapedTasks = dashboardTaskDtos.ShapeData(fields.ToStringArray());
				var shapedTasksWithLinks = shapedTasks.Select(task =>
				{
					var taskAsDictionary = task as IDictionary<string, object>;
					var taskLinks = CreateLinksForDashboardTask((int)taskAsDictionary[PropertyKeys.Id], fields.ToCommaSeparatedString());


					// Remove the Id if it was not requested (was added to fields only to generate the links)
					if (isIdNotInFields)
						taskAsDictionary.Remove(PropertyKeys.Id);

					taskAsDictionary.Add(PropertyKeys.Links, taskLinks);

					return taskAsDictionary;
				});

				var returnedLinkedResource = new
				{
					Value = shapedTasksWithLinks,
					Links = tasksLinks
				};

				//return Ok(returnedLinkedResource);
				resourceResult.ResultObject = returnedLinkedResource;
				resourceResult.StatusCode = StatusCodes.Status200OK;
				resourceResult.Message = string.Empty;
				return resourceResult;
			}
			else
			{
				var nextPageLink = taskEntities.HasNext
					? _resourceUri.CreateResourceUri(_resourceParameters, ResourceUriType.Next,
						_urlHelper, ActionNames.Tasks.GetDashboardTasks)
					: null;

				var previousPageLink = taskEntities.HasPrevious
					? _resourceUri.CreateResourceUri(_resourceParameters, ResourceUriType.Previous,
						_urlHelper, ActionNames.Tasks.GetDashboardTasks)
					: null;

				var paginationMetaData = new
				{
					taskEntities.TotalItems,
					taskEntities.PageSize,
					taskEntities.CurrentPage,
					taskEntities.TotalPages,
					NextPageLink = nextPageLink,
					PreviousPageLink = previousPageLink
				};

				Response.Headers.Add(HttpHeader.Pagination, Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));
				var dashboardTaskDtos = await MapDashboardTaskDtosAsync(taskEntities);
				return Ok(dashboardTaskDtos.ShapeData(fields.ToStringArray()));
			}
		}
	}
}
