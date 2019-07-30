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
        .ReverseMap();
    }
	}
}
