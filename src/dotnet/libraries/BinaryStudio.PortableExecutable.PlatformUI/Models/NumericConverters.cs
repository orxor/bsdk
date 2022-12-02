using BinaryStudio.PlatformUI.Shell.Controls;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    public class NumericConverters
        {
        public static AddInt64 ADDI8 { get; }
        public static SubInt64 SUBI8 { get; }

        static NumericConverters()
            {
            ADDI8 = new AddInt64();
            SUBI8 = new SubInt64();
            }
        }
    }