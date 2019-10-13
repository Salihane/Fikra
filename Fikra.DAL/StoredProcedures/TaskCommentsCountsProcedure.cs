using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Common.Constants;
using Fikra.DAL.Helpers;
using Fikra.DAL.StoredProcedures.Interfaces;
using Fikra.DAL.Types;
using Fikra.DAL.Views;

namespace Fikra.DAL.StoredProcedures
{
	public class TaskCommentsCountsProcedure : IStoredProcedure
	{
		private readonly IStoredProcBuilder<TaskCommentsCountsProcedure> _storedProcBuilder;
		private readonly GuidList _guidList;
		public readonly string TaskIdsKey = $"@{nameof(Model.QueryEntities.TaskCommentsCount.TaskId)}s";
		public string Name => DbUtility.GetDbObjectName<TaskCommentsCountsProcedure>();
		public string SqlQuery => $"{Name} {StoredProcedureUtility.GetParametersSignature(this)}";
		public ICollection<SqlParameter> SqlParameters { get; set; }

		public string Body => _storedProcBuilder
							  .StoredProc()
							  .Input(new StoredProcInput
							  {
								  Name = TaskIdsKey,
								  Specification = $"{_guidList.Name} READONLY"
							  })
							  .As()
							  .Begin()
							  .SelectAll()
							  .From($"{new TaskCommentsCountView().Name} vtcc")
							  .Where($"vtcc.TaskId IN (SELECT * FROM {TaskIdsKey})")
							  .End();

		public TaskCommentsCountsProcedure(IStoredProcBuilder<TaskCommentsCountsProcedure> storedProcBuilder)
			: this()
		{
			_storedProcBuilder = storedProcBuilder;
		}
		public TaskCommentsCountsProcedure()
		{
			_guidList = new GuidList();
			SqlParameters = new Collection<SqlParameter>
			{
				new SqlParameter(TaskIdsKey, SqlDbType.Structured)
				{
					Direction = ParameterDirection.Input,
					TypeName = _guidList.Name,
					Value = null
				}
			};
		}

		public TaskCommentsCountsProcedure(IEnumerable<Guid> taskIds)
			: this()
		{
			taskIds.ToList().ForEach(x => _guidList.DataTable.Rows.Add(x));
			SqlParameters.First().Value = _guidList.DataTable;
		}
	}
}
