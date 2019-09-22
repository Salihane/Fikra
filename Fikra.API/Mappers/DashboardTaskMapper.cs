using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fikra.API.Mappers.Interfaces;
using Fikra.API.Models.DashboardTask;
using Fikra.DAL.Interfaces;
using Fikra.DAL.StoredProcedures;
using Fikra.Model.Entities;
using Fikra.Model.QueryEntities;

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
	  //var taskId = new SqlParameter("@TaskId", SqlDbType.UniqueIdentifier) {Value = new Guid("DD413A21-6431-4F70-EAB5-08D73E4DFBEF"), Direction = ParameterDirection.Input};
	  //var count = new SqlParameter("@CommentsCount", SqlDbType.Int) {Direction = ParameterDirection.Output};
	  //var parms = new SqlParameter[] {taskId, count};

	  //var comments = _dashboardTasksRepo.ExecuteStoredProc("[dbo].[GetTaskComments] @TaskId", taskId);
	  //var commentsCount = _dashboardTasksRepo.ExecuteStoredProc("[dbo].[CountTaskComments] @TaskId,  @CommentsCount OUT", parms);


	  var storedProc = new TaskCommentsCountProcedure(source.Id);

	  var taskId = source.Id;
	  var y = await _dashboardTasksRepo.ExecuteStoredProc<TaskCommentsCount>(storedProc);
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
