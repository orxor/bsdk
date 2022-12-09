using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinaryStudio.PlatformUI.Controls
    {
    public class ProgressRing : Control
        {
        static ProgressRing()
            {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
            }

        public ProgressRingTemplateSettings TemplateSettings { get;set; }
        #region P:IsActive:Boolean
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(Boolean), typeof(ProgressRing), new PropertyMetadata(default(Boolean), OnIsActiveChanged));
        private static void OnIsActiveChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var source = (sender as ProgressRing);
            if (source != null) {
                source.OnIsActiveChanged();
                }
            }

        protected void OnIsActiveChanged()
            {
            if (IsActive)
                {
                VisualStateManager.GoToState(this, "Active", true);
                }
            else
                {
                VisualStateManager.GoToState(this, "Inactive", true);
                }
            }

        public Boolean IsActive
            {
            get { return (Boolean)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
            }
        #endregion

        public ProgressRing()
            {
            TemplateSettings = new ProgressRingTemplateSettings();
            }

        public override void OnApplyTemplate()
            {
            base.OnApplyTemplate();
            OnIsActiveChanged();
            }
        }
    }
