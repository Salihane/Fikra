using System;
using System.Collections.Generic;
using System.Text;
using Fikra.Model.Interfaces;

namespace Fikra.Model.Entities
{
	public class Project : IEntity<int>
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ModifiedOn { get; set; }
		public bool IsDeleted { get; set; }
		public string Name { get; set; }
		public virtual ICollection<ProjectTask> Tasks { get; set; }
	}
}
