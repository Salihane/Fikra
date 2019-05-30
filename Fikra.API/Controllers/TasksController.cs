using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Models;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;
using Fikra.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Entities = Fikra.Model.Entities;

namespace Fikra.API.Controllers
{
	[ApiController]
	public class TasksController : ControllerBase
	{
		private readonly IRepository<Entities.DashboardTask, Guid> _dashboardTasksRepo;
		private readonly IRepository<Entities.Dashboard, int> _dashboardsRepo;
		private readonly IMapper _mapper;
		private IResourceParameters<Entities.DashboardTask, Guid> _resourceParameters;
		private IResourceUri<Entities.DashboardTask, Guid> _resourceUri;
		private IUrlHelper _urlHelper;
		private ILinkDtoFactory _linkDtoFactory;

		public TasksController(IRepository<Entities.DashboardTask, Guid> dashboardTasksRepo,
			IRepository<Entities.Dashboard, int> dashboardsRepo,
			IResourceParameters<Entities.DashboardTask, Guid> resourceParameters,
			IResourceUri<Entities.DashboardTask, Guid> resourceUri,
			IUrlHelper urlHelper,
			ILinkDtoFactory linkDtoFactory,
			IMapper mapper)
		{
			_dashboardTasksRepo = dashboardTasksRepo;
			_dashboardsRepo = dashboardsRepo;
			_resourceParameters = resourceParameters;
			_resourceUri = resourceUri;
			_urlHelper = urlHelper;
			_linkDtoFactory = linkDtoFactory;
			_mapper = mapper;
		}

		[AcceptVerbs(
		ActionMethods.Get,
		Name = ActionNames.Tasks.GetDashboardTasks,
		Route = RouteNames.Tasks.TasksByDashboard)]
		public async Task<IActionResult> Get(int dashboardId,
			[FromQuery]DashboardTaskResourceParametersDto resourceParameters,
			[FromHeader(Name = HttpHeader.Accept)]string mediaType)
		{

			_mapper.Map(resourceParameters, _resourceParameters);
			ICollection<string> fields = null;
			var isRequestForSpeceficFields = (_resourceParameters?.Fields.Any()).ToBool();

			if (isRequestForSpeceficFields)
			{
				// todo: test _resourceParameters = null
				var fieldsDoNotExist = !typeof(DashboardTaskDto).HasProperties(_resourceParameters?.Fields?.ToArray());
				if (fieldsDoNotExist) return BadRequest();

				fields = _resourceParameters?.Fields;
			}

			var taskEntities = await _dashboardTasksRepo
				.SearchForAsync(x => x.DashboardId == dashboardId, _resourceParameters);

			var noTasksFound = taskEntities == null || taskEntities.Count == 0;
			if (noTasksFound) return NotFound();

			if (mediaType == CustomMediaTypes.HateoasJson)
			{
				var paginationMetaData = new
				{
					totalItems = taskEntities.TotalItems,
					pageSize = taskEntities.PageSize,
					currentPage = taskEntities.CurrentPage,
					totalPages = taskEntities.TotalPages
				};

				Response.Headers.Add(HttpHeader.Pagination,
					Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));

				var dashboardTaskDtos = await MapDashboardTaskDtosAsync(taskEntities);
				var tasksLinks =
					CreateLinksForDashboardTasks(taskEntities.HasNext, taskEntities.HasPrevious, dashboardId);

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

				return Ok(returnedLinkedResource);
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

		private IEnumerable<LinkDto> CreateLinksForDashboardTask(int taskId, string taskFields)
		{
			dynamic linkValues = new ExpandoObject();
			linkValues.Id = taskId;

			if (!string.IsNullOrEmpty(taskFields))
				linkValues.Fields = taskFields;

			var getTaskLink = _linkDtoFactory.CreateLink(_urlHelper, ActionNames.Tasks.GetTaskById,
				linkValues, ActionMethods.Get);

			var deleteTaskLink = _linkDtoFactory.CreateLink(_urlHelper, ActionNames.Tasks.DeleteTask,
				linkValues, ActionMethods.Delete);

			var putTaskLink = _linkDtoFactory.CreateLink(_urlHelper, ActionNames.Tasks.UpdateTask,
				linkValues, ActionMethods.Put);

			var patchTaskLink = _linkDtoFactory.CreateLink(_urlHelper, ActionNames.Tasks.PatchTask,
				linkValues, ActionMethods.Patch);

			linkValues = new ExpandoObject();
			linkValues.TaskId = taskId;
			var taskCommentsLink = _linkDtoFactory.CreateLink(_urlHelper, ActionNames.Comments.GetDashboardTaskComments,
				linkValues, ActionMethods.Get);

			return new List<LinkDto>
			{
				getTaskLink,
				deleteTaskLink,
				putTaskLink,
				patchTaskLink,
				taskCommentsLink
			};
		}

		private IEnumerable<LinkDto> CreateLinksForDashboardTasks(bool hasNext, bool hasPrevious, int dashboardId)
		{
			var links = new List<LinkDto>();
			links.Add(GetLinkForDashboardTasks(ResourceUriType.Current));

			if (hasNext)
				links.Add(GetLinkForDashboardTasks(ResourceUriType.Next));
			if (hasPrevious)
				links.Add(GetLinkForDashboardTasks(ResourceUriType.Previous));

			links.Add(new LinkDto(
				_urlHelper.Link(ActionNames.Tasks.CreateDashboardTask, new { dashboardId = dashboardId }),
				LinkRelations.Task.CreateDashboardTask, ActionMethods.Post));

			return links;
		}

		private LinkDto GetLinkForDashboardTasks(ResourceUriType resourceUriType)
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

			return new LinkDto(
				_resourceUri.CreateResourceUri(_resourceParameters, resourceUriType, _urlHelper, ActionNames.Tasks.GetDashboardTasks),
				paginationPage,
				ActionMethods.Get);
		}

