using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BinaryStudio.PlatformUI.Shell.Controls
    {
    public class AddInt64 : IMultiValueConverter
        {
        private static Int64 ToInt64(Object value) {
            if ((value == null) || (value is DBNull))   { return 0; }
            if (value == DependencyProperty.UnsetValue) { return 0; }
            if (value is Int64 I8) { return I8; }
            if (value is Int32 I4) { return I4; }
            if (value is Int16 I2) { return I2; }
            if (value is UInt32 UI4) { return UI4; }
            if (value is UInt16 UI2) { return UI2; }
            if (value is IConvertible Convertible) {
                return Convertible.ToInt64(null);
                }
            return ToInt64(value.ToString());
            }

        /// <summary>Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.</summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.If the method returns <see langword="null"/>, the valid <see langword="null" /> value is used.A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.</returns>
        Object IMultiValueConverter.Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
            {
            Int64 r = 0;
                 if (values.Length >= 2) { r = ToInt64(values[0]) + ToInt64(values[1]); }
            else if (values.Length == 1) { r = ToInt64(values[0]); }
            return r;
            }

        /// <summary>Converts a binding target value to the source binding values.</summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        Object[] IMultiValueConverter.ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
            {
            throw new NotSupportedException();
            }
        }

    public class SubInt64 : IMultiValueConverter
        {
        private static Int64 ToInt64(Object value) {
            if ((value == null) || (value is DBNull))   { return 0; }
            if (value == DependencyProperty.UnsetValue) { return 0; }
            if (value is Int64 I8) { return I8; }
            if (value is Int32 I4) { return I4; }
            if (value is Int16 I2) { return I2; }
            if (value is UInt32 UI4) { return UI4; }
            if (value is UInt16 UI2) { return UI2; }
            if (value is IConvertible Convertible) {
                return Convertible.ToInt64(null);
                }
            return ToInt64(value.ToString());
            }

        /// <summary>Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.</summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.If the method returns <see langword="null"/>, the valid <see langword="null" /> value is used.A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding" />.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> or the default value.</returns>
        Object IMultiValueConverter.Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
            {
            Int64 r = 0;
                 if (values.Length >= 2) { r = ToInt64(values[0]) - ToInt64(values[1]); }
            else if (values.Length == 1) { r = ToInt64(values[0]); }
            return r;
            }

        /// <summary>Converts a binding target value to the source binding values.</summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        Object[] IMultiValueConverter.ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
            {
            throw new NotSupportedException();
            }
        }
    }