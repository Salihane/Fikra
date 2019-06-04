﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Helpers.ResourceResponse;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models;
using Fikra.API.Models.DashboardTask;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Serialization;
using Task = System.Threading.Tasks.Task;

namespace Fikra.API.Services.Tasks
{
	public class TaskService : ITaskService
	{
		private readonly IRepository<Dashboard, int> _dashboardRepo;
		private readonly IRepository<DashboardTask, Guid> _dashboardTasksRepo;
		private readonly IResourceParameters<DashboardTask, Guid> _dashboardTaskParameters;
		private readonly IFikraMapper<DashboardTask, DashboardTaskDto> _dashboardTaskMapper;
		private readonly ILinkDtoFactory<DashboardTask, Guid> _dashboardTaskLinkDtoFactory;
		private readonly IResourceUri<DashboardTask, Guid> _dashboardTaskUri;
		private readonly IResourceResultBuilder _resourceResultBuilder;
		private readonly IUrlHelper _urlHelper;
		private readonly IMapper _mapper;

		public TaskService(
			IRepository<Dashboard, int> dashboardRepo,
			IRepository<DashboardTask, Guid> dashboardTasksRepo,
			IResourceParameters<DashboardTask, Guid> dashboardTaskParameters,
			IResourceUri<DashboardTask, Guid> dashboardTaskUri,
			IMapper mapper, IUrlHelper urlHelper,
			IFikraMapper<DashboardTask, DashboardTaskDto> dashboardTaskMapper,
			ILinkDtoFactory<DashboardTask, Guid> dashboardTaskLinkDtoFactory,
			IResourceResultBuilder resourceResultBuilder)
		{
			_dashboardTasksRepo = dashboardTasksRepo;
			_dashboardTaskParameters = dashboardTaskParameters;
			_mapper = mapper;
			_urlHelper = urlHelper;
			_dashboardTaskMapper = dashboardTaskMapper;
			_dashboardTaskLinkDtoFactory = dashboardTaskLinkDtoFactory;
			_resourceResultBuilder = resourceResultBuilder;
			_dashboardRepo = dashboardRepo;
			_dashboardTaskUri = dashboardTaskUri;
		}

		public async Task<ResourceResult> GetTasksByDashboardIdAsync(
			int dashboardId,
			DashboardTaskResourceParametersDto resourceParametersDto,
			string mediaType)
		{
			_mapper.Map(resourceParametersDto, _dashboardTaskParameters);

			ICollection<string> fields = null;
			var isRequestForSpeceficFields = (_dashboardTaskParameters?.Fields.Any()).ToBool();

			if (isRequestForSpeceficFields)
			{
				var fieldsDoNotExist = !typeof(DashboardTaskDto)
					.HasProperties(_dashboardTaskParameters?.Fields?.ToArray());

				if (fieldsDoNotExist)
				{
					return _resourceResultBuilder
						   .WithStatusCode(StatusCodes.Status400BadRequest)
						   .WithMessage(ResourceMessages.InvalidResourceFields)
						   .Build();
				}

				fields = _dashboardTaskParameters?.Fields;
			}

			var taskEntities = await _dashboardTasksRepo
				.SearchForAsync(x => x.DashboardId == dashboardId, _dashboardTaskParameters);

			var noTasksFound = taskEntities == null || taskEntities.Count == 0;
			if (noTasksFound)
			{
				return _resourceResultBuilder
					   .WithStatusCode(StatusCodes.Status404NotFound)
					   .WithMessage(ResourceMessages.ResourceNotFound)
					   .Build();
			}

			if (mediaType == CustomMediaTypes.HateoasJson)
			{
				return await
					BuildHateoasResourceResultAsync(taskEntities, dashboardId, fields);
			}

			return await
				BuildNonHateoasResourceResultAsync(taskEntities, fields);
		}
		public async Task<ResourceResult> CreateTaskAsync(
			int dashboardId,
			DashboardTaskDtoCreate dashboardTaskDtoCreate,
			string mediaType,
			ModelStateDictionary modelState)
		{
			if (dashboardTaskDtoCreate == null)
			{
				return _resourceResultBuilder
					   .WithStatusCode(StatusCodes.Status400BadRequest)
					   .WithMessage(ResourceMessages.PostedResourceIsNull)
					   .Build();
			}

			var invalidModelState = CheckModelStateValidation(modelState);
			if (invalidModelState != null)
			{
				return invalidModelState;
			}

			var dashboardExistsResult = await CheckDashboardInexistenceAsync(dashboardId);
			var dashboardDoseNotExist = dashboardExistsResult != null;
			if (dashboardDoseNotExist)
			{
				return dashboardExistsResult;
			}

			var taskExistsAlreadyResult = await CheckTaskExistenceAsync(dashboardId, dashboardTaskDtoCreate);
			if (taskExistsAlreadyResult != null)
			{
				return taskExistsAlreadyResult;
			}





			// Dashboard exists => Ok
			// New task does not exist yet in the db => Ok
			var taskEntity = _mapper.Map<DashboardTask>(dashboardTaskDtoCreate);
			// todo: go further (check brainy solution)


			return new ResourceResult();
		}

