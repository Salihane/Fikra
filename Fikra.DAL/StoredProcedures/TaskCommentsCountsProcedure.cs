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
using Fikra.DAL.Types;
using Fikra.DAL.Views;

namespace Fikra.DAL.StoredProcedures
{
    public class TaskCommentsCountsProcedure : IStoredProcedure
    {
	    private readonly GuidList _guidList;
	    public readonly string TaskIdsKey = $"@{nameof(Model.QueryEntities.TaskCommentsCount.TaskId)}s";
	    public string Name => nameof(TaskCommentsCountsProcedure);
	    public string SqlQuery => $"{Name} {StoredProcedureUtility.GetParametersSignature(this)}";
	    public ICollection<SqlParameter> SqlParameters { get; set; }
	    public string Body => $@"CREATE PROCEDURE {Name}
						{TaskIdsKey} {_guidList.Name} READONLY
						AS
						BEGIN
							SET NOCOUNT ON;
							SELECT *
							FROM {new TaskCommentsCountView().Name} vtcc
							WHERE vtcc.TaskId IN (SELECT * FROM {TaskIdsKey})
						END";


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

	    public TaskCommentsCountsProcedure(IEnumerable<Guid> taskIds) : this()
	    {
			taskIds.ToList().ForEach(x => _guidList.DataTable.Rows.Add(x));
			SqlParameters.First().Value = _guidList.DataTable;
	    }
	}
}
