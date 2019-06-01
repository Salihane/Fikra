using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fikra.Model.Interfaces;

namespace Fikra.DAL.Interfaces
{
    public interface IFakeRepository<T, K> : IRepository<T, K> where T : IEntity<K> where K : IEquatable<K>
	{
        
    }
}
