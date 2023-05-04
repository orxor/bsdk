using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using BinaryStudio.IO;
using BinaryStudio.PlatformUI.Models;
using BinaryStudio.PlatformUI.Shell;
using BinaryStudio.PortableExecutable;
using BinaryStudio.PortableExecutable.PlatformUI.Models;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.Controls;

internal class DocumentManager
    {
    private readonly DocumentGroup dockgroup;
    private readonly WindowProfile profile;
    public DocumentManager(DocumentGroup dockgroup,WindowProfile profile)
        {
        if (dockgroup == null) { throw new ArgumentNullException(nameof(dockgroup)); }
        this.dockgroup = dockgroup;
        this.profile = profile;
        }

    public void OpenDocument(String filename) {
        if (String.IsNullOrWhiteSpace(filename)) { throw new ArgumentOutOfRangeException(nameof(filename)); }
        var e = Path.GetExtension(filename);
        Object o = null;
        switch (e.ToLowerInvariant()) {
            case ".dll":
            case ".exe":
                var scope = new MetadataScope();
                o = scope.Load(filename);
                break;
            case ".crl":
            case ".cer":
            case ".ber":
            case ".der":
            case ".enc":
                {
                var oo = Asn1Object.Load(new ReadOnlyMemoryMappingStream(File.ReadAllBytes(filename))).ToArray();
                o = oo[0];
                }
                break;
            default:
                {
                var oo = Asn1Object.Load(new ReadOnlyMemoryMappingStream(File.ReadAllBytes(filename))).ToArray();
                o = oo[0];
                }
                break;
            }
        if (o != null) {
            var r = new View {
                Content = new ContentControl { Content = Model.CreateModel(o) },
                Title = Path.GetFileName(filename)
                };
            dockgroup.Children.Add(r);
            r.IsSelected = true;
            r.IsActive = true;
            }
        }

    static DocumentManager()
        {
        Model.RegisterModelTypes(typeof(Module).Assembly);
        Model.RegisterModelTypes(typeof(Asn1Control).Assembly);
        }
    }
