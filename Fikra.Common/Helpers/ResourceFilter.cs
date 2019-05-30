using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fikra.Model.Interfaces;

namespace Fikra.Common.Helpers
{
	public class ResourceFilter<T, K> where T : IEntity<K> where K : IEquatable<K>
	{
		public Expression<Func<T, bool>> Expression { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public ICollection<string> Errors { get; set; } = new List<string>();

		public bool HasErrors => Errors.Any();

		public ResourceFilter() {}
	}
}
