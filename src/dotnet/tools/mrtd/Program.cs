using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using BinaryStudio.DirectoryServices;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.CryptographicMessageSyntax;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office.Visio;
using DocumentFormat.OpenXml.Spreadsheet;

internal class Program
    {
    private static void Main(String[] args)
        {
        //ProcessA("результат25.11.21.bin");
        //ProcessB("20220316.hexcsv");
        //ProcessB("20220426.hexcsv");
        //ProcessB("20220511.hexcsv");
        F1();
        }

    private static XDocument GetXMLFromPart(PackagePart packagePart)
        {
        XDocument partXml = null;
        // Open the packagePart as a stream and then 
        // open the stream in an XDocument object.
        Stream partStream = packagePart.GetStream();
        partXml = XDocument.Load(partStream);
        return partXml;
        }

    private static void Serialize(IServiceProvider InputValue,out String OutputValue) {
        var builder = new StringBuilder();
        using (var Writer = XmlWriter.Create(builder, new XmlWriterSettings{
            IndentChars = " ",
            Indent = true
            }))
            {
            var o = (OpenXmlServiceElement)InputValue.GetService(typeof(OpenXmlServiceElement));
            o.WriteTo(Writer);
            }
        OutputValue = builder.ToString();
        }

    private static void F1() {
        Serialize(new Shape{
            Name = "Rectangle",
            Text = new TextType("Hello World!")
            }, out var o);
        var nsmgr = new XmlNamespaceManager(new NameTable());
        nsmgr.AddNamespace("a","http://schemas.microsoft.com/office/visio/2012/main");
        using (Package fPackage = Package.Open(@"C:\Users\maistrenko\Documents\Drawing5.vsdx",FileMode.Open,FileAccess.ReadWrite)) {
            PackagePartCollection fParts = fPackage.GetParts();
            var part = fParts?.OfType<PackagePart>().FirstOrDefault(i => i.Uri.ToString() == "/visio/pages/page1.xml");
            if (part != null) {
                var document = part.GetDocument();
                var shapes = document.XPathSelectElement("a:PageContents/a:Shapes",nsmgr);
                if (shapes != null) {
                    shapes.Add((XElement)(new Shape{
                        Id = 10,
                        Name = "Rectangle",
                        Text = new TextType("Hello World!"),
                        PinX = 0.0,
                        PinY = 0.0,
                        Width = 5,
                        Height = 5
                        }));
                    }
                using (var writer = XmlWriter.Create(part.GetStream(), new XmlWriterSettings{
                    Encoding = Encoding.UTF8
                    }))
                    {
                    document.WriteTo(writer);
                    writer.Flush();
                    writer.Close();
                    }
                }
            }
        return;
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
        var folder = Directory.GetCurrentDirectory();
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
                                //if (Certificate.Country == "ru")
                                    {
                                    try
                                        {
                                        var TargetFileName = Path.Combine(folder,$"{Certificate.FriendlyName}.cer");
                                        File.WriteAllBytes($@"\\?\{TargetFileName}",Certificate.Body);
                                        }
                                    catch (Exception e)
                                        {
                                        File.WriteAllBytes($@"{Certificate.Thumbprint}.cer",Certificate.Body);
                                        }
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

