using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using BinaryStudio.PlatformUI.Controls;

namespace BinaryStudio.PlatformUI
    {
    public class EventTrigger : System.Windows.Interactivity.EventTrigger
        {
        #region P:Setters:SetterBaseCollection
        private SetterBaseCollection setters;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SetterBaseCollection Setters { get {
            VerifyAccess();
            return setters ?? (setters = new SetterBaseCollection());
            }}
        #endregion
        private INameScope Scope { get;set; }

        #region M:OnAttached
        /// <summary>
        /// Called after the trigger is attached to an <see cref="P:TriggerBase.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached() {
            base.OnAttached();
            }
        #endregion
        #region M:OnEvent(EventArgs)
        /// <summary>Called when the event associated with this EventTriggerBase is fired. By default, this will invoke all actions on the trigger.</summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        /// <remarks>Override this to provide more granular control over when actions associated with this trigger will be invoked.</remarks>
        protected override void OnEvent(EventArgs e)
            {
            base.OnEvent(e);
            InvokeSetters(AssociatedObject as FrameworkElement, Setters);
            }
        #endregion

        private void InvokeSetters(FrameworkElement source, SetterBaseCollection setters) {
            if (source != null) {
                foreach (var setter in setters.OfType<Setter>()) {
                    var resolver = new NameResolver {
                        NameScopeReferenceElement = source,
                        Name = setter.TargetName
                        };
                    var r = resolver.Object;
                    if (r != null) {
                        if (setter.Property != null) {
                            var value = setter.Value;
                            if (value is Binding binding) { value = binding.GetValue(); }
                            r.SetValue(setter.Property, value);
                            }
                        }
                    }
                }
            }
        }
    }