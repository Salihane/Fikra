using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fikra.Common.Extensions
{
    public static class QueryCollectionExtensions
    {
      public static Dictionary<string, string> ToDictionary(this IQueryCollection source)
      {
        var dictionary = new Dictionary<string, string>();
        foreach (var key in source.Keys)
        {
          source.TryGetValue(key, out var value);
          dictionary.Add(key, value);
        }

        return dictionary;
      }
    }
}
