using System;
using System.Collections.Generic;
using System.Text;

namespace Fikra.Model.Entities
{
    public class TaskComment : Comment
    {
        public Task Task { get; set; }
    }
}
