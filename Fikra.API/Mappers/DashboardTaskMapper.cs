using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models;
using Fikra.DAL.Interfaces;
using Fikra.Model.Entities;

namespace Fikra.API.Mappers
{
    public class DashboardTaskMapper : IFikraMapper<DashboardTask, DashboardTaskDto>
    {
	    private readonly IRepository<DashboardTask, Guid> _dashboardTasksRepo;
	    private readonly IMapper _mapper;

		public DashboardTaskMapper(IRepository<DashboardTask, Guid> dashboardTasksRepo, IMapper mapper)
		{
			_dashboardTasksRepo = dashboardTasksRepo;
			_mapper = mapper;
		}
		public async Task<DashboardTask> ToModelAsync(DashboardTaskDto source)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<DashboardTask>> ToModelAsync(IReadOnlyCollection<DashboardTaskDto> sourceCollection)
		{
			throw new NotImplementedException();
		}

		public async Task<DashboardTaskDto> ToViewModelAsync(DashboardTask source)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<DashboardTaskDto>> ToViewModelAsync(IReadOnlyCollection<DashboardTask> sourceCollection)
		{
			if (sourceCollection == null || sourceCollection.Count == 0)
				return Enumerable.Empty<DashboardTaskDto>();

			var taskDtos = new List<DashboardTaskDto>();
			DashboardTask task;
			var taskChildNames = new[] { nameof(task.Comments) };
			foreach (var entity in sourceCollection)
			{
				var childsCounts = await _dashboardTasksRepo.CountChildsAsync(entity, taskChildNames);
				var taskDto = _mapper.Map<DashboardTaskDto>(entity);
				taskDto.CommentsCount = childsCounts[nameof(task.Comments)];
				taskDtos.Add(taskDto);
			}

			return taskDtos;
		}
	}
}
