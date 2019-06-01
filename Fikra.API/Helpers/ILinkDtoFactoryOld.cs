using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Fikra.API.Helpers
{
    public interface ILinkDtoFactoryOld
    {
	    LinkDto CreateLink(IUrlHelper urlHelper, string linkName, ExpandoObject linkParams, string actionMethod);
    }
}
