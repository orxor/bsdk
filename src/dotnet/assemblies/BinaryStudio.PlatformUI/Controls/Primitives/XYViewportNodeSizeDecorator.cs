using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    public class XYViewportNodeSizeDecorator : Control
        {
        static XYViewportNodeSizeDecorator()
            {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XYViewportNodeSizeDecorator), new FrameworkPropertyMetadata(typeof(XYViewportNodeSizeDecorator)));
            }

        public override void OnApplyTemplate()
            {
            base.OnApplyTemplate();
            InstallThumbHandlers(ThumbRight       = GetTemplateChild("ThumbRight")       as Thumb);
            InstallThumbHandlers(ThumbBottom      = GetTemplateChild("ThumbBottom")      as Thumb);
            InstallThumbHandlers(ThumbRightBottom = GetTemplateChild("ThumbRightBottom") as Thumb);
            InstallThumbHandlers(ThumbTop         = GetTemplateChild("ThumbTop")         as Thumb);
            InstallThumbHandlers(ThumbLeft        = GetTemplateChild("ThumbLeft")        as Thumb);
            InstallThumbHandlers(ThumbRightTop    = GetTemplateChild("ThumbRightTop")    as Thumb);
            InstallThumbHandlers(ThumbLeftBottom  = GetTemplateChild("ThumbLeftBottom")  as Thumb);
            InstallThumbHandlers(ThumbLeftTop     = GetTemplateChild("ThumbLeftTop")     as Thumb);
            InstallThumbHandlers(ThumbAll         = GetTemplateChild("ThumbAll")         as Thumb);
            }

        private void InstallThumbHandlers(Thumb source) {
            if (source != null) {
                source.DragStarted   += OnDragStarted;
                source.DragCompleted += OnDragCompleted;
                source.DragDelta     += OnDragDelta;
                }
            }

        #region M:OnDragStarted(Object,DragStartedEventArgs)
        private void OnDragStarted(Object sender, DragStartedEventArgs e) {
            }
        #endregion
        #region M:OnDragCompleted(Object,DragCompletedEventArgs)
        private void OnDragCompleted(Object sender, DragCompletedEventArgs e) {
            }
        #endregion
        #region M:OnDragDelta(Object,DragDeltaEventArgs)
        private void OnDragDelta(Object sender, DragDeltaEventArgs e) {
            }
        #endregion

        private Thumb ThumbBottom;
        private Thumb ThumbLeft;
        private Thumb ThumbLeftBottom;
        private Thumb ThumbLeftTop;
        private Thumb ThumbRight;
        private Thumb ThumbRightBottom;
        private Thumb ThumbRightTop;
        private Thumb ThumbTop;
        private Thumb ThumbAll;
        }
    }
