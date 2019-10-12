using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL.StoredProcedures;
using NUnit.Framework;

namespace Fikra.DAL.UnitTests
{
    public class TaskCommentsCountProcedureTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void TaskCommentsCountProcedure_Creation_Succeeds()
		{
			// Arrange
			var storedProc = new TaskCommentsCountProcedure(new StoredProcBuilder<TaskCommentsCountProcedure>());

			// Act
			var body = storedProc.Body;

			// Assert
			Assert.IsNotNull(body);
			Assert.IsTrue(body.Length > 0);
		}

	}
}
