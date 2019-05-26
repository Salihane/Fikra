using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Model.Entities
{
    public class TaskComment : Text
    {
	    public Task Task { get; set; }
	    public Guid TaskId { get; set; }
    }
}
