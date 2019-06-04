using AutoMapper;
using Fikra.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Fikra.API.Helpers.DashboardTask;
using Fikra.API.Models;
using Fikra.API.Models.DashboardTask;
using Fikra.Common.Constants;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;

namespace Fikra.API.Mappers
{
	public class TaskProfile : Profile
	{
		public TaskProfile(Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			CreateMap<Model.Entities.Task, DashboardTask>();
			CreateMap<Model.Entities.Task, ProjectTask>();

			CreateMap<DashboardTask, DashboardTaskDto>().ReverseMap();
			CreateMap<DashboardTask, DashboardTaskDtoCreate>().ReverseMap();
			CreateMap<DashboardTask, DashboardTaskDtoUpdate>().ReverseMap();

			CreateMap<DashboardTaskResourceParametersDto, DashboardTaskResourceParameters>()
				.ForMember(dest => dest.PageNumber, opt => opt.Condition(src => src.PageNumber != 0))
				.ForMember(dest => dest.PageSize, opt => opt.Condition(src => src.PageSize != 0))
				.ForMember(dest => dest.SearchQuery, opt => opt.MapFrom(src => src.Search))
				.ForMember(dest => dest.Fields, opt => opt.MapFrom((src, dest) =>
					src.Fields?.Split(Chars.Comma)))
				.ForMember(dest => dest.ResourceFilter, opt => opt.MapFrom((src, dest) =>
				{
					if (string.IsNullOrEmpty(src.Filter)) return null;

					var nameToFilter = src.Filter.Trim();
					var nameToFilterMaxLength = configuration[ConfigKeys.Pagination.DashboardTaskFilterNameMaxLength]
						.TryParseToInt();

					if (nameToFilter.Length > nameToFilterMaxLength)
						nameToFilter = nameToFilter.Substring(0, nameToFilterMaxLength);

					Func<DashboardTask, bool> contains = x =>
						x.Name.Contains(nameToFilter, StringComparison.CurrentCultureIgnoreCase);
					Expression<Func<DashboardTask, bool>> predicate = x => contains(x);


					return new ResourceFilter<DashboardTask, Guid>
					{
						Name = "Name",
						Value = nameToFilter,
						Expression = predicate
					};
				})).ReverseMap()
					.ForMember(dest => dest.Search, opt => opt.MapFrom((src, dest) => src.SearchQuery))
					.ForMember(dest => dest.Filter, opt => opt.MapFrom((src, dest) => src.ResourceFilter?.Value))
					.ForMember(dest => dest.Fields, opt => opt.MapFrom((src, dest) =>
						src.Fields == null ? null : string.Join(",", src.Fields)));
		}
	}
}