		private async Task<IEnumerable<DashboardTaskDto>> MapDashboardTaskDtosAsync(
			IReadOnlyCollection<Entities.DashboardTask> taskEntities)
		{
			if (taskEntities == null || taskEntities.Count == 0)
				return Enumerable.Empty<DashboardTaskDto>();

			var taskDtos = new List<DashboardTaskDto>();
			Entities.DashboardTask task;
			var taskChildNames = new[] { nameof(task.Comments) };
			foreach (var entity in taskEntities)
			{
				var childsCounts = await _dashboardTasksRepo.CountChildsAsync(entity, taskChildNames);
				var taskDto = _mapper.Map<DashboardTaskDto>(entity);
				taskDto.CommentsCount = childsCounts[nameof(task.Comments)];
				taskDtos.Add(taskDto);
			}

			return taskDtos;

		}

		//[HttpGet(Route("api/"))]
		//      public async Task<IEnumerable<Entities.Task>> GetTasksByDashboardAsync(int id)
		//      {
		//       var dashboard = await _dashboardsRepo.SearchForAsync(x => x.Id == id, y => y.Tasks);

		//       return null;
		//      }

		//[HttpPost]
		//public async Task<IActionResult> Post([FromBody]Entities.Task task)
		//{
		//    _dashboardTasksRepo.Add(task);
		//    await _dashboardTasksRepo.SaveChangesAsync();

		//    return Ok(task);
		//}

		//[HttpPut("{id}")]
		//public async Task<IActionResult> Put(Guid id, [FromBody]Entities.Task taskData)
		//{
		//    if (taskData.Id != id)
		//    {
		//        return BadRequest("Query id and entity id mismatch");
		//    }

		//    _dashboardTasksRepo.Update(taskData);
		//    await _dashboardTasksRepo.SaveChangesAsync();

		//    return Ok(taskData);
		//}
	}
}