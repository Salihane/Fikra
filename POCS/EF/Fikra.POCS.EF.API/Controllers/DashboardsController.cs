using System;
using System.Collections.Generic;
using System.Linq;
using ThreadingTasks = System.Threading.Tasks;
using Fikra.POCS.EF.API.DAL.Interfaces;
using Fikra.POCS.EF.API.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.POCS.EF.API.Controllers
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

        [HttpPost]
        public async ThreadingTasks.Task<IActionResult> PostDashboard([FromBody] Dashboard dashboard)
        {
            _dashboardRepo.Add(dashboard);
            await _dashboardRepo.SaveChangesAsync();

            return Ok(dashboard);
        }

        [HttpPut("{id}")]
        public async ThreadingTasks.Task<IActionResult> PutDashboard(int id, [FromBody] Dashboard dashboard)
        {
            if (id != dashboard.Id)
            {
                return BadRequest("Query id and entity id mismatch");
            }

            _dashboardRepo.Update(dashboard);
            await _dashboardRepo.SaveChangesAsync();

            return Ok(dashboard);
        }

        [HttpGet("{id}/tasks")]
        public async ThreadingTasks.Task<IActionResult> GetDashboardTask(int id)
        {
            var dashboard = await _dashboardRepo.SearchForAsync(x => x.Id.Equals(id), y => y.Tasks);

            if (dashboard == null)
            {
                return BadRequest("The requested dashboard couldn't be found");
            }

            var tasks = dashboard.Single().Tasks;

            return Ok(tasks);
        }

        [HttpPost("{id}/tasks")]
        public async ThreadingTasks.Task<IActionResult> PostDashboardTask(int id, [FromBody]Task task)
        {
            var dashboardSearch = await _dashboardRepo
                .SearchForAsync(x => x.Id.Equals(id), y => y.Tasks);

            if (dashboardSearch == null)
            {
                return BadRequest("The requested dashboard couldn't be found");
            }

            var dashboard = dashboardSearch.Single();

            var taskExists = dashboard.Tasks
                .FirstOrDefault(x => x.Name.Equals(task.Name, StringComparison.CurrentCultureIgnoreCase)) != null;

            if (taskExists)
            {
                return BadRequest("A task with the same name exists already!");
            }
            
            dashboard.Tasks.Add(task);
            await _dashboardRepo.SaveChangesAsync();

            return Ok(task);
        }

    }
}