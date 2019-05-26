using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IRepository<Dashboard, int> _dashboardRepo;

        public DashboardsController(IRepository<Dashboard, int> dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }

        [HttpGet]
        public IEnumerable<Dashboard> GetAllDashboards()
        {
            return _dashboardRepo.GetAll();
        }

        //[HttpPost]
        //public async Task<IActionResult> PostDashboard([FromBody] Dashboard dashboard)
        //{
        //    _dashboardRepo.Add(dashboard);
        //    await _dashboardRepo.SaveChangesAsync();

        //    return Ok(dashboard);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDashboard(int id, [FromBody] Dashboard dashboard)
        //{
        //    if (id != dashboard.Id)
        //    {
        //        return BadRequest("Query id and entity id mismatch");
        //    }

        //    _dashboardRepo.Update(dashboard);
        //    await _dashboardRepo.SaveChangesAsync();

        //    return Ok(dashboard);
        //}

        //[HttpGet("{id}/tasks")]
        //public async Task<IActionResult> GetDashboardTask(int id)
        //{
        //    var dashboard = await _dashboardRepo.SearchForAsync(x => x.Id.Equals(id), y => y.Tasks);

        //    if (dashboard == null)
        //    {
        //        return BadRequest("The requested dashboard couldn't be found");
        //    }

        //    var tasks = dashboard.Single().Tasks;

        //    return Ok(tasks);
        //}

        //[HttpPost("{id}/tasks")]
        //public async Task<IActionResult> PostDashboardTask(int id, [FromBody]Model.Entities.Task task)
        //{
        //    var dashboardSearch = await _dashboardRepo
        //        .SearchForAsync(x => x.Id.Equals(id), y => y.Tasks);

        //    if (dashboardSearch == null)
        //    {
        //        return BadRequest("The requested dashboard couldn't be found");
        //    }

        //    var dashboard = dashboardSearch.Single();

        //    var taskExists = dashboard.Tasks
        //        .FirstOrDefault(x => x.Name.Equals(task.Name, StringComparison.CurrentCultureIgnoreCase)) != null;

        //    if (taskExists)
        //    {
        //        return BadRequest("A task with the same name exists already!");
        //    }
            
        //    dashboard.Tasks.Add(task);
        //    await _dashboardRepo.SaveChangesAsync();

        //    return Ok(task);
        //}

    }
}