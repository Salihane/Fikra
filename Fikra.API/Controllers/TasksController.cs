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
        public async Task Post([FromBody]Entities.Task task)
        {
            _tasksRepo.Add(task);
            await _tasksRepo.SaveChangesAsync();
        }
    }
}