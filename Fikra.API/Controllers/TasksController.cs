using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fikra.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities = Fikra.Model.Entities;

namespace Fikra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IRepository<Entities.Task, Guid> _tasksRepo;

        public TasksController(IRepository<Entities.Task, Guid> tasksRepo)
        {
            _tasksRepo = tasksRepo;
        }

        [HttpGet]
        public IEnumerable<Entities.Task> Get()
        {
            return _tasksRepo.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Entities.Task task)
        {
            _tasksRepo.Add(task);
            await _tasksRepo.SaveChangesAsync();

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]Entities.Task taskData)
        {
            if (taskData.Id != id)
            {
                return BadRequest("Query id and entity id mismatch");
            }

            _tasksRepo.Update(taskData);
            await _tasksRepo.SaveChangesAsync();

            return Ok(taskData);
        }
    }
}