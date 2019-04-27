using Fikra.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fikra.Model.Entities
{
    public class Task : IEntity<Guid>
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
