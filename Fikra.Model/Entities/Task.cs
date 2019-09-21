using Fikra.Model.Entities.Enums;
using Fikra.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fikra.Model.Entities
{
    public class Task : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
		public DateTime? Due { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Effort Effort { get; set; }
        public ICollection<TaskComment> Comments { get; set; }
    }
}
