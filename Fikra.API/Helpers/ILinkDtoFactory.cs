using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.API.Helpers
{
    public interface ILinkDtoFactory<T, K> where T : class where K : IEquatable<K>
	{
	    LinkDto CreateGetEntityLink(ResourceUriType resourceUriType);
	    IEnumerable<LinkDto> CreateNavigationLinksForEntity(bool hasNext, bool hasPrevious, object linkValues);
	    IEnumerable<LinkDto> CreateCrudLinksForEntity(K entityId, string entityFields);

    }
}
