using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Model.QueryEntities;

namespace Fikra.DAL.Views
{
    public class TaskCommentsCountView : IView
    {
	    public string Name => $"[dbo].[{nameof(TaskCommentsCountView)}]";
	    public string Body => $@"CREATE VIEW {Name} AS 
				        SELECT t.Id as {nameof(TaskCommentsCount.TaskId)}, 
						Count(c.Id) as {nameof(TaskCommentsCount.CommentsCount)} 
				        FROM Task t
				        JOIN TaskComments c on c.TaskId = t.Id
				        GROUP BY t.Id";
	}
}
