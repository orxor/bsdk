using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.Serialization;

namespace BinaryStudio.PortableExecutable
    {
    internal class ResourceMessageTableDescriptor : ResourceDescriptor, IDictionary<UInt32, String>
        {
        private readonly IDictionary<UInt32,String> values;

        private enum MESSAGE_RESOURCE_ENTRY_ENCODING : ushort
            {
            ANSI,
            UNICODE
            }

        /// <summary>
        /// The message-table entry descriptor.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct MESSAGE_RESOURCE_BLOCK
            {
            public readonly UInt32 LowId;                           /* First message identifier.    */
            public readonly UInt32 HighId;                          /* Last message identifier.     */
            public readonly UInt32 OffsetToEntries;                 /* Offset of the first message. */
            }

        /// <summary>
        /// The message-table string entry.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct MESSAGE_RESOURCE_ENTRY
            {
            public readonly UInt16 Length;                          /* Size.  */
            public readonly MESSAGE_RESOURCE_ENTRY_ENCODING Flags;  /* Flags. */
            }

        internal ResourceMessageTableDescriptor(ResourceDescriptor owner, ResourceIdentifier identifier, Byte[] source)
            :base(owner, identifier, source)
            {
            try
                {
                values = new SortedDictionary<UInt32, String>();
                unsafe
                    {
                    fixed (Byte* r = source) {
                        var ANSICodePage = CultureInfo.GetCultureInfo(Identifier.Identifier.Value).TextInfo.OEMCodePage;
                        var NumberOfBlocks = *(UInt32*)r; /* MESSAGE_RESOURCE_DATA.NumberOfBlocks */
                        if (NumberOfBlocks > 0) {
                            var block = (MESSAGE_RESOURCE_BLOCK*)(r + sizeof(UInt32));
                            for (var i = 0U; i < NumberOfBlocks; i++) {
                                var entry = (MESSAGE_RESOURCE_ENTRY*)(r + block->OffsetToEntries);
                                for (var j = block->LowId; j <= block->HighId; j++) {
                                    if (entry->Length > 0) {
                                        var Size = entry->Length - 4;
                                        var encoding = (entry->Flags == MESSAGE_RESOURCE_ENTRY_ENCODING.UNICODE)
                                                ? Encoding.Unicode
                                                : Encoding.GetEncoding(ANSICodePage);
                                        entry++;
                                        values.Add(j, encoding.GetString((Byte*)entry, Size).Trim('\0').Trim('\r','\n'));
                                        entry = (MESSAGE_RESOURCE_ENTRY*)(((Byte*)entry) + Size);
                                        }
                                    else
                                        {
                                        entry = (MESSAGE_RESOURCE_ENTRY*)(((Byte*)entry) + entry->Length);
                                        }
                                    }
                                block++;
                                }
                            }
                        }
                    }
                }
            catch (Exception e)
                {
                throw;
                }
            }

        #region M:IEnumerable<KeyValuePair<UInt32,String>>.GetEnumerator:IEnumerator<KeyValuePair<UInt32,String>>
        IEnumerator<KeyValuePair<UInt32, String>> IEnumerable<KeyValuePair<UInt32, String>>.GetEnumerator() { return values.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return ((IEnumerable)values).GetEnumerator(); }
        #endregion
        #region M:ICollection<KeyValuePair<UInt32,String>>.Add(KeyValuePair<UInt32,String>)
        void ICollection<KeyValuePair<UInt32, String>>.Add(KeyValuePair<UInt32, String> item) {
            throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }
        #endregion
        #region M:ICollection<KeyValuePair<UInt32,String>>.Clear
        void ICollection<KeyValuePair<UInt32, String>>.Clear() {
            throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }
        #endregion
        #region M:ICollection<KeyValuePair<UInt32,String>>.Contains(KeyValuePair<UInt32,String>):Boolean
        Boolean ICollection<KeyValuePair<UInt32, String>>.Contains(KeyValuePair<UInt32, String> item) {
            return values.Contains(item);
            }
        #endregion
        #region M:ICollection<KeyValuePair<UInt32,String>>.CopyTo(KeyValuePair<UInt32,String>[],Int32)
        void ICollection<KeyValuePair<UInt32, String>>.CopyTo(KeyValuePair<UInt32, String>[] array, Int32 arrayIndex) {
            values.CopyTo(array, arrayIndex);
            }
        #endregion
        #region M:ICollection<KeyValuePair<UInt32,String>>.Remove(KeyValuePair<UInt32,String>):Boolean
        Boolean ICollection<KeyValuePair<UInt32, String>>.Remove(KeyValuePair<UInt32, String> item) {
            throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }
        #endregion
        #region M:IDictionary<UInt32,String>.ContainsKey(UInt32):Boolean
        Boolean IDictionary<UInt32, String>.ContainsKey(UInt32 key) {
            return values.ContainsKey(key);
            }
        #endregion
        #region M:IDictionary<UInt32,String>.Add(UInt32,String)
        void IDictionary<UInt32, String>.Add(UInt32 key, String value) {
            throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }
        #endregion
        #region M:IDictionary<UInt32, String>.Remove(UInt32):Boolean
        Boolean IDictionary<UInt32, String>.Remove(UInt32 key) {
            throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }
        #endregion
        #region M:IDictionary<UInt32,String>.TryGetValue(UInt32,out String):Boolean
        Boolean IDictionary<UInt32, String>.TryGetValue(UInt32 key, out String value) {
            return values.TryGetValue(key, out value);
            }
        #endregion
        #region P:ICollection<KeyValuePair<UInt32,String>>.Count:Int32
        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        Int32 ICollection<KeyValuePair<UInt32, String>>.Count {
            get { return values.Count; }
            }
        #endregion
        #region P:ICollection<KeyValuePair<UInt32,String>>.IsReadOnly:Boolean
        Boolean ICollection<KeyValuePair<UInt32, String>>.IsReadOnly {
            get { return true; }
            }
        #endregion
        #region P:IDictionary<UInt32,String>.this[UInt32]:String
        String IDictionary<UInt32, String>.this[UInt32 key] {
            get { return values[key]; }
            set { throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection); }
            }
        #endregion
        #region P:IDictionary<UInt32,String>.Keys:ICollection<UInt32>
        ICollection<UInt32> IDictionary<UInt32, String>.Keys {
            get { return values.Keys; }
            }
        #endregion
        #region P:IDictionary<UInt32,String>.Values:ICollection<String>
        ICollection<String> IDictionary<UInt32, String>.Values {
            get { return values.Values; }
            }
        #endregion
        
        public override void WriteTo(IJsonWriter writer) {
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
                writer.WriteValueIfNotNull("Values",values.Select(i => $"{i.Key:x8}:{i.Value}").ToArray());
                writer.WriteValueIfNotNull(nameof(Resources),Resources);
                }
            }
        }
    }