using System;
using System.Collections.Generic;
using System.Diagnostics;
using BinaryStudio.PortableExecutable.Win32;

namespace BinaryStudio.PortableExecutable
    {
    public class COFFDebugDirectoryLoader
        {
        public virtual unsafe Boolean Load(out Exception e, Byte* VirtualAddress, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory, Int32 count)
            {
            if (ImageDebugDirectory == null) { throw new ArgumentNullException(nameof(ImageDebugDirectory)); }
            if (VirtualAddress == null) { throw new ArgumentNullException(nameof(VirtualAddress)); }
            e = null;
            var Header = ImageDebugDirectory;
            try
                {
                for (var i = 0; i < count; i++) {
                    try
                        {
                        LoadDirectory(VirtualAddress,Header);
                        Header++;
                        }
                    catch (Exception x)
                        {
                        x.Data["HeaderOrder"] = i;
                        throw;
                        }
                    }
                }
            catch (Exception x)
                {
                e = x;
                }
            return (e == null);
            }

        protected virtual unsafe void LoadDirectory(Byte* VirtualAddress, IMAGE_DEBUG_DIRECTORY* ImageDebugDirectory)
            {
            if (ImageDebugDirectory == null) { throw new ArgumentNullException(nameof(ImageDebugDirectory)); }
            if (VirtualAddress == null) { throw new ArgumentNullException(nameof(VirtualAddress)); }
            }
        }
    }