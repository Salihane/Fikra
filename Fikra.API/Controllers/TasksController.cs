using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Extensions;
using Fikra.API.Filters;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Models;
using Fikra.API.Models.DashboardTask;
using Fikra.API.Services.Tasks;
using Fikra.Common.Constants;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Fikra.API.Controllers
{
	[ApiController]
	public class TasksController : ODataController // ControllerBase
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
    [ODataRoute]
    //[EnableQuery(PageSize = 2, AllowedQueryOptions = AllowedQueryOptions.All)]
    [EnableQuery()]
    //[AddSelectQuery(nameof(DashboardTaskDto.Status))]
    [AddExpandQuery(nameof(DashboardTaskDto.Links))]
    public async Task<IActionResult> GetDashboardTasks(int dashboardId,
			[FromQuery]DashboardTaskResourceParametersDto resourceParameters,
			[FromHeader(Name = HttpHeader.Accept)]string mediaType)
    {
      var request = this.Request;
			var (tasks, responseMetaData) = await _dashboardTaskService
        .GetTasksByDashboardIdNewAsync(dashboardId,
                                       resourceParameters,
                                       mediaType);

      var responseCode = responseMetaData?.ResponseCode ?? -1;
      var responseMessage = responseMetaData?.ResponseMessage ?? string.Empty;

      Response.AddHeaders(responseMetaData?.ResponseHeaders);
      Response.StatusCode = responseCode;

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


   //   if (responseHeaders != null)
   //   {
   //     Response.AddHeaders(responseHeaders);
   //   }

			//return Ok(tasks);

			//var result = await _dashboardTaskService
			//	.GetTasksByDashboardIdAsync(dashboardId, resourceParameters, mediaType);

			//Response.AddHeaders(result.ResponseHeaders);

			//switch (result.StatusCode)
			//{
			//	case StatusCodes.Status200OK:
			//		return Ok(result.ResultObject);
			//	case StatusCodes.Status404NotFound:
			//		return NotFound(result.Message);
			//	case StatusCodes.Status400BadRequest:
			//		return BadRequest(result.Message);
			//	case StatusCodes.Status201Created:
			//		var test = result.ResultObject;
			//		return null;
			//	//return CreatedAtRoute(ActionNames.GetContactNote,
			//	//                      new { contactId = contactId, id = returnedContactNote.Id },
			//	//                      returnedContactNote);

			//	// todo:
			//	default:
			//		return NoContent();
			//}
		}

		[AcceptVerbs(ActionMethods.Get,
			Name = ActionNames.Tasks.GetDashboardTaskById,
			Route = RouteNames.Task.TaskByDashboardAndId)]
		public async Task<IActionResult> GetTaskById(int dashboardId, Guid id)
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