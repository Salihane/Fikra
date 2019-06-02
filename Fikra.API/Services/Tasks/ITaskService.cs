﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.API.Helpers;
using Fikra.API.Helpers.DashboardTask;
using Fikra.Model.Entities;

namespace Fikra.API.Services.Tasks
{
    public interface ITaskService
    {
	    Task<ResourceResult> GetTasksByDashboardIdAsync(
		    int dashboardId,
		    DashboardTaskResourceParametersDto resourceParametersDto,
		    string mediaType);



    }
}
