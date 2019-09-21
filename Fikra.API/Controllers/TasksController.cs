using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fikra.API.Extensions;
using Fikra.API.Filters;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Models.DashboardTask;
using Fikra.API.Services.Tasks;
using Fikra.Common.Constants;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.API.Controllers
{
	[ApiController]
	public class TasksController : ODataController
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
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ODataRoute]
		[EnableQuery()]
		[AddExpandQuery(nameof(DashboardTaskDto.Links))]
		public async Task<IActionResult> GetDashboardTasks(int dashboardId,
			[FromQuery]DashboardTaskResourceParametersDto resourceParameters,
			[FromHeader(Name = HttpHeader.Accept)]string mediaType)
		{
			var (tasks, responseMetaData) = await _dashboardTaskService
		.GetDashboardTasksAsync(dashboardId,
									   resourceParameters,
									   mediaType);

			var responseCode = responseMetaData?.ResponseCode ?? -1;
			Response.StatusCode = responseCode;
			Response.AddHeaders(responseMetaData?.ResponseHeaders);

			var responseMessage = responseMetaData?.ResponseMessage ?? string.Empty;

			switch (responseCode)
			{
				case StatusCodes.Status200OK:
					return Ok(tasks);
				case StatusCodes.Status404NotFound:
					return NotFound(responseMessage);
				case StatusCodes.Status400BadRequest:
					return BadRequest(responseMessage);
				default:
					return NoContent();
			}
		}

		[AcceptVerbs(ActionMethods.Get,
			Name = ActionNames.Tasks.GetDashboardTaskById,
			Route = RouteNames.Task.TaskById)]
		[ODataRoute]
		[EnableQuery()]
		[AddExpandQuery(nameof(DashboardTaskDto.Links))]
		public async Task<IActionResult> GetTaskById(Guid id,
												 [FromQuery]DashboardTaskResourceParametersDto resourceParameters,
												 [FromHeader(Name = HttpHeader.Accept)]string mediaType)
		{
			var (task, responseMetaData) = await _dashboardTaskService
			  .GetDashboardTaskByIdAsync(id, resourceParameters, mediaType);

			var responseCode = responseMetaData?.ResponseCode ?? -1;
			Response.StatusCode = responseCode;
			Response.AddHeaders(responseMetaData?.ResponseHeaders);

			var responseMessage = responseMetaData?.ResponseMessage ?? string.Empty;


			switch (responseCode)
			{
				case StatusCodes.Status200OK:
					return Ok(task);
				case StatusCodes.Status404NotFound:
					return NotFound(responseMessage);
				case StatusCodes.Status400BadRequest:
					return BadRequest(responseMessage);
				default:
					return NoContent();
			}

			//var responseCode = responseMetaData?.ResponseCode ?? -1;
			//Response.StatusCode = responseCode;
			//Response.AddHeaders(responseMetaData?.ResponseHeaders);

			//var responseMessage = responseMetaData?.ResponseMessage ?? string.Empty;

			//switch (responseCode)
			//{
			//  case StatusCodes.Status200OK:
			//    return Ok(tasks);
			//  case StatusCodes.Status404NotFound:
			//    return NotFound(responseMessage);
			//  case StatusCodes.Status400BadRequest:
			//    return BadRequest(responseMessage);
			//  default:
			//    return NoContent();
			//}
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
				case StatusCodes.Status201Created:
					var createdAtRouteResource = result.ResultObject as CreatedAtRouteResource;
					return CreatedAtRoute(createdAtRouteResource?.ActionName,
										  createdAtRouteResource?.RouteValues,
										  createdAtRouteResource?.CreatedEntity);


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
				case StatusCodes.Status500InternalServerError:
					throw new Exception(result.Message);
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

		[AcceptVerbs(ActionMethods.Get,
		  Name = ActionNames.Comments.GetTaskComments,
		  Route = RouteNames.Comment.CommentsByTask)]
		public async Task<IActionResult> GetDashboardTaskComments(Guid id)
		{
			return NoContent();
		}

	}
}