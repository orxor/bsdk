using System;
using BinaryStudio.PortableExecutable;
using BinaryStudio.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTests.BinaryStudio.PortableExecutable
    {
    [TestClass]
    public class MZMetadataObjectT
        {
        [TestInitialize]
        public void Setup()
            {
            }

        [TestMethod]
        public void JsonS() {
            using (var Scope = new MetadataScope()) {
                var o = Scope.Load(@"C:\TFS\bsdk\src\delphi\tests\UnitTests.BinaryStudio.Task\Task.exe");
                if (o != null) {
                    using (var writer = new DefaultJsonWriter(new JsonTextWriter(Console.Out){
                        Formatting = Formatting.Indented,
                        Indentation = 2,
                        IndentChar = ' '
                        }))
                        {
                        o.WriteTo(writer);
                        }
                    }
                }
            }
        }
    }
