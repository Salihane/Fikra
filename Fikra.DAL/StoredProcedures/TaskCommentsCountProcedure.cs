using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Fikra.DAL.Helpers;
using Fikra.DAL.Views;

namespace Fikra.DAL.StoredProcedures
{
	public class TaskCommentsCountProcedure : IStoredProcedure
	{
		public readonly string TaskIdKey = $"@{nameof(Model.QueryEntities.TaskCommentsCount.TaskId)}";
		//public string Name => $"[dbo].[{nameof(TaskCommentsCountProcedure)}]";
		public string Name => nameof(TaskCommentsCountProcedure);
		public string SqlQuery => $"{Name} {StoredProcedureUtility.GetParametersSignature(this)}";
		//public Dictionary<string, object> Parameters { get; set; }
		public ICollection<SqlParameter> SqlParameters { get; set; }
		public string Body => $@"CREATE PROCEDURE {Name}
						{TaskIdKey} uniqueidentifier
						AS
						BEGIN
							SELECT *
							FROM {new TaskCommentsCountView().Name} vtcc
							WHERE vtcc.TaskId = {TaskIdKey}
						END";


		public TaskCommentsCountProcedure()
		{
			//Parameters = new Dictionary<string, object>
			//{
			//	{TaskIdKey, null}
			//};

			SqlParameters = new Collection<SqlParameter>
			{
				new SqlParameter(TaskIdKey, SqlDbType.UniqueIdentifier)
				{
					Direction = ParameterDirection.Input,
					Value = null
				}
			};
		}

		public TaskCommentsCountProcedure(Guid taskId):this()
		{
			//Parameters[TaskIdKey] = taskId;
			SqlParameters.First().Value = taskId;
		}

		//public void BuildSqlParameters()
		//{
		//	SqlParameters = new Collection<SqlParameter>();
		//	foreach (var (key, value) in Parameters)
		//	{
		//		SqlParameters.Add(new SqlParameter(key, SqlDbType.Int)
		//		{
		//			Direction = ParameterDirection.Output,
		//			Value =  value
		//		});
		//	}
		//}
	}
}
