using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BinaryStudio.PlatformUI
    {
    public class ColorConverter : TypeConverter,IValueConverter
        {
        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
            if (value is String Text) {
                if ((targetType == typeof(Color)) || (targetType == typeof(Object))) {
                    Text = Text.Trim();
                    if (!String.IsNullOrWhiteSpace(Text)) {
                        if (Text[0] == '#') {
                            Text = Text.Substring(1).Trim();
                            if (UInt64.TryParse($"{Text}",NumberStyles.HexNumber, culture, out var o)) {
                                var a = (Byte)((o & 0xff000000) >> 24);
                                var r = (Byte)((o & 0x00ff0000) >> 16);
                                var g = (Byte)((o & 0x0000ff00) >>  8);
                                var b = (Byte)(o & 0x000000ff);
                                return Color.FromRgb(r,g,b);
                                }
                            }
                        }
                    return Colors.Black;
                    }
                }
            return value;
            }

        public Color Convert(Object value, Object parameter, CultureInfo culture)
            {
            return (Color)Convert(value,typeof(Color),parameter,culture);
            }

        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
            {
            return value;
            }

        public static Color PerformOpacity(Color Target,Color Source, Single Opacity) {
            return Color.FromScRgb(Source.ScA, 
                Math.Min(1.0f, Math.Max(0.0f, Source.ScR * Opacity + Target.ScR * (1.0f - Opacity))),
                Math.Min(1.0f, Math.Max(0.0f, Source.ScG * Opacity + Target.ScG * (1.0f - Opacity))),
                Math.Min(1.0f, Math.Max(0.0f, Source.ScB * Opacity + Target.ScB * (1.0f - Opacity))));
            }

        public static Color PerformOpacity(Color Source, Single opacity) {
            return PerformOpacity(Colors.White,Source,opacity);
            }

        public static Color AdjustBrightness(Color value, Single factor) {
            return new Color{
                A = value.A,
                ScR = Math.Min(Math.Max(value.ScR + factor,0),1),
                ScG = Math.Min(Math.Max(value.ScG + factor,0),1),
                ScB = Math.Min(Math.Max(value.ScB + factor,0),1)
                };
            }
        }
    }