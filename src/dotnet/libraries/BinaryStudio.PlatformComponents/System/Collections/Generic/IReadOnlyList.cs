#if NET35 || NET40
namespace System.Collections.Generic
    {
    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>
        {
        T this[Int32 index] { get; }
        }
    }
#endif