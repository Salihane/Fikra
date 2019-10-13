using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL.Helpers;
using Fikra.DAL.Views.Interfaces;
using Fikra.Model.Entities;
using Fikra.Model.QueryEntities;
using Task = Fikra.Model.Entities.Task;

namespace Fikra.DAL.Views
{
	public class TaskCommentsCountView : IView
	{
		private readonly IViewBuilder<TaskCommentsCountView> _viewBuilder;

		public static string TaskId => nameof(TaskCommentsCount.TaskId);
		public static string CommentsCount => nameof(TaskCommentsCount.CommentsCount);
		public string Name => DbUtility.GetDbObjectName<TaskCommentsCountView>();

		public string Body => _viewBuilder
				.View()
				.AsSelect()
				.SelectCount($"t.Id as {TaskCommentsCountView.TaskId}")
				.Count($"(c.Id) as {TaskCommentsCountView.CommentsCount}")
				.FromJoin("Task t")
				.JoinGroupBy("TaskComments c on c.TaskId = t.Id")
				.GroupBy("t.Id");


		public TaskCommentsCountView()
		{
		}

		public TaskCommentsCountView(IViewBuilder<TaskCommentsCountView> viewBuilder)
		{
			_viewBuilder = viewBuilder;
		}
	}
}
