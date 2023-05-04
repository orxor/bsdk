using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BinaryStudio.DataProcessing
    {
    public class PropertyDescriptorComparer : IComparer<PropertyDescriptor>
        {
        /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
        public Int32 Compare(PropertyDescriptor x, PropertyDescriptor y) {
            if (ReferenceEquals(x, y)) { return 0; }
            if (ReferenceEquals(x, null)) { return -1; }
            if (x is IComparable<PropertyDescriptor> e) { return e.CompareTo(y); }
            if (y is null) { return +1; }
            var orderX = (x.Attributes.OfType<OrderAttribute>().FirstOrDefault()?.Order).GetValueOrDefault(0);
            var orderY = (y.Attributes.OfType<OrderAttribute>().FirstOrDefault()?.Order).GetValueOrDefault(0);
            var nameX = x.DisplayName;
            var nameY = y.DisplayName;
            return (orderX == orderY)
                ? nameX.CompareTo(nameY)
                : orderX.CompareTo(orderY);
            }
        }
    }