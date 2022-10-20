using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BinaryStudio.PlatformUI.Models;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.Certificates.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.PlatformUI.Models
    {
    [TypeConverter(typeof(ERelativeDistinguishedNameSequenceTypeConverter))]
    internal class ERelativeDistinguishedNameSequence : NotifyPropertyChangedDispatcherObject<X509RelativeDistinguishedNameSequence>
        {
        public ERelativeDistinguishedNameSequence(X509RelativeDistinguishedNameSequence source)
            : base(source)
            {
            }

        #region M:GetEditor(Type):Object
        /// <summary>Returns an editor of the specified type for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.Object"/> of the specified type that is the editor for this object, or null if the editor cannot be found.</returns>
        /// <param name="editorBaseType">A <see cref="T:System.Type"/> that represents the editor for this object.</param>
        public override Object GetEditor(Type editorBaseType)
            {
            return null;
            }
        #endregion
        #region M:GetAttributes:IEnumerable<Attribute>
        /// <summary>Returns a collection of custom attributes for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.ComponentModel.AttributeCollection"/> containing the attributes for this object.</returns>
        protected override IEnumerable<Attribute> GetAttributes() {
            return TypeDescriptor.GetAttributes(GetType()).OfType<Attribute>();
            }
        #endregion

        private class Descriptor : PropertyDescriptor
            {
            private Object Value { get; }
            public Descriptor(Object value, Asn1ObjectIdentifier oid, params Attribute[] attributes)
                : base(oid.FriendlyName, attributes)
                {
                Value = value;
                }

            #region M:CanResetValue(Object):Boolean
            /**
             * <summary>When overridden in a derived class, returns whether resetting an object changes its value.</summary>
             * <param name="component">The component to test for reset capability.</param>
             * <returns>true if resetting the component changes its value; otherwise, false.</returns>
             * */
            public override Boolean CanResetValue(Object component)
                {
                return false;
                }
            #endregion
            #region M:GetValue(Object):Object
            /**
             * <summary>When overridden in a derived class, gets the current value of the property on a component.</summary>
             * <param name="component">The component with the property for which to retrieve the value.</param>
             * <returns>The value of a property for a given component.</returns>
             * */
            public override Object GetValue(Object component)
                {
                return Value;
                }
            #endregion
            #region M:ResetValue(Object)
            /**
             * <summary>When overridden in a derived class, resets the value for this property of the component to the default value.</summary>
             * <param name="component">The component with the property value that is to be reset to the default value.</param>
             * */
            public override void ResetValue(Object component)
                {
                throw new NotSupportedException();
                }
            #endregion
            #region M:SetValue(Object,Object)
            /**
             * <summary>When overridden in a derived class, sets the value of the component to a different value.</summary>
             * <param name="component">The component with the property value that is to be set.</param>
             * <param name="value">The new value.</param>
             * */
            public override void SetValue(Object component, Object value)
                {
                throw new NotSupportedException();
                }
            #endregion
            #region M:ShouldSerializeValue(Object):Boolean
            /**
             * <summary>When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.</summary>
             * <param name="component">The component with the property to be examined for persistence.</param>
             * <returns>true if the property should be persisted; otherwise, false.</returns>
             * */
            public override Boolean ShouldSerializeValue(Object component)
                {
                return false;
                }
            #endregion

            public override Type ComponentType { get { return typeof(X509RelativeDistinguishedNameSequence); }}
            public override Boolean IsReadOnly { get { return true; }}
            public override Type PropertyType  { get { return (Value != null) ? Value.GetType() : typeof(Object); }}
            public override String DisplayName { get { return Name; }}
            }

        #region M:GetProperties(Attribute[]):IEnumerable<PropertyDescriptor>
        /// <summary>Returns the properties for this instance of a component using the attribute array as a filter.</summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        /// <returns>A collection that represents the filtered properties for this component instance.</returns>
        protected override IEnumerable<PropertyDescriptor> GetProperties(Attribute[] attributes) {
            foreach (var i in Source) {
                yield return new Descriptor(i.Value,i.Key);
                }
            }
        #endregion
        }
    }