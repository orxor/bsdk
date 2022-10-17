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
        public static Object MenuStyleKey                           { get { return CreateInstance(ref FieldMenuStyleKey,ControlStyleCategory,nameof(MenuStyleKey)); }}
        public static Object ContextMenuStyleKey                    { get { return CreateInstance(ref FieldContextMenuStyleKey,ControlStyleCategory,nameof(ContextMenuStyleKey)); }}
        public static Object ScrollViewerStyleKey                   { get { return CreateInstance(ref FieldScrollViewerStyleKey,ControlStyleCategory,nameof(ScrollViewerStyleKey)); }}
        public static Object ScrollBarStyleKey                      { get { return CreateInstance(ref FieldScrollBarStyleKey,ControlStyleCategory,nameof(ScrollBarStyleKey)); }}
        public static Object ListBoxStyleKey                        { get { return CreateInstance(ref FieldListBoxStyleKey,ControlStyleCategory,nameof(ListBoxStyleKey)); }}
        public static Object ListBoxItemStyleKey                    { get { return CreateInstance(ref FieldListBoxItemStyleKey,ControlStyleCategory,nameof(ListBoxItemStyleKey)); }}

        private static Object FieldTabControlStyleKey;
        private static Object FieldTabItemStyleKey;
        private static Object FieldDataGridCellStyleKey;
        private static Object FieldDataGridRowHeaderStyleKey;
        private static Object FieldDataGridCellsPresenterStyleKey;
        private static Object FieldDataGridRowStyleKey;
        private static Object FieldDataGridColumnHeaderStyleKey;
        private static Object FieldDataGridColumnHeadersPresenterStyleKey;
        private static Object FieldDataGridStyleKey;
        private static Object FieldMenuStyleKey;
        private static Object FieldContextMenuStyleKey;
        private static Object FieldScrollViewerStyleKey;
        private static Object FieldScrollBarStyleKey;
        private static Object FieldListBoxStyleKey;
        private static Object FieldListBoxItemStyleKey;
        }
    }