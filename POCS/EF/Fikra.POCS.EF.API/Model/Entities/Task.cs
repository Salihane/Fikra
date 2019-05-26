using System;
using Fikra.POCS.EF.API.Model.Interfaces;

namespace Fikra.POCS.EF.API.Model.Entities
{
    public class Task : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
