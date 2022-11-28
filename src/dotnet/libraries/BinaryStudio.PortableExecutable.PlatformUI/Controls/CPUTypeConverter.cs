using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using BinaryStudio.PortableExecutable.Win32;

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
        Object IValueConverter.Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
            if (value == null) { return null; }
            if (value is CV_CPU_TYPE CPU) {
                switch (CPU) {
                    case CV_CPU_TYPE.CPU_80286:
                        {
                        var r = new Paragraph();
                        r.Inlines.Add(new Run("Intel® 286 Processor"));
                        return r;
                        }
                    }
                }
            var type = value.GetType();
            if (type.IsEnum) {
                var display = (DisplayAttribute)type.GetField(value.ToString())?.GetCustomAttributes(typeof(DisplayAttribute), false)?.FirstOrDefault();
                if (display != null) {
                    return !String.IsNullOrWhiteSpace(display.Name)
                        ? display.Name
                        : value.ToString();
                    }
                }
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