		private ResourceResult CheckModelStateValidation(ModelStateDictionary modelState)
		{
			if (!modelState.IsValid)
			{
				return _resourceResultBuilder
					   .WithStatusCode(StatusCodes.Status422UnprocessableEntity)
					   .WithResultObject(new Helpers.UnprocessableEntityObjectResult(modelState))
					   .Build();
			}

			return null;
		}

		private async Task<ResourceResult> CheckDashboardInexistenceAsync(int dashboardId)
		{
			var dashboardEntity = await GetDashboardByIdAsync(dashboardId);
			var dashboardDoesNotExist = dashboardEntity == null;

			if (dashboardDoesNotExist)
			{
				return _resourceResultBuilder
					   .WithStatusCode(StatusCodes.Status404NotFound)
					   .WithMessage(string.Format(ResourceMessages.ResourceWithIdNotFound,
												  nameof(Dashboard),
												  dashboardId))
					   .Build();
			}

			return null;
		}

		private async Task<ResourceResult> CheckTaskExistenceAsync(int dashboardId, DashboardTaskDtoCreate dashboardTaskDtoCreate)
		{
			_dashboardTaskParameters.SearchQuery = dashboardTaskDtoCreate.Name;
			_dashboardTaskParameters.Fields = new List<string>
			{
				nameof(DashboardTask.Name),
				nameof(DashboardTask.Id)

			};

			var tasksWithSameNewName = await GetDashboardTasksByNameAsync(dashboardId, dashboardTaskDtoCreate.Name);
			if (tasksWithSameNewName.ResultObject == null) return null;

			var resource = tasksWithSameNewName.ResultObject as LinkedResource;
			var taskExistsAlready = (resource?.Value.Any()).ToBool();

			return taskExistsAlready ? BuildResultForExistingTask(resource) : null;
		}

		private ResourceResult BuildResultForExistingTask(LinkedResource resource)
		{
			var existingTask = resource?.Value.First();

			var linkedResource = new LinkedResource
			{
				Links = existingTask[ResourceKeys.Links] as IEnumerable<LinkDto>,
				Value = new IDictionary<string, object>[]
				{
					new Dictionary<string, object>
					{
						{ResourceKeys.Id, existingTask[ResourceKeys.Id]},
						{ResourceKeys.Name, existingTask[ResourceKeys.Name]}
					}
				}
			};

			return _resourceResultBuilder
				   .WithStatusCode(StatusCodes.Status409Conflict)
				   .WithMessage(ResourceMessages.PostedResourceExistsAlready)
				   .WithResultObject(linkedResource)
				   .Build();
		}

		private async Task<ResourceResult> GetDashboardTasksByNameAsync(int dashboardId, string dashboardTaskName)
		{
			Func<DashboardTask, bool> taskNameEquals = x =>
				x.Name.Equals(dashboardTaskName, StringComparison.CurrentCultureIgnoreCase);
			Expression<Func<DashboardTask, bool>> predicate = x => taskNameEquals(x);

			_dashboardTaskParameters.ResourceFilter = new ResourceFilter<DashboardTask, Guid>
			{
				Expression = predicate
			};
			var dashboardTaskParameters = _mapper.Map<DashboardTaskResourceParametersDto>(_dashboardTaskParameters);

			return await GetTasksByDashboardIdAsync(dashboardId,
				dashboardTaskParameters, CustomMediaTypes.HateoasJson);
		}

