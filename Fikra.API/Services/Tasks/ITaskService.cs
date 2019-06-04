using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Helpers.ResourceResponse;
using Fikra.API.Models.DashboardTask;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fikra.API.Services.Tasks
{
    public interface ITaskService
    {
	    Task<ResourceResult> GetTasksByDashboardIdAsync(
		    int dashboardId,
		    DashboardTaskResourceParametersDto resourceParametersDto,
		    string mediaType);

		   Task<ResourceResult> CreateTaskAsync(
			   int dashboardId,
			   DashboardTaskDtoCreate dashboardTaskDtoCreate, 
			   string mediaType, 
			   ModelStateDictionary modelState);
    }
}
