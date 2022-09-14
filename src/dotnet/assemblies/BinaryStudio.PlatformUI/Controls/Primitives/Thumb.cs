using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    using UIThumb = System.Windows.Controls.Primitives.Thumb;
    public class Thumb : Control
        {
        static Thumb()
            {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Thumb), new FrameworkPropertyMetadata(typeof(Thumb)));
            }
        private static readonly DependencyPropertyKey IsDraggingPropertyKey = DependencyProperty.RegisterReadOnly("IsDragging", typeof(Boolean), typeof(Thumb), new PropertyMetadata(default(Boolean)));
        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;
        public Boolean IsDragging
            {
            get { return (Boolean) GetValue(IsDraggingProperty); }
            protected set { SetValue(IsDraggingPropertyKey, value); }
            }

        #region E:DragCompleted:DragCompletedEventHandler
        public event DragCompletedEventHandler DragCompleted
            {
            add    { AddHandler(UIThumb.DragCompletedEvent, value);    }
            remove { RemoveHandler(UIThumb.DragCompletedEvent, value); }
            }
        #endregion
        #region E:DragDelta:DragDeltaEventHandler
        public event DragDeltaEventHandler DragDelta
            {
            add    { AddHandler(UIThumb.DragDeltaEvent, value);    }
            remove { RemoveHandler(UIThumb.DragDeltaEvent, value); }
            }
        #endregion
        #region E:DragStarted:DragStartedEventHandler
        public event DragStartedEventHandler DragStarted
            {
            add    { AddHandler(UIThumb.DragStartedEvent, value);    }
            remove { RemoveHandler(UIThumb.DragStartedEvent, value); }
            }
        #endregion
        }
    }
