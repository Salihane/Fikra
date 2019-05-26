using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Model.Interfaces;

namespace Fikra.Model.Entities
{
    public class DashboardTask : Task
    {
	    public Dashboard Dashboard { get; set; }
	    public int DashboardId { get; set; }
    }
}
