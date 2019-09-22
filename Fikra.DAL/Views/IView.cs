using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.Views
{
    public interface IView
    {
        string Name { get; }
		string Body { get; }
    }
}
