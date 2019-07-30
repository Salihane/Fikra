using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Extensions;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Helpers.ResourceResponse;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models.DashboardTask;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using SBHLib.Extensions;
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

    public async Task<(IEnumerable<DashboardTaskDto> tasks, ResponseMetaData responseMetaData)> GetTasksByDashboardIdNewAsync(
      int dashboardId,
      DashboardTaskResourceParametersDto resourceParametersDto,
      string mediaType)
    {
      _mapper.Map(resourceParametersDto, _dashboardTaskParameters);

      var taskEntities = await _dashboardTasksRepo
        .SearchForAsync(x => x.DashboardId == dashboardId, _dashboardTaskParameters);


      var noTasksFound = taskEntities == null || taskEntities.Count == 0;
      if (noTasksFound)
      {
        var responseMetaData = new ResponseMetaData
        {
          ResponseCode = StatusCodes.Status404NotFound,
          ResponseMessage = ResourceMessages.ResourceNotFound
        };


        return (null, responseMetaData);
      }

      if (mediaType == CustomMediaTypes.HateoasJson)
      {
        return await BuildHateoasResourceResultNewAsync(taskEntities, dashboardId);
      }
      
      return await
        BuildNonHateoasResourceResultNewAsync(taskEntities);
    }

    private Dictionary<string, StringValues> GetPaginationMetaDataHeader(PagedList<DashboardTask> taskEntities)
    {
      var paginationMetaData = BuildPaginationMetaData(taskEntities);
      var responseHeaders = new Dictionary<string, StringValues>
      {
        {
          HttpHeader.Pagination,
          Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData)
        }
      };

      return responseHeaders;
    }

    private async Task<(IEnumerable<DashboardTaskDto>, ResponseMetaData)> BuildNonHateoasResourceResultNewAsync(
      PagedList<DashboardTask> taskEntities)
    {
      var dashboardTaskDtos = await _dashboardTaskMapper.ToViewModelAsync(taskEntities);
      var responseHeaders = GetPaginationMetaDataHeader(taskEntities);
      var responseMetaData = new ResponseMetaData
      {
        ResponseHeaders = responseHeaders,
        ResponseCode = StatusCodes.Status200OK
      };

      return (dashboardTaskDtos, responseMetaData);
    }

    private async Task<(IEnumerable<DashboardTaskDto>, ResponseMetaData)> BuildHateoasResourceResultNewAsync(
      PagedList<DashboardTask> taskEntities,
      int dashboardId)
    {
      var dashboardTaskDtos = await _dashboardTaskMapper.ToViewModelAsync(taskEntities);

      var dashboardTaskDtosWithLinks = dashboardTaskDtos as DashboardTaskDto[] ?? dashboardTaskDtos.ToArray();
      foreach (var taskDto in dashboardTaskDtosWithLinks)
      { 
        taskDto.Links = _dashboardTaskLinkDtoFactory
          .CreateCrudLinksForEntity(taskDto.Id);
      }

      var responseHeaders = GetPaginationMetaDataHeader(taskEntities);
      var responseMetaData = new ResponseMetaData
      {
        ResponseHeaders = responseHeaders,
        ResponseCode = StatusCodes.Status200OK
      };

      return (dashboardTaskDtosWithLinks, responseMetaData);
    }

    public async Task<ResourceResult> GetTasksByDashboardIdAsync(
      int dashboardId,
      DashboardTaskResourceParametersDto resourceParametersDto,
      string mediaType)
    {
      _mapper.Map(resourceParametersDto, _dashboardTaskParameters);

      ICollection<string> fields = null;
      //var isRequestForSpeceficFields = (_dashboardTaskParameters?.Fields.Any()).ToBool();

      //if (isRequestForSpeceficFields)
      //{
      //	var fieldsDoNotExist = !typeof(DashboardTaskDto)
      //	.HasProperties(_dashboardTaskParameters?.Fields?.ToArray());

      //	if (fieldsDoNotExist)
      //	{
      //		return _resourceResultBuilder
      //		.Clear()
      //		.WithStatusCode(StatusCodes.Status400BadRequest)
      //		.WithMessage(ResourceMessages.InvalidResourceFields)
      //		.Build();
      //	}

      //	fields = _dashboardTaskParameters?.Fields;
      //}

      // todo: search not working (url:....?search=task
      var taskEntities = await _dashboardTasksRepo
      .SearchForAsync(x => x.DashboardId == dashboardId, _dashboardTaskParameters);

      var noTasksFound = taskEntities == null || taskEntities.Count == 0;
      if (noTasksFound)
      {
        return _resourceResultBuilder
        .Clear()
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
        .Clear()
        .WithStatusCode(StatusCodes.Status400BadRequest)
        .WithMessage(ResourceMessages.PostedResourceIsNull)
        .Build();
      }

      var invalidModelState = CheckModelStateValidation(modelState);
      if (invalidModelState != null)
      {
        return invalidModelState;
      }

      var dashboardExistsResult = await CheckDashboardExistenceAsync(dashboardId);
      var dashboardDoseNotExist = dashboardExistsResult.StatusCode == StatusCodes.Status404NotFound;
      if (dashboardDoseNotExist)
      {
        return dashboardExistsResult;
      }

      var dashboardEntity = dashboardExistsResult.ResultObject as Dashboard;

      var taskExistsAlreadyResult = await CheckTaskExistenceAsync(dashboardId, dashboardTaskDtoCreate);
      if (taskExistsAlreadyResult != null)
      {
        return taskExistsAlreadyResult;
      }

      var taskEntity = _mapper.Map<DashboardTask>(dashboardTaskDtoCreate);
      taskEntity.Dashboard = dashboardEntity;
      taskEntity.JustCreated();
      _dashboardTasksRepo.Add(taskEntity);

      if (await _dashboardTasksRepo.SaveChangesAsync())
      {
        return CreatedAtAction(taskEntity, dashboardId, mediaType);
      }

      var errorMsg = $"Failed to save the new task '{dashboardTaskDtoCreate.Name}' for the dashboard '{dashboardEntity?.Name}'";
      return _resourceResultBuilder
           .Clear()
           .WithStatusCode(StatusCodes.Status500InternalServerError)
           .WithMessage(errorMsg)
           .Build();
    }


    #region private methods

    private ResourceResult CreatedAtAction(DashboardTask taskEntity, int dashboardId, string mediaType)
    {
      _resourceResultBuilder
        .Clear()
        .WithStatusCode(StatusCodes.Status201Created);

      var returnedDashboardTask = _mapper.Map<DashboardTaskDto>(taskEntity);
      if (mediaType != CustomMediaTypes.HateoasJson)
      {
        return _resourceResultBuilder
             .WithResultObject(new CreatedAtRouteResource
             {
               ActionName = ActionNames.Tasks.GetDashboardTaskById,
               RouteValues = new RouteValueDictionary(new
               {
                 dashboardId,
                 id = returnedDashboardTask.Id
               }),
               CreatedEntity = new RouteValueDictionary(returnedDashboardTask)
             })
             .Build();
      }

      var taskLinks = _dashboardTaskLinkDtoFactory
        .CreateCrudLinksForEntity(returnedDashboardTask.Id);

      var returnedLinkedResource = returnedDashboardTask
        .ShapeData(propertyNames: null) as IDictionary<string, object>;

      returnedLinkedResource.Add(ResourceKeys.Links, taskLinks);

      return _resourceResultBuilder
           .WithResultObject(new CreatedAtRouteResource
           {
             ActionName = ActionNames.Tasks.GetDashboardTaskById,
             RouteValues = new RouteValueDictionary(new
             {
               dashboardId,
               id = (Guid)returnedLinkedResource[ResourceKeys.Id]
             }),
             CreatedEntity = new RouteValueDictionary(returnedLinkedResource)
           })
           .Build();
    }

    private ResourceResult CheckModelStateValidation(ModelStateDictionary modelState)
    {
      if (!modelState.IsValid)
      {
        return _resourceResultBuilder
             .Clear()
             .WithStatusCode(StatusCodes.Status422UnprocessableEntity)
             .WithResultObject(new Helpers.UnprocessableEntityObjectResult(modelState))
             .Build();
      }

      return null;
    }

    private async Task<ResourceResult> CheckDashboardExistenceAsync(int dashboardId)
    {
      var dashboardEntity = await GetDashboardByIdAsync(dashboardId);
      var dashboardDoesNotExist = dashboardEntity == null;

      if (dashboardDoesNotExist)
      {
        return _resourceResultBuilder
             .Clear()
             .WithStatusCode(StatusCodes.Status404NotFound)
             .WithMessage(string.Format(ResourceMessages.ResourceWithIdNotFound,
                          nameof(Dashboard),
                          dashboardId))
             .Build();
      }

      return _resourceResultBuilder
           .Clear()
           .WithResultObject(dashboardEntity)
           .Build();
    }

    private async Task<ResourceResult> CheckTaskExistenceAsync(int dashboardId, DashboardTaskDtoCreate dashboardTaskDtoCreate)
    {
      //_dashboardTaskParameters.SearchQuery = dashboardTaskDtoCreate.Name;
      //_dashboardTaskParameters.Fields = new List<string>
      //{
      //	nameof(DashboardTask.Name),
      //	nameof(DashboardTask.Id)
      //};

      var tasksWithSameNewName = await GetDashboardTasksByNameAsync(dashboardId, dashboardTaskDtoCreate.Name);
      if (tasksWithSameNewName.ResultObject == null) return null;

      var resource = tasksWithSameNewName.ResultObject as LinkedResource;
      //var resourceHasTasks = (resource?.Value.Any()).ToBool();
      //if (resourceHasTasks)
      //{
      //	var taskWithSameName =
      //		resource?.Value
      //		        .FirstOrDefault(x => x.ContainsKey(ResourceKeys.Name) &&
      //		                             (string) x[ResourceKeys.Name] == dashboardTaskDtoCreate.Name) != null;
      //}
      var taskExistsAlready = (resource?.Value.Any()).ToBool();

      return taskExistsAlready ? BuildResultForExistingTask(resource) : null;
    }

    private ResourceResult BuildResultForExistingTask(LinkedResource resource)
    {
      var existingTask = resource?.Value.First();

      var linkedResource = default(LinkedResource);
      if (existingTask != null)
      {
        linkedResource = new LinkedResource
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
      }

      return _resourceResultBuilder
           .Clear()
           .WithStatusCode(StatusCodes.Status409Conflict)
           .WithMessage(ResourceMessages.PostedResourceExistsAlready)
           .WithResultObject(linkedResource)
           .Build();
    }

    private async Task<ResourceResult> GetDashboardTasksByNameAsync(int dashboardId, string dashboardTaskName)
    {

      if (string.IsNullOrEmpty(dashboardTaskName))
      {
        Func<DashboardTask, bool> taskNameEquals = x =>
        x.Name.Equals(dashboardTaskName, StringComparison.CurrentCultureIgnoreCase);
        Expression<Func<DashboardTask, bool>> predicate = x => taskNameEquals(x);

        //_dashboardTaskParameters.ResourceFilter = new ResourceFilter<DashboardTask, Guid>
        //{
        //	Expression = predicate
        //};
      }

      var dashboardTaskParameters = _mapper.Map<DashboardTaskResourceParametersDto>(_dashboardTaskParameters);

      return await GetTasksByDashboardIdAsync(dashboardId, dashboardTaskParameters, CustomMediaTypes.HateoasJson);
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
        totalItems = taskEntities.PaginationMetaData.PageItemsMetaData.TotalItems,
        pageSize = taskEntities.PaginationMetaData.PageItemsMetaData.PageSize,
        currentPage = taskEntities.PaginationMetaData.PageItemsMetaData.CurrentPage,
        totalPages = taskEntities.PaginationMetaData.PageItemsMetaData.TotalPages
      };

      _resourceResultBuilder
        .Clear()
        .WithResponseHeader(HttpHeader.Pagination,
                  Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

      var dashboardTaskDtos = await _dashboardTaskMapper.ToViewModelAsync(taskEntities);

      var tasksLinks = _dashboardTaskLinkDtoFactory
        .CreateNavigationLinksForEntity(taskEntities.PaginationMetaData.HasNext,
                        taskEntities.PaginationMetaData.HasPrevious,
                        new { dashboardId });

      // The Id is needed to generate the links
      var isIdNotInFields = fields.AddIfNotIn(ResourceKeys.Id);
      var shapedTasks = dashboardTaskDtos.ShapeData(fields.ToStringArray());
      var shapedTasksWithLinks = shapedTasks.Select(task =>
      {
        var taskAsDictionary = task as IDictionary<string, object>;
        var taskId = (Guid)taskAsDictionary[ResourceKeys.Id];
        var taskFields = fields.ToCsvString();
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
           .Clear()
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
           .Clear()
           .WithStatusCode(StatusCodes.Status200OK)
           .WithResultObject(dashboardTaskDtos.ShapeData(taskFields.ToStringArray()))
           .WithResponseHeader(HttpHeader.Pagination,
                     Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData))
           .Build();
    }

    private object BuildPaginationMetaData(PagedList<DashboardTask> taskEntities)
    {
      var nextPageLink = taskEntities.PaginationMetaData.HasNext
        ? _dashboardTaskUri.CreateResourceUri(_dashboardTaskParameters,
                            ResourceUriType.Next,
                            _urlHelper,
                            ActionNames.Tasks.GetDashboardTasks)
        : null;

      var previousPageLink = taskEntities.PaginationMetaData.HasPrevious
        ? _dashboardTaskUri.CreateResourceUri(_dashboardTaskParameters,
                            ResourceUriType.Previous,
                            _urlHelper,
                            ActionNames.Tasks.GetDashboardTasks)
        : null;

      var paginationMetaData = new
      {
        taskEntities.PaginationMetaData.PageItemsMetaData.TotalItems,
        taskEntities.PaginationMetaData.PageItemsMetaData.PageSize,
        taskEntities.PaginationMetaData.PageItemsMetaData.CurrentPage,
        taskEntities.PaginationMetaData.PageItemsMetaData.TotalPages,
        NextPageLink = nextPageLink,
        PreviousPageLink = previousPageLink
      };

      return paginationMetaData;
    }

    #endregion private methods

  }
}
