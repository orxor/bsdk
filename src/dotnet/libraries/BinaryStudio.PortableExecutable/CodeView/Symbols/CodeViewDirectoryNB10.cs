using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable LocalVariableHidesMember
// ReSharper disable ParameterHidesMember

namespace BinaryStudio.PortableExecutable.CodeView
    {
    [OMFDirectorySignature(OMFDirectorySignature.NB10)]
    public class CodeViewDirectoryNB10 : OMFDirectory
        {
        /// <summary>
        /// CodeView NB10 debug information of a PDB 2.00 file (VS 6).
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct CV_INFO_PDB20
            {
            private readonly OMFDirectorySignatureHeader Header;
            private readonly Int32 Signature;
            public  readonly Int32 Age;
            private readonly Byte FileName;

            /// <summary>Returns the fully qualified type name of this instance.</summary>
            /// <returns>A <see cref="T:System.String"/> containing a fully qualified type name.</returns>
            /// <filterpriority>2</filterpriority>
            public override unsafe String ToString() {
                var r = new StringBuilder();
                fixed (Byte* bytes = &FileName) {
                    for (var i = 0; ; i++) {
                        if (bytes[i] == 0) { break; }
                        r.Append((Char)bytes[i]);
                        }
                    }
                return r.ToString();
                }
            }

        public override OMFDirectorySignature Signature { get { return OMFDirectorySignature.NB10; }}
        public String FileName { get;private set; }
        public Int32 Age { get;private set; }

        public CodeViewDirectoryNB10(IntPtr BaseAddress, IntPtr BegOfDebugData, IntPtr EndOfDebugData)
            :base(BaseAddress,BegOfDebugData,EndOfDebugData)
            {
            }

        public override unsafe void Analyze()
            {
            ValidateSignature((OMFDirectorySignatureHeader*)BegOfDebugData);
            var Header = (CV_INFO_PDB20*)BegOfDebugData;
            FileName = Header->ToString();
            Age = Header->Age;
            Status = 1;
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override String ToString()
            {
            return $"{Signature}:{((Status == 1) ? FileName : "Pending...")}";
            }
        }
    }