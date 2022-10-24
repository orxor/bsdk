using System;
using System.Reflection;
using System.Windows;

namespace BinaryStudio.PlatformUI
    {
    public class ThemeResourceKey : ResourceKey, IEquatable<ThemeResourceKey>
        {
        public Guid Category { get; }
        public String Name { get; }

        public ThemeResourceKey(Guid category, String name)
            {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            Category = category;
            Name = name;
            }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>/// <see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <see langword="false"/>.</returns>
        public Boolean Equals(ThemeResourceKey other)
            {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true;  }
            return Category.Equals(other.Category) && String.Equals(Name, other.Name);
            }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public override Boolean Equals(Object other)
            {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true;  }
            return other.GetType() == GetType() && Equals((ThemeResourceKey)other);
            }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override Int32 GetHashCode() {
            unchecked
                {
                return (Category.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                }
            }

        /// <summary>Gets an assembly object that indicates which assembly's dictionary to look in for the value associated with this key.</summary>
        /// <returns>The retrieved assembly, as a reflection class.</returns>
        public override Assembly Assembly { get{
            return GetType().Assembly;
            }}

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return Name;
            }

        /// <summary>Returns this <see cref="T:System.Windows.ResourceKey"/>. Instances of this class are typically used as a key in a dictionary.</summary>
        /// <param name="serviceProvider">A service implementation that provides the desired value.</param>
        /// <returns>Calling this method always returns the instance itself.</returns>
        public override Object ProvideValue(IServiceProvider serviceProvider)
            {
            var r = base.ProvideValue(serviceProvider);
            return r;
            }
        }
    }