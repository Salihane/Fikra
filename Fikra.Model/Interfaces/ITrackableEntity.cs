using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.Model.Interfaces
{
    public interface ITrackableEntity
	{
		DateTime CreatedOn { get; set; }
		DateTime ModifiedOn { get; set; }
		bool IsDeleted { get; set; }
	}
}
