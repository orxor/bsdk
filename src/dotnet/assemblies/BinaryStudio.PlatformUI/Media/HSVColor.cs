using System;
using System.Diagnostics;
using System.Windows.Media;
using BinaryStudio.PlatformUI.Extensions;

namespace BinaryStudio.PlatformUI.Media
    {
    public struct HSVColor
        {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Single h,s,v;

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

        public Single ScV
            {
            get { return v; }
            set { v = Math.Min(Math.Max(value, 0.0f),1.0f); }
            }

        public HSVColor(Color value):
            this(new HSLColor(value))
            {
            }

        public HSVColor(HSLColor hsl) {
            h = hsl.ScH;
            v = hsl.ScL + hsl.ScS*Math.Min(hsl.ScL,1 - hsl.ScL);
            s = FloatUtil.AreClose(v,0) ? 0 : 2*(1-hsl.ScL/v);
            }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="T:System.String"/> containing a fully qualified type name.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString() {
            return String.Format("{0:F2}°,{1:F2}%,{2:F2}%", h*360, s*100, v*100);
            }

        public static explicit operator HSLColor(HSVColor value) {
            return new HSLColor(value);
            }

        public static explicit operator Color(HSVColor value) {
            return (Color)(new HSLColor(value));
            }
        }
    }