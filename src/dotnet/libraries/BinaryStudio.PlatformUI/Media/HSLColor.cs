using System;
using System.Diagnostics;
using System.Windows.Media;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI.Media
    {
    public struct HSLColor : IEquatable<HSLColor>,IFormattable
        {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Single h,s,l;

        public Single ScH
            {
            get { return h; }
            set { h = Math.Min(Math.Max(value, 0.0f),1.0f); }
            }

        public Single ScS
            {
            get { return s; }
            set { s = Math.Min(Math.Max(value, 0.0f),1.0f); }
            }

        public Single ScL
            {
            get { return l; }
            set { l = Math.Min(Math.Max(value, 0.0f),1.0f); }
            }

        public HSLColor(HSVColor value) {
            h = value.ScH;
            l = value.ScV*(1-value.ScS*0.5f);
            s = (FloatUtil.AreClose(l,0) || FloatUtil.AreClose(l,1)) ? 0 : ((value.ScV - l)/Math.Min(l,1-l));
            }

        public HSLColor(Color value) {
            var α = Math.Max(value.R,Math.Max(value.G,value.B));
            var β = Math.Min(value.R,Math.Min(value.G,value.B));
            var δ = α - β;
            h = 0.0f;
            if (δ != 0) {
                h = ((α == value.R)
                    ? (value.G >= value.B)
                        ? (((value.G - value.B)/(Single)δ) + 0f)
                        : (((value.G - value.B)/(Single)δ) + 6f)
                    : (α == value.G)
                        ? (((value.B - value.R)/(Single)δ) + 2f)
                        : (((value.R - value.G)/(Single)δ) + 4f))/6f;
                }
            l = ((α + β)*0.5f)/255f;
            s = ((α + β) == 0) ? 0.0f : (Single)δ/(255f - Math.Abs(255f - (α + β)));
            }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public Boolean Equals(HSLColor other)
            {
            return h.Equals(other.h) && s.Equals(other.s) && l.Equals(other.l);
            }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="other"/> and this instance are the same type and represent the same value; otherwise, false.</returns>
        /// <param name="other">Another object to compare to.</param>
        /// <filterpriority>2</filterpriority>
        public override Boolean Equals(Object other)
            {
            return (other is HSLColor color) && Equals(color);
            }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        /// <filterpriority>2</filterpriority>
        public override Int32 GetHashCode() {
            unchecked
                {
                var r = h.GetHashCode();
                r = (r * 397) ^ s.GetHashCode();
                r = (r * 397) ^ l.GetHashCode();
                return r;
                }
            }

        /// <summary>Formats the value of the current instance using the specified format.</summary>
        /// <returns>The value of the current instance in the specified format.</returns>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation. </param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system. </param>
        /// <filterpriority>2</filterpriority>
        public String ToString(String format, IFormatProvider formatProvider)
            {
            return String.Format(formatProvider,"{0:F2}°,{1:F2}%,{2:F2}%", h*360, s*100, l*100);
            }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="T:System.String"/> containing a fully qualified type name.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString() {
            return String.Format("{0:F2}°,{1:F2}%,{2:F2}%", h*360, s*100, l*100);
            }

        private static float Hue2C(float p, float q, float t) {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            return (t < 1f/6f) ? (p + ((q - p)*(t)*6f)) : (t < 0.5f) ? q :
                   (t < 2f/3f) ? (p + ((q - p)*(2f/3f - t)*6f)) : p;
            }

        public static explicit operator Color(HSLColor value) {
            float q = value.l < 0.5 ? value.l * (1 + value.s) : value.l + value.s - value.l * value.s;
            float p = 2 * value.l - q;
            var r = new Color{
                ScR = Hue2C(p,q,value.h + 1f/3f),
                ScG = Hue2C(p,q,value.h),
                ScB = Hue2C(p,q,value.h - 1f/3f)
                };
            r.A = 0xff;
            return r;
            }
        }
    }