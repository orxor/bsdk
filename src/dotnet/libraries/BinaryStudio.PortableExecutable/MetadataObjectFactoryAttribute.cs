using System;

namespace BinaryStudio.PortableExecutable
    {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class MetadataObjectFactoryAttribute : Attribute
        {
        }
    }