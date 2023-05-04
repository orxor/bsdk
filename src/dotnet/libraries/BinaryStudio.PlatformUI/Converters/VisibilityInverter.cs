using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BinaryStudio.PlatformUI.Converters
    {
    public class VisibilityInverter : IValueConverter
        {
        public Visibility Invisible { get;set; }
        /// <summary>Converts a value. </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        Object IValueConverter.Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
            if (value is Visibility visibility) {
                switch (visibility) {
                    case Visibility.Visible:   return Invisible;
                    case Visibility.Hidden:    return Visibility.Visible;
                    case Visibility.Collapsed: return Visibility.Visible;
                    default: throw new ArgumentOutOfRangeException();
                    }
                }
            return value;
            }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        Object IValueConverter.ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) {
            if (value is Visibility visibility) {
                switch (visibility) {
                    case Visibility.Visible:   return Invisible;
                    case Visibility.Hidden:    return Visibility.Visible;
                    case Visibility.Collapsed: return Visibility.Visible;
                    default: throw new ArgumentOutOfRangeException();
                    }
                }
            return value;
            }

        public VisibilityInverter()
            {
            Invisible = Visibility.Hidden;
            }
        }
    }