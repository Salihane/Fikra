using System;
using System.Collections.Generic;
using System.Linq;

namespace Fikra.Common.Helpers
{
	public class PagedList<T> : List<T>
	{
		public PaginationMetaData PaginationMetaData { get; set; }

		public object Include { get; set; }

		private PagedList(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
		{
			PaginationMetaData = new PaginationMetaData
			{
				PageItemsMetaData =
					new PageItemsMetaData(pageNumber,
										  (int)Math.Ceiling(totalItems / (double)pageSize),
										  pageSize,
										  totalItems)
			};

			AddRange(items);

		}

		public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var totalItems = source.Count();
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
			return new PagedList<T>(items, totalItems, pageNumber, pageSize);
		}

		public PageItemsMetaData GetPageItemsMetaData()
		{
			return PaginationMetaData.PageItemsMetaData;
		}
	}

	public class PageItemsMetaData
	{
		public PageItemsMetaData(int currentPage, int totalPages, int pageSize, int totalItems)
		{
			CurrentPage = currentPage;
			TotalPages = totalPages;
			PageSize = pageSize;
			TotalItems = totalItems;
		}

		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalItems { get; private set; }
	}

	public class PaginationMetaData
	{
		public PageItemsMetaData PageItemsMetaData { get; set; }
		public bool HasPrevious => PageItemsMetaData?.CurrentPage > 1;
		public bool HasNext => PageItemsMetaData?.CurrentPage < PageItemsMetaData?.TotalPages;
	}
}
