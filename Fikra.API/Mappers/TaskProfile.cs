using AutoMapper;
using Fikra.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fikra.API.Mappers
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
			CreateMap<Model.Entities.Task, DashboardTask>();
			CreateMap<Model.Entities.Task, ProjectTask>();
		}
    }
}
