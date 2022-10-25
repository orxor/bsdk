using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BinaryStudio.PlatformComponents
    {
    public class EmptyArray<T>
        {
        public static readonly T[] Value = new T[0];
        }

    public class EmptyList<T>
        {
        public static readonly IList<T> Value = new ReadOnlyCollection<T>(EmptyArray<T>.Value);
        }
    }