		private async Task<Dashboard> GetDashboardByIdAsync(int dashboardId, bool includeTasks = false)
		{
			IQueryable<Dashboard> queryResult;
			if (includeTasks)
			{
				queryResult = await _dashboardRepo
				.SearchForAsync(x => x.Id == dashboardId, x => x.Tasks);
			}
			else
			{
				queryResult = await _dashboardRepo
					.SearchForAsync(x => x.Id == dashboardId);
			}


			return queryResult.AsQueryable().FirstOrDefault();
		}

		private async Task<ResourceResult> BuildHateoasResourceResultAsync(
			PagedList<DashboardTask> taskEntities,
			int dashboardId, ICollection<string> fields)
		{
			// todo: use named type "PaginationMetaData" instead of anonymous type
			var paginationMetaData = new
			{
				totalItems = taskEntities.TotalItems,
				pageSize = taskEntities.PageSize,
				currentPage = taskEntities.CurrentPage,
				totalPages = taskEntities.TotalPages
			};

			_resourceResultBuilder
				.WithResponseHeader(HttpHeader.Pagination,
									Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

			var dashboardTaskDtos = await _dashboardTaskMapper.ToViewModelAsync(taskEntities);

			var tasksLinks = _dashboardTaskLinkDtoFactory.CreateNavigationLinksForEntity(
				taskEntities.HasNext, taskEntities.HasPrevious, new { dashboardId });

			// The Id is needed to generate the links
			var isIdNotInFields = fields.AddIfNotIn(ResourceKeys.Id);
			var shapedTasks = dashboardTaskDtos.ShapeData(fields.ToStringArray());
			var shapedTasksWithLinks = shapedTasks.Select(task =>
			{
				var taskAsDictionary = task as IDictionary<string, object>;
				var taskId = (Guid)taskAsDictionary[ResourceKeys.Id];
				var taskFields = fields.ToCommaSeparatedString();
				var taskLinks = _dashboardTaskLinkDtoFactory
					.CreateCrudLinksForEntity(taskId, taskFields);

				// Remove the Id if it was not requested (was added to fields only to generate the links)
				if (isIdNotInFields)
					taskAsDictionary.Remove(ResourceKeys.Id);

				taskAsDictionary.Add(nameof(LinkedResource.Links), taskLinks);

				return taskAsDictionary;
			});

			var returnedLinkedResource = new LinkedResource
			{
				Value = shapedTasksWithLinks,
				Links = tasksLinks
			};

			return _resourceResultBuilder
				   .WithStatusCode(StatusCodes.Status200OK)
				   .WithResultObject(returnedLinkedResource)
				   .Build();
		}

		private async Task<ResourceResult> BuildNonHateoasResourceResultAsync(
			PagedList<DashboardTask> taskEntities,
			ICollection<string> taskFields)
		{
			var paginationMetaData = BuildPaginationMetaData(taskEntities);
			var dashboardTaskDtos = await _dashboardTaskMapper.ToViewModelAsync(taskEntities);

			return _resourceResultBuilder
			       .WithStatusCode(StatusCodes.Status200OK)
			       .WithResultObject(dashboardTaskDtos.ShapeData(taskFields.ToStringArray()))
			       .WithResponseHeader(HttpHeader.Pagination,
			                           Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData))
			       .Build();
		}

		private object BuildPaginationMetaData(PagedList<DashboardTask> taskEntities)
		{
			var nextPageLink = taskEntities.HasNext
				? _dashboardTaskUri.CreateResourceUri(_dashboardTaskParameters, ResourceUriType.Next,
					_urlHelper, ActionNames.Tasks.GetDashboardTasks)
				: null;

			var previousPageLink = taskEntities.HasPrevious
				? _dashboardTaskUri.CreateResourceUri(_dashboardTaskParameters, ResourceUriType.Previous,
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

			return paginationMetaData;
		}
	}
}
