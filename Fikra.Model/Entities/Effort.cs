using Fikra.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fikra.Model.Entities
{
    public class Effort : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
		public double Estimated { get; set; }
        public double Remaining { get; set; }
        public double Completed { get; set; }
    }
}
