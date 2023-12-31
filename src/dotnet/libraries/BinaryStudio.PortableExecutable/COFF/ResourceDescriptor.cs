﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.PortableExecutable
    {
    public class ResourceDescriptor : IJsonSerializable
        {
        [DebuggerDisplay("{ToString(Identifier),nq}")]
        public ResourceIdentifier Identifier { get; }
        public ResourceDescriptor Owner { get; }
        internal IMAGE_RESOURCE_LEVEL Level { get; set; }
        public UInt32? CodePage { get; internal set; }
        public IList<Byte[]> Source { get; protected set; }
        public IList<ResourceDescriptor> Resources { get; private set; }

        #region M:ResourceDescriptor(ResourceDescriptor,ResourceIdentifier)
        protected internal ResourceDescriptor(ResourceDescriptor owner, ResourceIdentifier identifier)
            {
            Identifier = identifier;
            Owner = owner;
            Level = (owner != null)
                ? owner.Level + 1
                : 0;
            }
        #endregion
        #region M:ResourceDescriptor(ResourceDescriptor,ResourceIdentifier,Byte[])
        protected internal ResourceDescriptor(ResourceDescriptor owner, ResourceIdentifier identifier, Byte[] source)
            : this(owner, identifier)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            Source = new[] { source };
            }
        #endregion
        #region M:AddRange(IEnumerable<ResourceDescriptor>)
        internal void AddRange(IEnumerable<ResourceDescriptor> descriptors) {
            if (descriptors == null) { throw new ArgumentNullException(nameof(descriptors)); }
            if (Resources == null) { Resources = new List<ResourceDescriptor>(); }
            foreach (var descriptor in descriptors) {
                Resources.Add(descriptor);
                }
            }
        #endregion
        #region M:ToString(ResourceIdentifier):String
        //[UsedImplicitly]
        private String ToString(ResourceIdentifier source)
            {
            if ((Level == IMAGE_RESOURCE_LEVEL.LEVEL_TYPE) && (source.Identifier.HasValue)) { return ((IMAGE_RESOURCE_TYPE)source.Identifier).ToString(); }
            if ((Level == IMAGE_RESOURCE_LEVEL.LEVEL_LANGUAGE) && (source.Identifier.HasValue)) {
                if (source.Identifier == 0) { return "(neutral)"; }
                return CultureInfo.GetCultureInfo(source.Identifier.Value).IetfLanguageTag;
                }
            return source.ToString();
            }
        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return ToString(Identifier);
            }

        #region M:GetIetfLanguageTag(Int32):String
        protected static String GetIetfLanguageTag(Int32 value) {
            try
                {
                return (value == 0)
                    ? "{neutral}"
                    : CultureInfo.GetCultureInfo(value).IetfLanguageTag;
                }
            catch
                {
                return $"{{invalid}}:{{{value}}}";
                }
            }
        #endregion
        #region M:GetEncoding(Int32):Encoding
        protected static Encoding GetEncoding(Int32 codepage)
            {
            try
                {
                return (!encodings.TryGetValue(codepage, out var encoding))
                    ? encodings[codepage]=Encoding.GetEncoding(codepage)
                    : encoding;
                }
            catch
                {
                return encodings[codepage] = Encoding.ASCII;
                }
            }
        #endregion

        public virtual void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValueIfNotNull(nameof(Level),Level);
                if ((Level == IMAGE_RESOURCE_LEVEL.LEVEL_LANGUAGE) && (Identifier.Identifier.HasValue)) {
                    writer.WriteValue("CodePage", GetIetfLanguageTag(Identifier.Identifier.Value));
                    }
                else if (Level == IMAGE_RESOURCE_LEVEL.LEVEL_TYPE)
                    {
                    writer.WriteValueIfNotNull("Type",(IMAGE_RESOURCE_TYPE)Identifier.Identifier.GetValueOrDefault());
                    }
                else
                    {
                    writer.WriteValueIfNotNull(nameof(Identifier),Identifier);
                    }
                writer.WriteValueIfNotNull(nameof(Resources),Resources);
                }
            }

        private static IDictionary<Int32,Encoding> encodings = new Dictionary<Int32,Encoding>();
        }
    }