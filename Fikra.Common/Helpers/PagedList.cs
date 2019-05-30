using System;
using System.Collections.Generic;
using System.Linq;

namespace Fikra.Common.Helpers
{
	public class PagedList<T> : List<T>
	{
		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalItems { get; private set; }

		public bool HasPrevious => CurrentPage > 1;

		public bool HasNext => CurrentPage < TotalPages;

		public object Include { get; set; }

		private PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
		{
			TotalItems = totalItems;
			CurrentPage = pageNumber;
			PageSize = pageSize;
			TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
			AddRange(items);

		}

		public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var totalItems = source.Count();
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			return new PagedList<T>(items, totalItems, pageNumber, pageSize);
		}
	}
}
