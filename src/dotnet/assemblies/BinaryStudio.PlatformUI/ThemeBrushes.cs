using System;
using System.Runtime.CompilerServices;

namespace BinaryStudio.PlatformUI
    {
    public partial class Theme
        {
        //public class Colors
        //    {
            public static Object ActiveBorderBrushKey            { get { return CreateInstance(ref FieldActiveBorderBrushKey                  ,SystemColorCategory,nameof(ActiveBorderBrushKey));            }}
            public static Object ActiveCaptionBrushKey           { get { return CreateInstance(ref FieldActiveCaptionBrushKey                 ,SystemColorCategory,nameof(ActiveCaptionBrushKey));           }}
            public static Object ActiveCaptionTextBrushKey       { get { return CreateInstance(ref FieldActiveCaptionTextBrushKey             ,SystemColorCategory,nameof(ActiveCaptionTextBrushKey));       }}
            public static Object AppWorkspaceBrushKey            { get { return CreateInstance(ref FieldAppWorkspaceBrushKey                  ,SystemColorCategory,nameof(AppWorkspaceBrushKey));            }}
            public static Object ControlBrushKey                 { get { return CreateInstance(ref FieldControlBrushKey                       ,SystemColorCategory,nameof(ControlBrushKey));                 }}
            public static Object ControlDarkBrushKey             { get { return CreateInstance(ref FieldControlDarkBrushKey                   ,SystemColorCategory,nameof(ControlDarkBrushKey));             }}
            public static Object ControlDarkDarkBrushKey         { get { return CreateInstance(ref FieldControlDarkDarkBrushKey               ,SystemColorCategory,nameof(ControlDarkDarkBrushKey));         }}
            public static Object ControlLightBrushKey            { get { return CreateInstance(ref FieldControlLightBrushKey                  ,SystemColorCategory,nameof(ControlLightBrushKey));            }}
            public static Object ControlLightLightBrushKey       { get { return CreateInstance(ref FieldControlLightLightBrushKey             ,SystemColorCategory,nameof(ControlLightLightBrushKey));       }}
            public static Object ControlTextBrushKey             { get { return CreateInstance(ref FieldControlTextBrushKey                   ,SystemColorCategory,nameof(ControlTextBrushKey));             }}
            public static Object DesktopBrushKey                 { get { return CreateInstance(ref FieldDesktopBrushKey                       ,SystemColorCategory,nameof(DesktopBrushKey));                 }}
            public static Object GradientActiveCaptionBrushKey   { get { return CreateInstance(ref FieldGradientActiveCaptionBrushKey         ,SystemColorCategory,nameof(GradientActiveCaptionBrushKey));   }}
            public static Object GradientInactiveCaptionBrushKey { get { return CreateInstance(ref FieldGradientInactiveCaptionBrushKey       ,SystemColorCategory,nameof(GradientInactiveCaptionBrushKey)); }}
            public static Object GrayTextBrushKey                { get { return CreateInstance(ref FieldGrayTextBrushKey                      ,SystemColorCategory,nameof(GrayTextBrushKey));                }}
            public static Object HighlightBrushKey               { get { return CreateInstance(ref FieldHighlightBrushKey                     ,SystemColorCategory,nameof(HighlightBrushKey));               }}
            public static Object HighlightLightBrushKey          { get { return CreateInstance(ref FieldHighlightLightBrushKey                ,SystemColorCategory,nameof(HighlightLightBrushKey));          }}
            public static Object HighlightTextBrushKey           { get { return CreateInstance(ref FieldHighlightTextBrushKey                 ,SystemColorCategory,nameof(HighlightTextBrushKey));           }}
            public static Object HighlightLightTextBrushKey      { get { return CreateInstance(ref FieldHighlightLightTextBrushKey            ,SystemColorCategory,nameof(HighlightLightTextBrushKey));      }}
            public static Object HotTrackBrushKey                { get { return CreateInstance(ref FieldHotTrackBrushKey                      ,SystemColorCategory,nameof(HotTrackBrushKey));                }}
            public static Object InactiveBorderBrushKey          { get { return CreateInstance(ref FieldInactiveBorderBrushKey                ,SystemColorCategory,nameof(InactiveBorderBrushKey));          }}
            public static Object InactiveCaptionBrushKey         { get { return CreateInstance(ref FieldInactiveCaptionBrushKey               ,SystemColorCategory,nameof(InactiveCaptionBrushKey));         }}
            public static Object InactiveCaptionTextBrushKey     { get { return CreateInstance(ref FieldInactiveCaptionTextBrushKey           ,SystemColorCategory,nameof(InactiveCaptionTextBrushKey));     }}
            public static Object InfoBrushKey                    { get { return CreateInstance(ref FieldInfoBrushKey                          ,SystemColorCategory,nameof(InfoBrushKey));                    }}
            public static Object InfoTextBrushKey                { get { return CreateInstance(ref FieldInfoTextBrushKey                      ,SystemColorCategory,nameof(InfoTextBrushKey));                }}
            public static Object MenuBrushKey                    { get { return CreateInstance(ref FieldMenuBrushKey                          ,SystemColorCategory,nameof(MenuBrushKey));                    }}
            public static Object MenuBarBrushKey                 { get { return CreateInstance(ref FieldMenuBarBrushKey                       ,SystemColorCategory,nameof(MenuBarBrushKey));                 }}
            public static Object MenuHighlightBrushKey           { get { return CreateInstance(ref FieldMenuHighlightBrushKey                 ,SystemColorCategory,nameof(MenuHighlightBrushKey));           }}
            public static Object MenuTextBrushKey                { get { return CreateInstance(ref FieldMenuTextBrushKey                      ,SystemColorCategory,nameof(MenuTextBrushKey));                }}
            public static Object ScrollBarBrushKey               { get { return CreateInstance(ref FieldScrollBarBrushKey                     ,SystemColorCategory,nameof(ScrollBarBrushKey));               }}
            public static Object WindowBrushKey                  { get { return CreateInstance(ref FieldWindowBrushKey                        ,SystemColorCategory,nameof(WindowBrushKey));                  }}
            public static Object WindowFrameBrushKey             { get { return CreateInstance(ref FieldWindowFrameBrushKey                   ,SystemColorCategory,nameof(WindowFrameBrushKey));             }}
            public static Object WindowTextBrushKey              { get { return CreateInstance(ref FieldWindowTextBrushKey                    ,SystemColorCategory,nameof(WindowTextBrushKey));              }}
            public static Object SelectionBrushKey               { get { return CreateInstance(ref FieldSelectionBrushKey                     ,SystemColorCategory,nameof(SelectionBrushKey));               }}
            public static Object TabItemUnselectedBrushKey               { get { return CreateInstance(ref FieldTabItemUnselectedBrushKey             ,SystemColorCategory,nameof(TabItemUnselectedBrushKey));               }}
            public static Object TabItemUnselectedTextBrushKey           { get { return CreateInstance(ref FieldTabItemUnselectedTextBrushKey         ,SystemColorCategory,nameof(TabItemUnselectedTextBrushKey));           }}
            public static Object TabItemUnselectedMouseOverBrushKey      { get { return CreateInstance(ref FieldTabItemUnselectedMouseOverBrushKey    ,SystemColorCategory,nameof(TabItemUnselectedMouseOverBrushKey));      }}
            public static Object TabItemUnselectedMouseOverTextBrushKey  { get { return CreateInstance(ref FieldTabItemUnselectedMouseOverTextBrushKey,SystemColorCategory,nameof(TabItemUnselectedMouseOverTextBrushKey));  }}
            public static Object TabItemSelectedBrushKey                 { get { return CreateInstance(ref FieldTabItemSelectedBrushKey               ,SystemColorCategory,nameof(TabItemSelectedBrushKey));                 }}
            public static Object TabItemSelectedTextBrushKey             { get { return CreateInstance(ref FieldTabItemSelectedTextBrushKey           ,SystemColorCategory,nameof(TabItemSelectedTextBrushKey));             }}
            public static Object TabItemSelectedMouseOverBrushKey        { get { return CreateInstance(ref FieldTabItemSelectedMouseOverBrushKey      ,SystemColorCategory,nameof(TabItemSelectedMouseOverBrushKey));        }}
            public static Object TabItemSelectedMouseOverTextBrushKey    { get { return CreateInstance(ref FieldTabItemSelectedMouseOverTextBrushKey  ,SystemColorCategory,nameof(TabItemSelectedMouseOverTextBrushKey));    }}
            public static Object InactiveSelectionHighlightBrushKey      { get { return CreateInstance(ref FieldInactiveSelectionHighlightBrushKey    ,SystemColorCategory,nameof(InactiveSelectionHighlightBrushKey));      }}
            public static Object InactiveSelectionHighlightTextBrushKey  { get { return CreateInstance(ref FieldInactiveSelectionHighlightTextBrushKey,SystemColorCategory,nameof(InactiveSelectionHighlightTextBrushKey));  }}
            public static Object DataGridColumnHeaderBackgroundBrushKey  { get { return CreateInstance(ref FieldDataGridColumnHeaderBackgroundBrushKey,SystemColorCategory,nameof(DataGridColumnHeaderBackgroundBrushKey));  }}

            private static Object FieldActiveBorderBrushKey;
            private static Object FieldActiveCaptionBrushKey;
            private static Object FieldActiveCaptionTextBrushKey;
            private static Object FieldAppWorkspaceBrushKey;
            private static Object FieldControlBrushKey;
            private static Object FieldControlDarkBrushKey;
            private static Object FieldControlDarkDarkBrushKey;
            private static Object FieldControlLightBrushKey;
            private static Object FieldControlLightLightBrushKey;
            private static Object FieldControlTextBrushKey;
            private static Object FieldDesktopBrushKey;
            private static Object FieldGradientActiveCaptionBrushKey;
            private static Object FieldGradientInactiveCaptionBrushKey;
            private static Object FieldGrayTextBrushKey;
            private static Object FieldHighlightBrushKey;
            private static Object FieldHighlightLightBrushKey;
            private static Object FieldHighlightTextBrushKey;
            private static Object FieldHighlightLightTextBrushKey;
            private static Object FieldHotTrackBrushKey;
            private static Object FieldInactiveBorderBrushKey;
            private static Object FieldInactiveCaptionBrushKey;
            private static Object FieldInactiveCaptionTextBrushKey;
            private static Object FieldInfoBrushKey;
            private static Object FieldInfoTextBrushKey;
            private static Object FieldMenuBrushKey;
            private static Object FieldMenuBarBrushKey;
            private static Object FieldMenuHighlightBrushKey;
            private static Object FieldMenuTextBrushKey;
            private static Object FieldScrollBarBrushKey;
            private static Object FieldWindowBrushKey;
            private static Object FieldWindowFrameBrushKey;
            private static Object FieldWindowTextBrushKey;
            private static Object FieldSelectionBrushKey;
            private static Object FieldTabItemUnselectedBrushKey;
            private static Object FieldTabItemUnselectedTextBrushKey;
            private static Object FieldTabItemUnselectedMouseOverBrushKey;
            private static Object FieldTabItemUnselectedMouseOverTextBrushKey;
            private static Object FieldTabItemSelectedBrushKey;
            private static Object FieldTabItemSelectedTextBrushKey;
            private static Object FieldTabItemSelectedMouseOverBrushKey;
            private static Object FieldTabItemSelectedMouseOverTextBrushKey;
            private static Object FieldInactiveSelectionHighlightBrushKey;
            private static Object FieldInactiveSelectionHighlightTextBrushKey;
            private static Object FieldDataGridColumnHeaderBackgroundBrushKey;
            //}

        #region M:CreateInstance({ref}Object,Guid,String):Object
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Object CreateInstance(ref Object o, Guid category, String name)
            {
            return o ?? (o = CreateInstance(category, name));
            }
        #endregion
        #region M:CreateInstance(Guid,String):Object
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Object CreateInstance(Guid category, String name)
            {
            //return $"{category:B}:{{{name}}}";
            return name;
            }
        #endregion
        }
    }