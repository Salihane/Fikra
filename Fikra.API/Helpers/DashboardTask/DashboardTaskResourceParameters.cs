using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;
using Microsoft.Extensions.Configuration;

namespace Fikra.API.Helpers.DashboardTask
{
    public class DashboardTaskResourceParameters : IResourceParameters<Model.Entities.DashboardTask, Guid>
	{
		private int _pageSize = 10;
		private string _searchQuery;
		private readonly int _maxPageSize;
		private readonly int _searchQueryMaxLength;

		public DashboardTaskResourceParameters(IConfiguration config)
		{
			_maxPageSize = config[ConfigKeys.Pagination.DashboardTaskssMaxSize].TryParseToInt();
			_searchQueryMaxLength = config[ConfigKeys.Pagination.DashboardTaskssMaxSize].TryParseToInt();
		}

		public int PageNumber { get; set; } = 1;
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
		}
		public ResourceFilter<Model.Entities.DashboardTask, Guid> ResourceFilter { get; set; }

		public string SearchQuery
		{
			get => _searchQuery;
			set
			{
				if (value != null)
				{
					var query = value.Trim();
					_searchQuery = (query.Length > _searchQueryMaxLength)
						? query.Substring(0, _searchQueryMaxLength)
						: query;
				}
				else
				{
					_searchQuery = value;
				}
			}

		}
		public ICollection<string> Fields { get; set; }
	}
}
