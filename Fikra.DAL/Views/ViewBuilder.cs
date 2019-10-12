using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.DAL.Views.Interfaces;
using Remotion.Linq.Clauses;

namespace Fikra.DAL.Views
{
    public class ViewBuilder<T> : IViewBuilder<T> where T : IView, new()
    {
		private StringBuilder _stringBuilder;

		public AsBuilder<T> View()
		{
			_stringBuilder = new StringBuilder($"CREATE VIEW {new T().Name}{Environment.NewLine}");

			return new AsBuilder<T>(_stringBuilder);
		}

		public abstract class StatementBuilder<TQ> where TQ : IView, new()
		{
			protected StringBuilder StringBuilder;
		}

		public class AsBuilder<TQ> : StatementBuilder<TQ> where TQ : IView, new()
		{
			public AsBuilder(StringBuilder stringBuilder)
			{
				StringBuilder = stringBuilder;
			}

			public SelectBuilder<TQ> AsSelect()
			{
				StringBuilder.Append($"AS{Environment.NewLine}");
				return new SelectBuilder<TQ>(StringBuilder);
			}

			public CountBuilder<TQ> AsCount()
			{
				StringBuilder.Append($"AS{Environment.NewLine}");
				return new CountBuilder<TQ>(StringBuilder);
			}

		}

		public class SelectBuilder<TQ> : StatementBuilder<TQ> where TQ : IView, new()
		{
			public SelectBuilder(StringBuilder stringBuilder)
			{
				StringBuilder = stringBuilder;
			}

			public FromBuilder<TQ> SelectFrom(string statement)
			{
				StringBuilder.Append($"SELECT {statement}{Environment.NewLine}");
				return new FromBuilder<TQ>(StringBuilder);
			}

			public CountBuilder<TQ> SelectCount(string statement)
			{
				StringBuilder.Append($"SELECT {statement}{Environment.NewLine}");
				return new CountBuilder<TQ>(StringBuilder);
			}

			public FromBuilder<TQ> SelectAll()
			{
				StringBuilder.Append($"SELECT * {Environment.NewLine}");
				return new FromBuilder<TQ>(StringBuilder);
			}

			public FromBuilder<TQ> Count(string statement)
			{
				StringBuilder.Append($"COUNT {statement}{Environment.NewLine}");
				return new FromBuilder<TQ>(StringBuilder);
			}
		}

		public class CountBuilder<TQ> : StatementBuilder<TQ> where TQ : IView, new()
		{
			public CountBuilder(StringBuilder stringBuilder)
			{
				StringBuilder = stringBuilder;
			}

			public FromBuilder<TQ> Count(string statement)
			{
				StringBuilder.Append($"COUNT {statement}{Environment.NewLine}");
				return new FromBuilder<TQ>(StringBuilder);
			}
		}

		public class FromBuilder<TQ> : StatementBuilder<TQ> where TQ : IView, new()
		{
			public FromBuilder(StringBuilder stringBuilder)
			{
				StringBuilder = stringBuilder;
			}

			public JoinBuilder<TQ> FromJoin(string statement)
			{
				StringBuilder.Append($"FROM {statement}{Environment.NewLine}");
				return new JoinBuilder<TQ>(StringBuilder);
			}

			public WhereBuilder<TQ> FromWhere(string statement)
			{
				StringBuilder.Append($"FROM {statement}{Environment.NewLine}");
				return new WhereBuilder<TQ>(StringBuilder);
			}

			public GroupByBuilder<TQ> FromGroupBy(string statement)
			{
				StringBuilder.Append($"FROM {statement}{Environment.NewLine}");
				return new GroupByBuilder<TQ>(StringBuilder);
			}
		}

		public class WhereBuilder<TQ> : StatementBuilder<TQ> where TQ : IView, new()
		{
			public WhereBuilder(StringBuilder stringBuilder)
			{
				StringBuilder = stringBuilder;
			}
			public string Where(string statement)
			{
				StringBuilder.Append($"WHERE {statement}{Environment.NewLine}");
				return StringBuilder.ToString();
			}
			public GroupByBuilder<TQ> WhereGroupBy(string statement)
			{
				StringBuilder.Append($"WHERE {statement}{Environment.NewLine}");
				return new GroupByBuilder<TQ>(StringBuilder);
			}
		}

		public class JoinBuilder<TQ> : StatementBuilder<TQ> where TQ : IView, new()
		{
			public JoinBuilder(StringBuilder stringBuilder)
			{
				StringBuilder = stringBuilder;
			}
			public string Join(string statement)
			{
				StringBuilder.Append($"JOIN {statement}{Environment.NewLine}");
				return StringBuilder.ToString();
			}
			public GroupByBuilder<TQ> JoinGroupBy(string statement)
			{
				StringBuilder.Append($"JOIN {statement}{Environment.NewLine}");
				return new GroupByBuilder<TQ>(StringBuilder);
			}
		}

		public class GroupByBuilder<TQ> : StatementBuilder<TQ> where TQ : IView, new()
		{
			public GroupByBuilder(StringBuilder stringBuilder)
			{
				StringBuilder = stringBuilder;
			}
			public string GroupBy(string statement)
			{
				StringBuilder.Append($"GROUP BY {statement}{Environment.NewLine}");
				return StringBuilder.ToString();
			}
		}

	}
}
