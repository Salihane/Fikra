using System;
using Fikra.Model.Interfaces;

namespace Fikra.Model.QueryEntities
{
    public class TaskCommentsCount : IQueryEntity
    {
		public Guid TaskId { get; set; }
	    public int CommentsCount { get; set; }
    }
}
