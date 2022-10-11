using System.Windows;
using System.Windows.Controls;

namespace BinaryStudio.PlatformUI.Controls
    {
    public class DataGridOptions
        {
        #region P:DataGridOptions.ColumnHeaderTemplateSelector:DataTemplateSelector
        public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.RegisterAttached("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridOptions), new PropertyMetadata(default(DataTemplateSelector)));
        public static void SetColumnHeaderTemplateSelector(DependencyObject element, DataTemplateSelector value)
            {
            element.SetValue(ColumnHeaderTemplateSelectorProperty, value);
            }

        public static DataTemplateSelector GetColumnHeaderTemplateSelector(DependencyObject element)
            {
            return (DataTemplateSelector)element.GetValue(ColumnHeaderTemplateSelectorProperty);
            }
        #endregion
        }
    }