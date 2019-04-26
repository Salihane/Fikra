using System;

namespace Fikra.Model
{
    public interface IEntity<T> where T : IEquatable<T>
    {
        T Id { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }
    }
}
