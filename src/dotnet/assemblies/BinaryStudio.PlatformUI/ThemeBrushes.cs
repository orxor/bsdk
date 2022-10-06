using System;
using System.Runtime.CompilerServices;

namespace BinaryStudio.PlatformUI
    {
    public partial class Theme
        {
        public static Object ActiveBorderBrushResourceKey            { get { return CreateInstance(ref FieldActiveBorderBrushKey           ,SystemColorCategory,nameof(ActiveBorderBrushResourceKey));            }}
        public static Object ActiveCaptionBrushResourceKey           { get { return CreateInstance(ref FieldActiveCaptionBrushKey          ,SystemColorCategory,nameof(ActiveCaptionBrushResourceKey));           }}
        public static Object ActiveCaptionTextBrushResourceKey       { get { return CreateInstance(ref FieldActiveCaptionTextBrushKey      ,SystemColorCategory,nameof(ActiveCaptionTextBrushResourceKey));       }}
        public static Object AppWorkspaceBrushResourceKey            { get { return CreateInstance(ref FieldAppWorkspaceBrushKey           ,SystemColorCategory,nameof(AppWorkspaceBrushResourceKey));            }}
        public static Object ControlBrushResourceKey                 { get { return CreateInstance(ref FieldControlBrushKey                ,SystemColorCategory,nameof(ControlBrushResourceKey));                 }}
        public static Object ControlDarkBrushResourceKey             { get { return CreateInstance(ref FieldControlDarkBrushKey            ,SystemColorCategory,nameof(ControlDarkBrushResourceKey));             }}
        public static Object ControlDarkDarkBrushResourceKey         { get { return CreateInstance(ref FieldControlDarkDarkBrushKey        ,SystemColorCategory,nameof(ControlDarkDarkBrushResourceKey));         }}
        public static Object ControlLightBrushResourceKey            { get { return CreateInstance(ref FieldControlLightBrushKey           ,SystemColorCategory,nameof(ControlLightBrushResourceKey));            }}
        public static Object ControlLightLightBrushResourceKey       { get { return CreateInstance(ref FieldControlLightLightBrushKey      ,SystemColorCategory,nameof(ControlLightLightBrushResourceKey));       }}
        public static Object ControlTextBrushResourceKey             { get { return CreateInstance(ref FieldControlTextBrushKey            ,SystemColorCategory,nameof(ControlTextBrushResourceKey));             }}
        public static Object DesktopBrushResourceKey                 { get { return CreateInstance(ref FieldDesktopBrushKey                ,SystemColorCategory,nameof(DesktopBrushResourceKey));                 }}
        public static Object GradientActiveCaptionBrushResourceKey   { get { return CreateInstance(ref FieldGradientActiveCaptionBrushKey  ,SystemColorCategory,nameof(GradientActiveCaptionBrushResourceKey));   }}
        public static Object GradientInactiveCaptionBrushResourceKey { get { return CreateInstance(ref FieldGradientInactiveCaptionBrushKey,SystemColorCategory,nameof(GradientInactiveCaptionBrushResourceKey)); }}
        public static Object GrayTextBrushResourceKey                { get { return CreateInstance(ref FieldGrayTextBrushKey               ,SystemColorCategory,nameof(GrayTextBrushResourceKey));                }}
        public static Object HighlightBrushResourceKey               { get { return CreateInstance(ref FieldHighlightBrushKey              ,SystemColorCategory,nameof(HighlightBrushResourceKey));               }}
        public static Object HighlightTextBrushResourceKey           { get { return CreateInstance(ref FieldHighlightTextBrushKey          ,SystemColorCategory,nameof(HighlightTextBrushResourceKey));           }}
        public static Object HotTrackBrushResourceKey                { get { return CreateInstance(ref FieldHotTrackBrushKey               ,SystemColorCategory,nameof(HotTrackBrushResourceKey));                }}
        public static Object InactiveBorderBrushResourceKey          { get { return CreateInstance(ref FieldInactiveBorderBrushKey         ,SystemColorCategory,nameof(InactiveBorderBrushResourceKey));          }}
        public static Object InactiveCaptionBrushResourceKey         { get { return CreateInstance(ref FieldInactiveCaptionBrushKey        ,SystemColorCategory,nameof(InactiveCaptionBrushResourceKey));         }}
        public static Object InactiveCaptionTextBrushResourceKey     { get { return CreateInstance(ref FieldInactiveCaptionTextBrushKey    ,SystemColorCategory,nameof(InactiveCaptionTextBrushResourceKey));     }}
        public static Object InfoBrushResourceKey                    { get { return CreateInstance(ref FieldInfoBrushKey                   ,SystemColorCategory,nameof(InfoBrushResourceKey));                    }}
        public static Object InfoTextBrushResourceKey                { get { return CreateInstance(ref FieldInfoTextBrushKey               ,SystemColorCategory,nameof(InfoTextBrushResourceKey));                }}
        public static Object MenuBrushResourceKey                    { get { return CreateInstance(ref FieldMenuBrushKey                   ,SystemColorCategory,nameof(MenuBrushResourceKey));                    }}
        public static Object MenuBarBrushResourceKey                 { get { return CreateInstance(ref FieldMenuBarBrushKey                ,SystemColorCategory,nameof(MenuBarBrushResourceKey));                 }}
        public static Object MenuHighlightBrushResourceKey           { get { return CreateInstance(ref FieldMenuHighlightBrushKey          ,SystemColorCategory,nameof(MenuHighlightBrushResourceKey));           }}
        public static Object MenuTextBrushResourceKey                { get { return CreateInstance(ref FieldMenuTextBrushKey               ,SystemColorCategory,nameof(MenuTextBrushResourceKey));                }}
        public static Object ScrollBarBrushResourceKey               { get { return CreateInstance(ref FieldScrollBarBrushKey              ,SystemColorCategory,nameof(ScrollBarBrushResourceKey));               }}
        public static Object WindowBrushResourceKey                  { get { return CreateInstance(ref FieldWindowBrushKey                 ,SystemColorCategory,nameof(WindowBrushResourceKey));                  }}
        public static Object WindowFrameBrushResourceKey             { get { return CreateInstance(ref FieldWindowFrameBrushKey            ,SystemColorCategory,nameof(WindowFrameBrushResourceKey));             }}
        public static Object WindowTextBrushResourceKey              { get { return CreateInstance(ref FieldWindowTextBrushKey             ,SystemColorCategory,nameof(WindowTextBrushResourceKey));              }}

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
        private static Object FieldHighlightTextBrushKey;
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