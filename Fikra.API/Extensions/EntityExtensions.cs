using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Model.Interfaces;

namespace Fikra.API.Extensions
{
    public static class EntityExtensions
    {
	    public static void JustCreated<T>(this IEntity<T> entity) where T : IEquatable<T>
	    {
		    var now = DateTime.Now;
		    entity.CreatedOn = now;
		    entity.ModifiedOn = now;
	    }

	}
}
