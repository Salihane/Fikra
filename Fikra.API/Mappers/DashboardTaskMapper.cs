using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models.DashboardTask;
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
      if (source == null) return null;

      var taskChildNames = new[] { nameof(DashboardTask.Comments) };
      var childsCounts = await _dashboardTasksRepo.CountChildsAsync(source, taskChildNames);
      var taskDto = _mapper.Map<DashboardTaskDto>(source);
      taskDto.CommentsCount = childsCounts[nameof(DashboardTask.Comments)];

      return taskDto;
    }

    public async Task<IEnumerable<DashboardTaskDto>> ToViewModelAsync(IReadOnlyCollection<DashboardTask> sourceCollection)
    {
      if (sourceCollection == null || sourceCollection.Count == 0)
        return Enumerable.Empty<DashboardTaskDto>();

      var taskDtos = new List<DashboardTaskDto>();
      foreach (var dashboardTask in sourceCollection)
      {
        var taskDto = await ToViewModelAsync(dashboardTask);
        taskDtos.Add(taskDto);
      }
      //var taskChildNames = new[] { nameof(DashboardTask.Comments) };

      //foreach (var entity in sourceCollection)
      //{
      //  var childsCounts = await _dashboardTasksRepo.CountChildsAsync(entity, taskChildNames);
      //  var taskDto = _mapper.Map<DashboardTaskDto>(entity);
      //  taskDto.CommentsCount = childsCounts[nameof(DashboardTask.Comments)];
      //  taskDtos.Add(taskDto);
      //}

      return taskDtos;
    }
  }
}
