using System;
using System.Collections.Generic;
using System.Text;

namespace Fikra.Model.Entities
{
    public class TaskEffort : Effort
    {
        public virtual Guid TaskId { get; set; }
    }
}
