using System;
using BinaryStudio.PortableExecutable;

namespace DllResources
    {
    class Program
        {
        static void Main(string[] args) {
            using (var Scope = new MetadataScope()) {
                var o = Scope.Load(@"C:\Windows\System32\winmm.dll");
                if (o != null)
                    {

                    }
                }
            }
        }
    }
