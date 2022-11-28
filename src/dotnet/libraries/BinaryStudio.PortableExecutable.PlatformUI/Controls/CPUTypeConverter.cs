﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace BinaryStudio.PortableExecutable.PlatformUI.Controls
    {
    internal class CPUTypeConverter : IValueConverter
        {
        /// <summary>Converts a value.</summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        Object IValueConverter.Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
            {
            if (value == null) { return null; }
            return value;
            }

        /// <summary>Converts a value.</summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        Object IValueConverter.ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
            {
            throw new NotImplementedException();
            }
        }
    }