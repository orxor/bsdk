using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.PortableExecutable
    {
    public class ResourceMUIDescriptor : ResourceDescriptor
        {
        private const UInt32 MUI_SIGNATURE = 0xfecdfecd;

        public CultureInfo UltimateFallbackLanguage { get; }
        public CultureInfo Language { get; }
        public Boolean IsUltimateFallbackLocationExternal { get; }
        public IList<ResourceIdentifier> LocalIdentifiers { get; }
        public IList<ResourceIdentifier> ExternalIdentifiers { get; }
        public CommonObjectFileSource ExternalSource { get;private set; }

        internal unsafe ResourceMUIDescriptor(ResourceDescriptor owner, ResourceIdentifier identifier, Byte[] source)
            : base(owner, identifier, source)
            {
            fixed(Byte* r = source) {
                var src = r;
                if ((*(UInt32*)src) == MUI_SIGNATURE) {
                    var header = (MUI_HEADER*)src;
                    IsUltimateFallbackLocationExternal = (header->UltimateFallbackLocation == MUI_ULTIMATE_FALLBACK_LOCATION.MUI_ULTIMATE_FALLBACK_LOCATION_EXTERNAL);
                    LocalIdentifiers = ToList(
                        LoadIdentifiers(src + header->MainIDTypesDataOffset, header->MainIDTypesDataSize),
                        LoadStrings(src + header->MainNameTypeDataOffset, header->MainNameTypeDataSize)
                        ).ToArray();
                    ExternalIdentifiers = ToList(
                        LoadIdentifiers(src + header->MUIIDTypesDataOffset, header->MUIIDTypesDataSize),
                        LoadStrings(src + header->MUINameTypeDataOffset, header->MUINameTypeDataSize)
                        ).ToArray();
                    UltimateFallbackLanguage = ToCultureInfo(src + header->UltimateFallbackLanguageDataOffset, header->UltimateFallbackLanguageDataSize);
                    Language = ToCultureInfo(src + header->LanguageDataOffset, header->LanguageDataSize);
                    }
                }
            }

        private static unsafe IMAGE_RESOURCE_TYPE[] LoadIdentifiers(Byte* source, UInt32 size) {
            if (size > 0) {
                var sz = size / 4;
                var r = new IMAGE_RESOURCE_TYPE[sz];
                var src = (IMAGE_RESOURCE_TYPE*)source;
                for (var i = 0; i < sz; i++) {
                    r[i] = *src;
                    src++;
                    }
                return r;
                }
            return null;
            }

        private static IEnumerable<ResourceIdentifier> ToList(IEnumerable<IMAGE_RESOURCE_TYPE> x, IEnumerable<String> y) {
            if (x != null) {
                foreach (var value in x) {
                    yield return new ResourceIdentifier((Int32)value) { Level = IMAGE_RESOURCE_LEVEL.LEVEL_TYPE };
                    }
                }
            if (y != null) {
                foreach (var value in y) {
                    yield return new ResourceIdentifier(value);
                    }
                }
            }

        private static unsafe List<String> LoadStrings(Byte* source, Int32 size) {
            if (size > 0) {
                var r = new List<String>();
                var src = (UInt16*)source;
                var bytes = new Byte[size];
                var sz = 0;
                for (var i = 0; size > 0;) {
                    if (*src > 0) {
                        bytes[i] = (Byte)((*src) & 0xFF);
                        bytes[i + 1] = (Byte)(((*src) >> 8) & 0xFF);
                        i += 2;
                        src++;
                        size-= 2;
                        sz += 2;
                        }
                    else
                        {
                        src++;
                        if (sz > 0) {
                            r.Add(Encoding.Unicode.GetString(bytes, 0, sz));
                            }
                        i = 0;
                        size -= 2;
                        bytes = new Byte[size];
                        sz = 0;
                        }
                    }
                return r;
                }
            return new List<String>();
            }

        private static unsafe CultureInfo ToCultureInfo(Byte* source, Int32 size) {
            var r = LoadStrings(source, size);
            return ((r != null) && (r.Count > 0))
                ? CultureInfo.GetCultureInfoByIetfLanguageTag(r[0])
                : null;
            }

        public void AttachExternalResource(MetadataObject source) {
            ExternalSource = null;
            if (source != null) {
                ExternalSource = ((CommonObjectFileSource)source.GetService(typeof(CommonObjectFileSource)));
                }
            }

        public override void WriteTo(IJsonWriter writer) {
            if (writer == null) { throw new ArgumentNullException(nameof(writer)); }
            using (writer.Object()) {
                writer.WriteValueIfNotNull(nameof(Level),Level);
                writer.WriteValueIfNotNull("IsMUI",true);
                if ((Level == IMAGE_RESOURCE_LEVEL.LEVEL_LANGUAGE) && (Identifier.Identifier.HasValue)) {
                    writer.WriteValue("CodePage", (Identifier.Identifier == 0)
                        ? "{neutral}"
                        : CultureInfo.GetCultureInfo(Identifier.Identifier.Value).IetfLanguageTag);
                    }
                else
                    {
                    writer.WriteValueIfNotNull(nameof(Identifier),Identifier);
                    }
                writer.WriteValueIfNotNull(nameof(Resources),Resources);
                if (ExternalSource != null) {
                    writer.WriteValueIfNotNull("LinkedResources",ExternalSource.Resources);
                    }
                }
            }
        }
    }