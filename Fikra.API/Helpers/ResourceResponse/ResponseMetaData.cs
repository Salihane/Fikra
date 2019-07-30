using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Fikra.API.Helpers.ResourceResponse
{
  public class ResponseMetaData
  {
    public int ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
    public Dictionary<string, StringValues> ResponseHeaders { get; set; } = new Dictionary<string, StringValues>();
  }
}
