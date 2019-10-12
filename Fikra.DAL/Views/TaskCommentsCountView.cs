using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL.Views.Interfaces;
using Fikra.Model.QueryEntities;

namespace Fikra.DAL.Views
{
    public class TaskCommentsCountView : IView
    {
	    public static string TaskId => nameof(TaskCommentsCount.TaskId);
	    public static string CommentsCount => nameof(TaskCommentsCount.CommentsCount);
	    public string Name => $"[dbo].[{nameof(TaskCommentsCountView)}]";
	    public string Body => $@"CREATE VIEW {Name} AS 
				        SELECT t.Id as {TaskId}, 
						Count(c.Id) as {CommentsCount} 
				        FROM Task t
				        JOIN TaskComments c on c.TaskId = t.Id
				        GROUP BY t.Id";
	}
}
