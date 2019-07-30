using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Model.Interfaces;
using Microsoft.AspNetCore.Routing;

namespace Fikra.API.Helpers
{
    public class CreatedAtRouteResource
	{
	    public string ActionName { get; set; }
	    public object RouteValues { get; set; }
	    public RouteValueDictionary CreatedEntity { get; set; }
    }
}
