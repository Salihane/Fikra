using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.Views.Interfaces
{
    public interface IViewBuilder<T> where T : IView, new()
    {
        
    }
}
