using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL.Views;
using NUnit.Framework;

namespace Fikra.DAL.UnitTests
{
	public class ViewBuilderTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void ViewBuilderCreate_TaskCommentsCountView_ReturnsViewSuccessfully()
		{
			// Arrange
			var viewBuilder = new ViewBuilder<TaskCommentsCountView>();

			// Act
			var viewBody = viewBuilder
			          .View()
			          .AsSelect()
			          .SelectCount($"t.Id as {TaskCommentsCountView.TaskId}")
			          .Count($"(c.Id) as {TaskCommentsCountView.CommentsCount}")
			          .FromJoin("Task t")
			          .JoinGroupBy("TaskComments c on c.TaskId = t.Id")
			          .GroupBy("t.Id");



			// Assert
			Assert.IsNotNull(viewBody);
			Assert.IsTrue(viewBody.Length > 0);
		}
	}
}
