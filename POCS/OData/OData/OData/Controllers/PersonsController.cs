using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using OData.Models;

namespace OData.Controllers
{
  //[Route("api/[controller]")]
  //[ApiController]
  [Produces("application/json")]
  public class PersonsController : ODataController
  {
    private static Random Random = new Random(DateTime.Now.Millisecond);
    // GET api/values
    [HttpGet]
    //[ODataRoute]
    [EnableQuery]
    public IActionResult Get()
    {
      var persons = new List<Person>
      {
        CreatePerson(),
        CreatePerson(),
        CreatePerson(),
        CreatePerson()
      };

      return Ok(persons.AsQueryable());
    }

    private static Person CreatePerson()
    {
      var id = Random.Next(99999);
      return new Person
      {
        Id = id,
        Name = $"Person {id}",
        Age = Random.Next(15, 50)
      };
    }
  }
}
