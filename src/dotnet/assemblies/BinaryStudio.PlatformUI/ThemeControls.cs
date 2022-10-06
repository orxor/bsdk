using System;

namespace BinaryStudio.PlatformUI
    {
    public partial class Theme
        {
        public static Object TabControlStyleKey { get { return FieldTabControlStyleKey = FieldTabControlStyleKey ?? new ThemeResourceKey(ControlStyleCategory,nameof(TabControlStyleKey)); }}
        public static Object TabItemStyleKey    { get { return FieldTabItemStyleKey    = FieldTabItemStyleKey    ?? new ThemeResourceKey(ControlStyleCategory,nameof(TabItemStyleKey));    }}

        private static ThemeResourceKey FieldTabControlStyleKey;
        private static ThemeResourceKey FieldTabItemStyleKey;
        }
    }