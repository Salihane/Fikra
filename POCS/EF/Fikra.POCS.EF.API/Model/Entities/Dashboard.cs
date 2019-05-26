using System;
using System.Collections.Generic;
using Fikra.POCS.EF.API.Model.Interfaces;

namespace Fikra.POCS.EF.API.Model.Entities
{
    public class Dashboard : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
