﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BinaryStudio.PlatformUI.Media;

namespace BinaryStudio.PlatformUI
    {
    public class HSLColorConverter : TypeConverter,IValueConverter
        {
        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
            if (value is Color rgb) {
                return Convert(new HSLColor(rgb), targetType, parameter, culture);
                }
            return value;
            }

        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        Object IValueConverter.ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) {
            return value;
            }
        }
    }