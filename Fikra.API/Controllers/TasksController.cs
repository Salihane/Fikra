using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fikra.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IRepository<Model.Task, Guid> _tasksRepo;

        public TasksController(IRepository<Model.Task, Guid> tasksRepo)
        {
            _tasksRepo = tasksRepo;
        }

        [HttpGet]
        public IEnumerable<Model.Task> Get()
        {
            return _tasksRepo.GetAll();
        }

        [HttpPost]
        public async Task Post([FromBody]Model.Task task)
        {
            _tasksRepo.Add(task);
            await _tasksRepo.SaveChangesAsync();
        }
    }
}