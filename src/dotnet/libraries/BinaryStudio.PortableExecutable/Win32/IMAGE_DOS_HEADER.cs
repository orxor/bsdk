using System;
using System.Runtime.InteropServices;

namespace BinaryStudio.PortableExecutable.Win32
    {
    /// <summary>
    /// DOS .EXE header.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_DOS_HEADER
        {
        public  readonly UInt16 e_magic;            /* Magic number.                        */
        private readonly UInt16 e_cblp;             /* Bytes on last page of file.          */
        private readonly UInt16 e_cp;               /* Pages in file.                       */
        private readonly UInt16 e_crlc;             /* Relocations.                         */
        private readonly UInt16 e_cparhdr;          /* Size of header in paragraphs.        */
        private readonly UInt16 e_minalloc;         /* Minimum extra paragraphs needed.     */
        private readonly UInt16 e_maxalloc;         /* Maximum extra paragraphs needed.     */
        private readonly UInt16 e_ss;               /* Initial (relative) SS value.         */
        private readonly UInt16 e_sp;               /* Initial SP value.                    */
        private readonly UInt16 e_csum;             /* Checksum.                            */
        private readonly UInt16 e_ip;               /* Initial IP value.                    */
        private readonly UInt16 e_cs;               /* Initial (relative) CS value.         */
        private readonly UInt16 e_lfarlc;           /* File address of relocation table.    */
        private readonly UInt16 e_ovno;             /* Overlay number.                      */
        private unsafe fixed UInt16 e_res[4];       /* Reserved words.                      */
        private readonly UInt16 e_oemid;            /* OEM identifier (for e_oeminfo).      */
        private readonly UInt16 e_oeminfo;          /* OEM information; e_oemid specific.   */
        private unsafe fixed UInt16 e_res2[10];     /* Reserved words.                      */
        public  readonly UInt32 e_lfanew;           /* File address of new exe header.      */
        }
    }