using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fikra.API.Extensions;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Models;
using Fikra.API.Models.DashboardTask;
using Fikra.API.Services.Tasks;
using Fikra.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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

		[AcceptVerbs(ActionMethods.Get,
			Name = ActionNames.Tasks.GetDashboardTasks,
			Route = RouteNames.Task.TasksByDashboard)]
		[ProducesResponseType(typeof(IEnumerable<DashboardTaskDto>),
			StatusCodes.Status200OK)]
		public async Task<IActionResult> GetDashboardTasks(int dashboardId,
			[FromQuery]DashboardTaskResourceParametersDto resourceParameters,
			[FromHeader(Name = HttpHeader.Accept)]string mediaType)
		{
			var result = await _dashboardTaskService
				.GetTasksByDashboardIdAsync(dashboardId, resourceParameters, mediaType);

			Response.AddHeaders(result.ResponseHeaders);

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
		[AcceptVerbs(ActionMethods.Get,
			Name = ActionNames.Tasks.GetTaskById,
			Route = RouteNames.Task.TaskById)]
		public async Task<IActionResult> GetTaskById(Guid id)
		{
			return NoContent();
		}
		[AcceptVerbs(ActionMethods.Post,
			Name = ActionNames.Tasks.CreateDashboardTask,
			Route = RouteNames.Task.TaskByDashboard)]
		public async Task<IActionResult> PostDashboardTask(
			int dashboardId,
			[FromBody]DashboardTaskDtoCreate dashboardTaskDtoCreate,
			[FromHeader(Name = HttpHeader.Accept)]string mediaType)
		{
			var result = await _dashboardTaskService
				.CreateTaskAsync(dashboardId, dashboardTaskDtoCreate, mediaType, ModelState);

			Response.AddHeaders(result.ResponseHeaders);

			switch (result.StatusCode)
			{
				case StatusCodes.Status400BadRequest:
					return BadRequest(result.Message);
				case StatusCodes.Status422UnprocessableEntity:
					return UnprocessableEntity(result.ResultObject);
				case StatusCodes.Status404NotFound:
					return NotFound(result.Message);
				case StatusCodes.Status409Conflict:
					Response.StatusCode = result.StatusCode;
					return new JsonResult(new
					{
						result.Message,
						result.ResultObject
					});
				default:
					return NoContent();
			}
		}

		[AcceptVerbs(ActionMethods.Delete,
			Name = ActionNames.Tasks.DeleteTask,
			Route = RouteNames.Task.TaskById)]
		public async Task<IActionResult> DeleteTask(Guid id)
		{
			return NoContent();
		}
		[AcceptVerbs(ActionMethods.Put,
			Name = ActionNames.Tasks.UpdateTask,
			Route = RouteNames.Task.TaskById)]
		public async Task<IActionResult> UpdateTask(Guid id)
		{
			return NoContent();
		}
		[AcceptVerbs(ActionMethods.Patch,
			Name = ActionNames.Tasks.PatchTask,
			Route = RouteNames.Task.TaskById)]
		public async Task<IActionResult> PatchTask(Guid id)
		{
			return NoContent();
		}
	}
}