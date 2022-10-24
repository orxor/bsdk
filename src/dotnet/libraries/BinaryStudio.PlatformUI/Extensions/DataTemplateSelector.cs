using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace BinaryStudio.PlatformUI
    {
    using UIDataTemplateSelector = System.Windows.Controls.DataTemplateSelector;
    [ContentProperty(nameof(Templates))]
    [DefaultProperty(nameof(Templates))]
    public class DataTemplateSelector : UIDataTemplateSelector
        {
        public ObservableCollection<DataTemplate> Templates { get; }
        public DataTemplate DefaultTemplate { get;set; }

        public DataTemplateSelector() {
            Templates = new ObservableCollection<DataTemplate>();
            }

        /// <summary>When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"/> based on custom logic.</summary>
        /// <returns>Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.</returns>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        public override DataTemplate SelectTemplate(Object item, DependencyObject container) {
            if (item != null) {
                foreach (var template in Templates) {
                    if (template.DataType is Type) {
                        var type = (Type)template.DataType;
                        if (type == item.GetType()) { return template; }
                        }
                    }
                }
            return DefaultTemplate ?? base.SelectTemplate(item, container);
            }
        }
    }