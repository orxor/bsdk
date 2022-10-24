using System;

namespace BinaryStudio.PortableExecutable
    {
    public class MetadataObjectIdentity
        {
        public String LocalName { get; }
        public Guid ServiceIdentity { get; }
        public MetadataObjectIdentity(String localName, Guid service)
            {
            if (localName == null) { throw new ArgumentNullException(nameof(localName)); }
            LocalName = localName;
            ServiceIdentity = service;
            }

        public MetadataObjectIdentity(String localName, Type service)
            {
            if (localName == null) { throw new ArgumentNullException(nameof(localName)); }
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            LocalName = localName;
            ServiceIdentity = service.GUID;
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return $"{LocalName}:{ServiceIdentity}";
            }
        }
    }