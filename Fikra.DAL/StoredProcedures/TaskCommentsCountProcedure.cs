using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Fikra.DAL.Extensions;
using Fikra.DAL.Helpers;
using Fikra.DAL.StoredProcedures.Interfaces;
using Fikra.DAL.Views;

namespace Fikra.DAL.StoredProcedures
{
	public class TaskCommentsCountProcedure : IStoredProcedure
	{
		private readonly IStoredProcBuilder<TaskCommentsCountProcedure> _storedProcBuilder;
		public readonly string TaskId = $"@{nameof(Model.QueryEntities.TaskCommentsCount.TaskId)}";
		public string Name => DbUtility.GetDbObjectName<TaskCommentsCountProcedure>();
		public string SqlQuery => $"{Name} {StoredProcedureUtility.GetParametersSignature(this)}";
		public ICollection<SqlParameter> SqlParameters { get; set; }

		public string Body => _storedProcBuilder
							.StoredProc()
							.Input(new StoredProcInput
							{
								Name = TaskId,
								Specification = DbType.Guid.Translate()

							})
							.As()
							.Begin()
							.SelectAll()
							.From($"{new TaskCommentsCountView().Name} vtcc")
							.Where($"vtcc.TaskId = {TaskId}")
							.End();

		public TaskCommentsCountProcedure(IStoredProcBuilder<TaskCommentsCountProcedure> storedProcBuilder)
			: this()
		{
			_storedProcBuilder = storedProcBuilder;
		}

		public TaskCommentsCountProcedure()
		{
			SqlParameters = new Collection<SqlParameter>
			{
				new SqlParameter(TaskId, SqlDbType.UniqueIdentifier)
				{
					Direction = ParameterDirection.Input,
					Value = null
				}
			};
		}

		public TaskCommentsCountProcedure(Guid taskId)
			: this()
		{
			SqlParameters.First().Value = taskId;
		}
	}
}
