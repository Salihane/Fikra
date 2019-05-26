using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Model.Entities
{
    public class ProjectTask : Task
    {
	    public Project Project { get; set; }
	    public int ProjectId { get; set; }
    }
}
