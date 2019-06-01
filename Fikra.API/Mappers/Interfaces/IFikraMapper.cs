using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fikra.API.Mappers.Interfaces
{
	public interface IFikraMapper<TM, TV> where TM : class where TV : class
	{
		Task<TM> ToModelAsync(TV source);
		Task<IEnumerable<TM>> ToModelAsync(IReadOnlyCollection<TV> sourceCollection);
		Task<TV> ToViewModelAsync(TM source);
		Task<IEnumerable<TV>> ToViewModelAsync(IReadOnlyCollection<TM> sourceCollection);

	}
}
