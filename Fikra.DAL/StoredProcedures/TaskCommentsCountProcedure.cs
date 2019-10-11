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
		public readonly string TaskIdKey = $"@{nameof(Model.QueryEntities.TaskCommentsCount.TaskId)}";
		public string Name => nameof(TaskCommentsCountProcedure);
		public string SqlQuery => $"{Name} {StoredProcedureUtility.GetParametersSignature(this)}";
		public ICollection<SqlParameter> SqlParameters { get; set; }
		//{TaskIdKey} {DbTypeUtility.Translate(DbType.Guid)}
		public string Body => $@"CREATE PROCEDURE {Name}
						{TaskIdKey} {DbType.Guid.Translate()}
						AS
						BEGIN
							SET NOCOUNT ON;
							SELECT *
							FROM {new TaskCommentsCountView().Name} vtcc
							WHERE vtcc.TaskId = {TaskIdKey}
						END";


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
