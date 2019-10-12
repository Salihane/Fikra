using System.Data;
using Fikra.DAL.Extensions;
using Fikra.DAL.StoredProcedures;
using Fikra.DAL.Views;
using NUnit.Framework;

namespace Fikra.DAL.UnitTests
{
	public class StoredProcBuilderTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void StoredProcBuilderCreate_TaskCommentsCount_ReturnsStoredProcSuccessfully()
		{
			// Arrange
			var storedProcBuilder = new StoredProcBuilder<TaskCommentsCountProcedure>();
			var taskIdKey = $"@{nameof(Fikra.Model.QueryEntities.TaskCommentsCount.TaskId)}";

			// Act
			var storedProc = storedProcBuilder
			                 .StoredProc()
			                 .Input(new StoredProcInput { Name = taskIdKey, Specification = DbType.Guid.Translate() })
			                 .As()
			                 .Begin()
			                 .SelectAll()
			                 .From($"{new TaskCommentsCountView().Name} vtcc")
			                 .Where($"vtcc.TaskId = {taskIdKey}")
			                 .End();

			// Assert
			Assert.IsNotNull(storedProc);
			Assert.IsTrue(storedProc.Length > 0);
		}
	}
}