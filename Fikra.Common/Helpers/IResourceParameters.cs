using System;
using System.Collections.Generic;
using Fikra.Model.Interfaces;

namespace Fikra.Common.Helpers
{
	public interface IResourceParameters<T, K> where T : IEntity<K> where K : IEquatable<K>
	{
		int PageNumber { get; set; }
		int PageSize { get; set; }
	}
}
