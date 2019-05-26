using Fikra.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Fikra.Model.Entities
{
    public class Dashboard : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DashboardTask> Tasks { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
