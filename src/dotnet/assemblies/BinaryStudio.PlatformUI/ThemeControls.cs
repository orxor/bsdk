using System;

namespace BinaryStudio.PlatformUI
    {
    public partial class Theme
        {
        public static Object TabControlStyleKey                     { get { return FieldTabControlStyleKey                     = FieldTabControlStyleKey                     ?? new ThemeResourceKey(ControlStyleCategory,nameof(TabControlStyleKey));                     }}
        public static Object TabItemStyleKey                        { get { return FieldTabItemStyleKey                        = FieldTabItemStyleKey                        ?? new ThemeResourceKey(ControlStyleCategory,nameof(TabItemStyleKey));                        }}
        public static Object DataGridCellStyleKey                   { get { return FieldDataGridCellStyleKey                   = FieldDataGridCellStyleKey                   ?? new ThemeResourceKey(ControlStyleCategory,nameof(DataGridCellStyleKey));                   }}
        public static Object DataGridRowHeaderStyleKey              { get { return FieldDataGridRowHeaderStyleKey              = FieldDataGridRowHeaderStyleKey              ?? new ThemeResourceKey(ControlStyleCategory,nameof(DataGridRowHeaderStyleKey));              }}
        public static Object DataGridCellsPresenterStyleKey         { get { return FieldDataGridCellsPresenterStyleKey         = FieldDataGridCellsPresenterStyleKey         ?? new ThemeResourceKey(ControlStyleCategory,nameof(DataGridCellsPresenterStyleKey));         }}
        public static Object DataGridRowStyleKey                    { get { return FieldDataGridRowStyleKey                    = FieldDataGridRowStyleKey                    ?? new ThemeResourceKey(ControlStyleCategory,nameof(DataGridRowStyleKey));                    }}
        public static Object DataGridColumnHeaderStyleKey           { get { return FieldDataGridColumnHeaderStyleKey           = FieldDataGridColumnHeaderStyleKey           ?? new ThemeResourceKey(ControlStyleCategory,nameof(DataGridColumnHeaderStyleKey));           }}
        public static Object DataGridColumnHeadersPresenterStyleKey { get { return FieldDataGridColumnHeadersPresenterStyleKey = FieldDataGridColumnHeadersPresenterStyleKey ?? new ThemeResourceKey(ControlStyleCategory,nameof(DataGridColumnHeadersPresenterStyleKey)); }}
        public static Object DataGridStyleKey                       { get { return FieldDataGridStyleKey                       = FieldDataGridStyleKey                       ?? new ThemeResourceKey(ControlStyleCategory,nameof(DataGridStyleKey));                       }}

        private static ThemeResourceKey FieldTabControlStyleKey;
        private static ThemeResourceKey FieldTabItemStyleKey;
        private static ThemeResourceKey FieldDataGridCellStyleKey;
        private static ThemeResourceKey FieldDataGridRowHeaderStyleKey;
        private static ThemeResourceKey FieldDataGridCellsPresenterStyleKey;
        private static ThemeResourceKey FieldDataGridRowStyleKey;
        private static ThemeResourceKey FieldDataGridColumnHeaderStyleKey;
        private static ThemeResourceKey FieldDataGridColumnHeadersPresenterStyleKey;
        private static ThemeResourceKey FieldDataGridStyleKey;
        }
    }