using System;

namespace Fikra.Model.Interfaces
{
    public interface IEntity<T> : ITrackableEntity where T : IEquatable<T>
    {
        T Id { get; set; }
    }
}
