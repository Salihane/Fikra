using System;
using System.Collections.Generic;
using System.Text;

namespace Fikra.Model
{
    public class Task : IEntity<Guid>
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
