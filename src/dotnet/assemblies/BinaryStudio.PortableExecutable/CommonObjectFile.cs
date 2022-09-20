using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    public class CommonObjectFile : MetadataObject
        {
        private const Int32 IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b;
        private const Int32 IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b;
        private const Int32 IMAGE_ROM_OPTIONAL_HDR_MAGIC  = 0x107;
        private const Int32 IMAGE_NUMBEROF_DIRECTORY_ENTRIES = 16;
        private const Int16 IMAGE_SYM_UNDEFINED =  0;
        private const Int16 IMAGE_SYM_ABSOLUTE  = -1;
        private const Int16 IMAGE_SYM_DEBUG     = -2;

        internal Boolean IgnoreOptionalHeaderSize { get;set; }
        public ImageFlags Flags { get;private set; }
        public Boolean Is64Bit { get { return Flags.HasFlag(ImageFlags.Is64Bit); }}
        public IList<ImportLibraryReference> ImportLibraryReferences { get;private set; }

        internal CommonObjectFile(MetadataScope scope, MetadataObjectIdentity identity)
            : base(scope, identity)
            {
            IgnoreOptionalHeaderSize = true;
            }

        private unsafe delegate void* RVA(UInt32 virtualaddress);

        #region M:GetString(Encoding,Byte*):String
        private static unsafe String GetString(Encoding encoding, Byte* source) {
            if (source == null) { return null; }
            var c = 0;
            for (;;++c) {
                if (source[c] == 0) { break; }
                }
            var r = new Byte[c];
            for (var i = 0;i < c;++i) {
                r[i] = source[i];
                }
            return encoding.GetString(r);
            }
        #endregion
        #region M:LoadCore(IntPtr[],Int64)
        /// <summary>Loads content from specified source.</summary>
        /// <param name="source">Content specific source addresses depending on its type.</param>
        /// <param name="length">Length of content.</param>
        protected override unsafe void LoadCore(IntPtr[] source, Int64 length) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source.Length == 0) { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (length < 2) { throw new ArgumentOutOfRangeException(nameof(length)); }
            Load((Byte*)source[0],(IMAGE_FILE_HEADER*)source[1],length);
            }
        #endregion
        /// <summary>Loads COFF content from specified sources.</summary>
        /// <param name="mapping">Base address of file.</param>
        /// <param name="source">COFF header(<see cref="IMAGE_FILE_HEADER"/>)</param>
        /// <param name="size">Length of content.</param>
        protected unsafe void Load(Byte* mapping, IMAGE_FILE_HEADER* source, Int64 size) {
            if (size > Marshal.SizeOf(typeof(IMAGE_FILE_HEADER))) {
                if (Enum.IsDefined(typeof(IMAGE_FILE_MACHINE), source->Machine)) {
                    var machine = source->Machine;
                    var r = (Byte*)(source + 1);
                    IMAGE_DATA_DIRECTORY*[] directories = null;
                    if ((source->SizeOfOptionalHeader > 0) && !IgnoreOptionalHeaderSize) {
                        var magic = *(UInt16*)r;
                        if ((magic == IMAGE_NT_OPTIONAL_HDR32_MAGIC) && (source->SizeOfOptionalHeader == (sizeof(IMAGE_OPTIONAL_HEADER32) + sizeof(IMAGE_DATA_DIRECTORY)*IMAGE_NUMBEROF_DIRECTORY_ENTRIES)))   { directories = Load((IMAGE_OPTIONAL_HEADER32*)r, size); }
                        if ((magic == IMAGE_NT_OPTIONAL_HDR64_MAGIC) && (source->SizeOfOptionalHeader == (sizeof(IMAGE_OPTIONAL_HEADER64) + sizeof(IMAGE_DATA_DIRECTORY)*IMAGE_NUMBEROF_DIRECTORY_ENTRIES)))   { directories = Load((IMAGE_OPTIONAL_HEADER64*)r, size); Flags |= ImageFlags.Is64Bit; }
                        if ((magic == IMAGE_ROM_OPTIONAL_HDR_MAGIC)  && (source->SizeOfOptionalHeader == (sizeof(IMAGE_ROM_OPTIONAL_HEADER)))) { directories = Load((IMAGE_ROM_OPTIONAL_HEADER*)r, size); }
                        r += source->SizeOfOptionalHeader;
                        }
                    var sections = (IMAGE_SECTION_HEADER*)r;
                    var rvami = new RVA((virtualaddress)=>{
                        for (var i = 0; i < source->NumberOfSections; ++i) {
                            if ((sections[i].VirtualAddress <= virtualaddress) && (virtualaddress <= (sections[i].VirtualAddress + sections[i].VirtualSize))) {
                                return mapping
                                    + (Int64)(sections[i].PointerToRawData
                                    - sections[i].VirtualAddress + virtualaddress);
                                }
                            }
                        return null;
                        });
                    #if DEBUG
                    for (var i = 0; i < source->NumberOfSections; ++i) {
                        Debug.Print("section:\"{0}\":{1:X8}:{2:X8}",
                            sections[i],
                            sections[i].VirtualAddress,
                            sections[i].VirtualAddress + sections[i].VirtualSize);
                        }
                    #endif
                    if ((directories != null) && (source->NumberOfSections > 0)) {
                        var sz = source->NumberOfSections;
                        var entries = new List<Tuple<IntPtr,IntPtr,IMAGE_DIRECTORY_ENTRY>>();
                        for (var i = 0; i < directories.Length; i++) {
                            if (directories[i]->Size > 0) {
                                for (var j = 0; j < sz; j++) {
                                    if ((sections[j].VirtualAddress == directories[i]->VirtualAddress) ||
                                        ((directories[i]->VirtualAddress >= sections[j].VirtualAddress) &&
                                         (directories[i]->VirtualAddress < sections[j].VirtualAddress + sections[j].SizeOfRawData))) {
                                        entries.Add(Tuple.Create(
                                            (IntPtr)(void*)directories[i],
                                            (IntPtr)(void*)&sections[j],
                                            (IMAGE_DIRECTORY_ENTRY)i));
                                        break;
                                        }
                                    }
                                }
                            }

                        foreach(var i in entries) {
                            Load(mapping,
                                (IMAGE_DATA_DIRECTORY*)i.Item1.ToPointer(),
                                (IMAGE_SECTION_HEADER*)i.Item2.ToPointer(),
                                i.Item3, machine, rvami);
                            }
                        }
                    return;
                    }
                }
            throw new InvalidDataException();
            }

        #region M:Load(IMAGE_OPTIONAL_HEADER32*,Int64):IMAGE_DATA_DIRECTORY*[]
        private unsafe IMAGE_DATA_DIRECTORY*[] Load(IMAGE_OPTIONAL_HEADER32* source, Int64 size) {
            var r = new IMAGE_DATA_DIRECTORY*[source->NumberOfRvaAndSizes];
            source++;
            var entries = (IMAGE_DATA_DIRECTORY*)source;
            for (var i = 0; i < r.Length; i++) {
                r[i] = &entries[i];
                }
            return r;
            }
        #endregion
        #region M:Load(IMAGE_OPTIONAL_HEADER64*,Int64):IMAGE_DATA_DIRECTORY*[]
        protected virtual unsafe IMAGE_DATA_DIRECTORY*[] Load(IMAGE_OPTIONAL_HEADER64* source, Int64 size) {
            var r = new IMAGE_DATA_DIRECTORY*[source->NumberOfRvaAndSizes];
            source++;
            var entries = (IMAGE_DATA_DIRECTORY*)source;
            for (var i = 0; i < r.Length; i++) {
                r[i] = &entries[i];
                }
            return r;
            }
        #endregion
        #region M:Load(IMAGE_ROM_OPTIONAL_HEADER*,Int64):IMAGE_DATA_DIRECTORY*[]
        private unsafe IMAGE_DATA_DIRECTORY*[] Load(IMAGE_ROM_OPTIONAL_HEADER* source, Int64 size)
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:Load(Byte*,IMAGE_DATA_DIRECTORY,IMAGE_SECTION_HEADER,IMAGE_DIRECTORY_ENTRY,IMAGE_FILE_MACHINE,RVA)
        private unsafe void Load(Byte* source, IMAGE_DATA_DIRECTORY* directory, IMAGE_SECTION_HEADER* section, IMAGE_DIRECTORY_ENTRY index, IMAGE_FILE_MACHINE machine, RVA rvami) {
            var address = source + (Int64)section->PointerToRawData - (Int64)section->VirtualAddress;
            switch (index) {
                case IMAGE_DIRECTORY_ENTRY.IMAGE_DIRECTORY_ENTRY_IMPORT:    { Load(address, (IMAGE_IMPORT_DIRECTORY*)(address + directory->VirtualAddress));    } break;
                case IMAGE_DIRECTORY_ENTRY.IMAGE_DIRECTORY_ENTRY_EXCEPTION: { Load(address, (IMAGE_RUNTIME_FUNCTION_ENTRY*)(address + directory->VirtualAddress), directory->Size, machine, rvami); } break;
                case IMAGE_DIRECTORY_ENTRY.IMAGE_DIRECTORY_ENTRY_EXPORT:    { Load(address, (IMAGE_EXPORT_DIRECTORY*)(address + directory->VirtualAddress), section); } break;
                case IMAGE_DIRECTORY_ENTRY.IMAGE_DIRECTORY_ENTRY_RESOURCE:
                    {
                    }
                    break;
                }
            }
        #endregion
        #region M:Load(Byte*,IMAGE_IMPORT_DIRECTORY*)
        private unsafe void Load(Byte* address, IMAGE_IMPORT_DIRECTORY* source) {
            if (ImportLibraryReferences != null) { throw new InvalidOperationException(); }
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var r = new List<ImportLibraryReference>();
            var header = source;
            while ((header->ImportAddressTable != 0) && (header->ImportLookupTable != 0)) {
                var library = (header->Name != 0)
                    ? GetString(Encoding.ASCII, address + (Int64)header->Name)
                    : null;
                var symbols = new HashSet<ImportSymbolDescriptor>();
                symbols.UnionWith(LoadImportTable(address, header->ImportLookupTable));
                symbols.UnionWith(LoadImportTable(address, header->ImportAddressTable));
                r.Add(new ImportLibraryReference(library, symbols));
                header++;
                }
            ImportLibraryReferences = new ReadOnlyCollection<ImportLibraryReference>(r.OrderBy(i => i).ToArray());
            }
        #endregion
        #region M:Load(Byte*,IMAGE_EXPORT_DIRECTORY*,IMAGE_SECTION_HEADER*)
        private unsafe void Load(Byte* address, IMAGE_EXPORT_DIRECTORY* source, IMAGE_SECTION_HEADER* section) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var ordinaltable     = (UInt16*)(address + (Int64)source->OrdinalTableRVA);
            var namepointertable = (UInt32*)(address + (Int64)source->NamePointerRVA);
            var exportaddresses  = (UInt32*)(address + (Int64)source->ExportAddressTableRVA);
            var left = section->VirtualAddress;
            var right = section->VirtualAddress + section->VirtualSize;
            var r = new ExportSymbolDescriptor[source->AddressTableEntries];
            for (var i = 0; i < source->NumberOfNamePointers; ++i) {
                var ordinal = ordinaltable[i];
                var name    = GetString(Encoding.ASCII, (Int64)namepointertable[i] + address);
                r[ordinal] = new ExportSymbolDescriptor(name, (Int32)(ordinal + source->OrdinalBase));
                }
            for (var i = 0; i < source->AddressTableEntries; ++i) {
                var n = exportaddresses[i];
                if (n == 0) { continue; }
                if ((n >= left) && (n <= right)) {
                    /* Forwarder RVA */
                    if (r[i] != null) {
                        r[i].EntryPointName = GetString(Encoding.ASCII, (Int64)n + address);
                        }
                    else
                        {
                        r[i] = new ExportSymbolDescriptor((Int32)(i + source->OrdinalBase), GetString(Encoding.ASCII, (Int64)n + address));
                        }
                    }
                else
                    {
                    /* Export RVA */
                    if (r[i] != null) {
                        r[i].EntryPoint = n;
                        }
                    else
                        {
                        r[i] = new ExportSymbolDescriptor((Int32)(i + source->OrdinalBase), n);
                        }
                    }
                }
            var target = r.Where(i => i != null).ToArray();
            }
        #endregion
        #region M:LoadImportTable(Byte*,Int64):IEnumerable<ImportSymbolDescriptor>
        private unsafe IEnumerable<ImportSymbolDescriptor> LoadImportTable(Byte* address, Int64 offset) {
            var r = address + offset;
            var target = new List<ImportSymbolDescriptor>();
            for (;;) {
                var ordinalnumber = -1;
                var name = 0UL;
                if (Is64Bit) {
                    var i = *(UInt64*)r;
                    if (i == 0) { break; }
                    if ((i & 0x8000000000000000) == 0x8000000000000000) {
                        ordinalnumber = (UInt16)(i & 0xFFFF);
                        }
                    else
                        {
                        name = i & 0x7FFFFFFFFFFFFFFF;
                        }
                    r += 8;
                    }
                else
                    {
                    var i = *(UInt32*)r;
                    if (i == 0) { break; }
                    if ((i & 0x80000000) == 0x80000000) {
                        ordinalnumber = (UInt16)(i & 0xFFFF);
                        }
                    else
                        {
                        name = i & 0x7FFFFFFF;
                        }
                    r += 4;
                    }
                if (ordinalnumber > 0)
                    {
                    target.Add(new ImportSymbolDescriptor(ordinalnumber));
                    }
                else if (name != 0)
                    {
                    var i = address + (Int64)name;
                    target.Add(new ImportSymbolDescriptor(GetString(Encoding.ASCII, i + 2), (Int32)(*(UInt16*)i)));
                    }
                }
            return target;
            }
        #endregion

        private unsafe void Load(Byte* address, IMAGE_RUNTIME_FUNCTION_ENTRY* entries, Int64 size, IMAGE_FILE_MACHINE machine, RVA rvami) {
            }
        }
    }