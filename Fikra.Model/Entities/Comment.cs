using Fikra.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fikra.Model.Entities
{
    public class Comment : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Content { get; set; }
    }
}
