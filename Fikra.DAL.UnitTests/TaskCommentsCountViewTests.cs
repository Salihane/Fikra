using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL.Views;
using NUnit.Framework;

namespace Fikra.DAL.UnitTests
{
    public class TaskCommentsCountViewTests
    {
		[SetUp]
	    public void Setup()
	    {
	    }

	    [Test]
	    public void TaskCommentsCountViewTests_Creation_Succeeds()
	    {
			// Arrange
			var view = new TaskCommentsCountView(new ViewBuilder<TaskCommentsCountView>());

			// Act
			var body = view.Body;

			// Assert
			Assert.IsNotNull(body);
			Assert.IsTrue(body.Length > 0);
	    }
	}
}
