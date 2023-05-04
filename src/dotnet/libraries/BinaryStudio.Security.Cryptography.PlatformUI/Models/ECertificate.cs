using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BinaryStudio.DataProcessing;
using BinaryStudio.PlatformUI.Models;
using BinaryStudio.Security.Cryptography.Certificates;
using BinaryStudio.Security.Cryptography.Certificates.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.PlatformUI.Models
    {
    public class ECertificate : NotifyPropertyChangedDispatcherObject<X509Certificate>
        {
        public ECertificate(X509Certificate source)
            : base(source)
            {
            }

        #region M:OnPropertyChanged(String)
        /// <summary>Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event when the specified property has been changed.</summary>
        /// <param name="PropertyName">The property that has been changed.</param>
        protected override void OnPropertyChanged(String PropertyName = null) {
            base.OnPropertyChanged(PropertyName);
            switch (PropertyName) {
                case nameof(Source):
                    {
                    Structure = Source.Source;
                    Properties = new EProperties(Source);
                    }
                    break;
                }
            }
        #endregion

        private class EProperties : NotifyPropertyChangedDispatcherObject<X509Certificate>
            {
            private static readonly IComparer<PropertyDescriptor> DefaultComparer = new PropertyDescriptorComparer();
            public EProperties(X509Certificate source)
                :base(source)
                {
                Subject = new ERelativeDistinguishedNameSequence(Source.Source.Subject);
                Issuer  = new ERelativeDistinguishedNameSequence(Source.Source.Issuer);
                }

            [Order(0x01)] public Int32 Version { get { return Source.Version; }}
            [Order(0x02)] public ERelativeDistinguishedNameSequence Subject { get; }
            [Order(0x03)] public ERelativeDistinguishedNameSequence Issuer { get; }

            #region M:GetProperties(Attribute[]):IEnumerable<PropertyDescriptor>
            /// <summary>Returns the properties for this instance of a component using the attribute array as a filter.</summary>
            /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
            /// <returns>A collection that represents the filtered properties for this component instance.</returns>
            protected override IEnumerable<PropertyDescriptor> GetProperties(Attribute[] attributes) {
                foreach (var descriptor in TypeDescriptor.GetProperties(GetType(), attributes).OfType<PropertyDescriptor>().OrderBy(i => i,DefaultComparer)) {
                    yield return descriptor;
                    }
                }
            #endregion
            #region M:GetPropertyOwner(PropertyDescriptor):Object
            /// <summary>Returns an object that contains the property described by the specified property descriptor.</summary>
            /// <param name="descriptor">A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that represents the property whose owner is to be found.</param>
            /// <returns>An <see cref="T:System.Object"/> that represents the owner of the specified property.</returns>
            protected override Object GetPropertyOwner(PropertyDescriptor descriptor)
                {
                return this;
                }
            #endregion
            }

        public Object Properties { get;private set; }
        public Object Structure { get;private set; }
        }
    }