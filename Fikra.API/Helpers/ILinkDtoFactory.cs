using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.API.Helpers
{
    public interface ILinkDtoFactory<T> where T : class
    {
	    LinkDto CreateGetEntityLink(ResourceUriType resourceUriType);
	    IEnumerable<LinkDto> CreateNavigationLinksForEntity(bool hasNext, bool hasPrevious, params object[] linkValues);

    }
}
