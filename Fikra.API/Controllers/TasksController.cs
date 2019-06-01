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
using Fikra.API.Services;
using Fikra.API.Services.Tasks;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;
using Fikra.DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Entities = Fikra.Model.Entities;

namespace Fikra.API.Controllers
{
	[ApiController]
	public class TasksController : ControllerBase
	{
		private readonly ITaskService _dashboardTaskService;
		public TasksController(ITaskService dashboardTaskService)
		{
			_dashboardTaskService = dashboardTaskService;
		}

		// , Route = RouteNames.Tasks.TasksByDashboard
		[AcceptVerbs(ActionMethods.Get, Name = ActionNames.Tasks.GetDashboardTasks, Route = RouteNames.Tasks.TasksByDashboard)]
		[ProducesResponseType(typeof(IEnumerable<DashboardTaskDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> Get(int dashboardId,
			[FromQuery]DashboardTaskResourceParametersDto resourceParameters,
			[FromHeader(Name = HttpHeader.Accept)]string mediaType)
		{
			var result = _dashboardTaskService.GetTasksByDashboardId(dashboardId, resourceParameters, mediaType);
			AddResponseHeaders(result.ResponseHeaders);

			switch (result.StatusCode)
			{
				case StatusCodes.Status200OK:
					return Ok(result.ResultObject);
				case StatusCodes.Status404NotFound:
					return NotFound(result.Message);
				case StatusCodes.Status400BadRequest:
					return BadRequest(result.Message);
				default:
					return NoContent();
			}
		}

		private void AddResponseHeaders(Dictionary<string, StringValues> headers)
		{
			if (!headers.Any()) return;

			foreach (var (key, value) in headers)
			{
				Response.Headers.Add(key, value);
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