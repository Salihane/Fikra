using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Fikra.DAL.Extensions;
using Fikra.DAL.Helpers;
using Fikra.DAL.Views;

namespace Fikra.DAL.StoredProcedures
{
	public class TaskCommentsCountProcedure : IStoredProcedure
	{
		public StoredProcBuilder<TaskCommentsCountProcedure> StoredProcBuilder = new StoredProcBuilder<TaskCommentsCountProcedure>();
		public readonly string TaskIdKey = $"@{nameof(Model.QueryEntities.TaskCommentsCount.TaskId)}";
		public string Name => nameof(TaskCommentsCountProcedure);
		public string SqlQuery => $"{Name} {StoredProcedureUtility.GetParametersSignature(this)}";
		public ICollection<SqlParameter> SqlParameters { get; set; }

		public string Body => new StoredProcBuilder<TaskCommentsCountProcedure>()
							.StoredProc()
							.Input(new StoredProcInput { Name = TaskIdKey, Spec = DbType.Guid.Translate() })
							.As()
							.Begin()
							.Select("*")
							.From($"{new TaskCommentsCountView().Name} vtcc")
							.Where($"vtcc.TaskId = {TaskIdKey}")
							.End();

		public TaskCommentsCountProcedure()
		{
			SqlParameters = new Collection<SqlParameter>
			{
				new SqlParameter(TaskIdKey, SqlDbType.UniqueIdentifier)
				{
					Direction = ParameterDirection.Input,
					Value = null
				}
			};
		}

		public TaskCommentsCountProcedure(Guid taskId) : this()
		{
			SqlParameters.First().Value = taskId;
		}
	}
}
