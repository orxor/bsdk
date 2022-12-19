using System;
using System.ComponentModel;
using System.Globalization;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation.Converters
    {
    public class Asn1DateTimeConverter : TypeConverter
        {
        #region M:GetPropertiesSupported(ITypeDescriptorContext):Boolean
        /**
         * <summary>Returns whether this object supports properties, using the specified context.</summary>
         * <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
         * <returns>true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"/> should be called to find the properties of this object; otherwise, false.</returns>
         * */
        public override Boolean GetPropertiesSupported(ITypeDescriptorContext context)
            {
            return false;
            }
        #endregion
        #region M:ToString(DateTime):String
        public static String ToString(DateTime value) {
            return (value).ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss");
            }
        #endregion
        #region M:ToString(DateTime,CultureInfo,String):String
        public static String ToString(DateTime value, CultureInfo culture, String format) {
            if (format != null) {
                if (format == "X509") { return ToString(value, culture, $"{culture.DateTimeFormat.FullDateTimePattern} HH:mm:ss K"); }
                if (format == "r") {
                    var r = value;
                    if (value.Kind != DateTimeKind.Utc) { r = r.ToUniversalTime(); }
                    return r.ToString(format, culture);
                    }
                }
            return (value).ToLocalTime().ToString(format, culture);
            }
        #endregion
        #region M:ToString(Object,CultureInfo,String):String
        private static String ToString(Object value, CultureInfo culture, String format) {
            if (value == null) { return null; }
            if (value is DateTime) { return ToString((DateTime)value, culture, format); }
            return value.ToString();
            }
        #endregion
        #region M:ConvertTo(ITypeDescriptorContext,CultureInfo,Object,Type):Object
        public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType) {
            if (value is DateTime) {
                if (destinationType == typeof(String)) {
                    return ToString((DateTime)value);
                    }
                }
            return base.ConvertTo(context, culture, value, destinationType);
            }
        #endregion
        }
    }
