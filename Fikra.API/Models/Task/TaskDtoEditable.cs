using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Model.Entities;
using Fikra.Model.Entities.Enums;

namespace Fikra.API.Models.Task
{
    public class TaskDtoEditable
    {
		public string Name { get; set; }
		public DateTime? Due { get; set; }
		public Status Status { get; set; }
		public Priority Priority { get; set; }
		public Effort Effort { get; set; }
	}
}
