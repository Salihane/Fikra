using Fikra.DAL.Interfaces;

namespace Fikra.DAL.Views.Interfaces
{
    public interface IView : IDbObject
    {
        string Name { get; }
		string Body { get; }
    }
}
