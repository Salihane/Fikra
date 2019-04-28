using System;
using System.Collections.Generic;
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
        public IEnumerable<Dashboard> Get()
        {
            return _dashboardRepo.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Dashboard dashboard)
        {
            _dashboardRepo.Add(dashboard);
            await _dashboardRepo.SaveChangesAsync();

            return Ok(dashboard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Dashboard dashboard)
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
        public IActionResult Get(int id)
        {
            var dashboard = _dashboardRepo.SearchFor(x => x.Id.Equals(id), y => y.Tasks);

            if (dashboard == null)
            {
                return BadRequest("The requested dashboard couldn't be found");
            }

            var tasks = dashboard.FirstOrDefault().Tasks;

            return Ok(tasks);
        }
    }
}