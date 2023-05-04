#if NET35 || NET40
namespace System.Collections.Generic
    {
    public interface IReadOnlyCollection<out T> : IEnumerable<T>
        {
        Int32 Count { get; }
        }
    }
#endif
