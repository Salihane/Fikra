using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Common.Constants;

namespace Fikra.DAL.StoredProcedures
{
	public class StoredProcBuilder<T> where T : IStoredProcedure, new()
	{
		private StringBuilder _stringBuilder;

		public StoredProcBuilder<T> StoredProc()
		{
			_stringBuilder = new StringBuilder($"CREATE PROCEDURE {new T().Name}{Environment.NewLine}");

			return this;
		}

		public StoredProcBuilder<T> From(string statement)
		{
			_stringBuilder.Append($"FROM {statement}{Environment.NewLine}");
			return this;
		}

		public StoredProcBuilder<T> Where(string statement)
		{
			_stringBuilder.Append($"WHERE {statement}{Environment.NewLine}");
			return this;
		}

		public StoredProcBuilder<T> Select(string statement)
		{
			_stringBuilder.Append($"SELECT {statement}{Environment.NewLine}");
			return this;
		}

		public StoredProcBuilder<T> Begin()
		{
			_stringBuilder.Append($"BEGIN{Environment.NewLine}SET NOCOUNT ON;{Environment.NewLine}");
			return this;
		}

		public StoredProcBuilder<T> As()
		{
			_stringBuilder.Append($"AS{Environment.NewLine}");
			return this;
		}

		public StoredProcBuilder<T> Input(StoredProcInput input)
		{
			_stringBuilder.Append($" {input.Name} {input.Spec}{Environment.NewLine}");
			return this;
		}

		public StoredProcBuilder<T> Input(IEnumerable<StoredProcInput> input)
		{
			var inputParams = input.ToList();
			foreach (var inputParam in inputParams)
			{
				var isLastInputParam = inputParams.IndexOf(inputParam) == inputParams.Count - 1;
				var inputParamsSeparator = isLastInputParam ? char.MinValue : Chars.Comma;
				_stringBuilder.Append($"{inputParam.Name} {inputParam.Spec}{inputParamsSeparator}{Environment.NewLine}");
			}

			return this;
		}

		public string End()
		{
			_stringBuilder.Append($"END;{Environment.NewLine}");
			return _stringBuilder.ToString();
		}




	}
}
