using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.API.Helpers.DashboardTask
{
	public class DashboardTaskResourceUri : IResourceUri<Model.Entities.DashboardTask, Guid>
	{
		private readonly IMapper _mapper;

		public DashboardTaskResourceUri(IMapper mapper)
		{
			_mapper = mapper;
		}
		public string CreateResourceUri(
			IResourceParameters<Model.Entities.DashboardTask, Guid> resourceParams,
			ResourceUriType uriType, 
			IUrlHelper urlHelper, 
			string urlName)
		{
			var dashboardTaskResourceParamDto = _mapper.Map<DashboardTaskResourceParametersDto>(resourceParams);

			switch (uriType)
			{
				case ResourceUriType.Previous:
					return urlHelper.Link(urlName,
						new
						{
							pageNumber = dashboardTaskResourceParamDto.PageNumber - 1,
							pageSize = dashboardTaskResourceParamDto.PageSize,
							filter = dashboardTaskResourceParamDto.Filter,
							search = dashboardTaskResourceParamDto.Search,
							fields = dashboardTaskResourceParamDto.Fields
						});
				case ResourceUriType.Next:
					return urlHelper.Link(urlName,
						new
						{
							pageNumber = dashboardTaskResourceParamDto.PageNumber + 1,
							pageSize = dashboardTaskResourceParamDto.PageSize,
							filter = dashboardTaskResourceParamDto.Filter,
							search = dashboardTaskResourceParamDto.Search,
							fields = dashboardTaskResourceParamDto.Fields
						});
				case ResourceUriType.Current:
				default:
					return urlHelper.Link(urlName,
						new
						{
							pageNumber = dashboardTaskResourceParamDto.PageNumber,
							pageSize = dashboardTaskResourceParamDto.PageSize,
							filter = dashboardTaskResourceParamDto.Filter,
							search = dashboardTaskResourceParamDto.Search,
							fields = dashboardTaskResourceParamDto.Fields
						});
			}
		}
	}
}
