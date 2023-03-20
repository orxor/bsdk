using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryStudio.DirectoryServices;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.CryptographicMessageSyntax;

internal class Program
    {
    private static void Main(String[] args)
        {
        ProcessB("20220316.hexcsv");
        ProcessB("20220426.hexcsv");
        ProcessB("20220511.hexcsv");
        }

    #region M:ProcessA(String)
    private static void ProcessA(String filename)
        {
        Process(new HexGroupServiceA(new LocalFileService(filename)));
        }
    #endregion
    #region M:ProcessB(String)
    private static void ProcessB(String filename)
        {
        Process(new HexGroupServiceB(new LocalFileService(filename)));
        }
    #endregion
    #region M:Process(IDirectoryService)
    private static void Process(IDirectoryService Service) {
        Process(Service.GetFiles());
        }
    #endregion

    private static void Process(IEnumerable<IFileService> Files) {
        var FileIndex = 0;
        foreach (var file in Files) {
            var InputBytes = file.ReadAllBytes();
            var o = Asn1Object.Load(InputBytes).FirstOrDefault();
            if ((o != null) && (!o.IsFailed) && (o.Count == 1)) {
                using (var Message = new CmsMessage(o[0])) {
                    if (!Message.IsFailed) {
                        var ContentInfo = (CmsSignedDataContentInfo)Message.GetService(typeof(CmsSignedDataContentInfo));
                        if (ContentInfo != null) {
                            foreach (var Certificate in ContentInfo.Certificates) {
                                if (Certificate.Country == "ru") {
                                    File.WriteAllBytes($"{Certificate.FriendlyName}.cer",Certificate.Body);
                                    Console.WriteLine(Certificate.FriendlyName);
                                    FileIndex++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

