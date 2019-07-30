using System;
using System.Collections.Generic;
using Fikra.API.Helpers;
using Fikra.API.Models.Task;
using Fikra.Model.Entities;
using Fikra.Model.Entities.Enums;
using Microsoft.AspNet.OData.Query;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Fikra.API.Models.DashboardTask
{
  public class DashboardTaskDto : TaskDto
  {
    public IEnumerable<LinkDto> Links { get; set; }
  }
}
