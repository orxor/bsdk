using System;
using System.IO;
using System.Windows.Controls;
using BinaryStudio.PlatformUI.Models;
using BinaryStudio.PlatformUI.Shell;
using BinaryStudio.PortableExecutable;
using BinaryStudio.PortableExecutable.PlatformUI.Models;

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
        var E = Path.GetExtension(filename);
        Object o = null;
        switch (E.ToLowerInvariant()) {
            case ".dll":
            case ".exe":
                var scope = new MetadataScope();
                o = scope.Load(filename);
                break;
            }
        if (o != null) {
            var r = new View();
            r.Content = new ContentControl {
                Content = Model.CreateModel(o)
                };
            dockgroup.Children.Add(r);
            r.IsSelected = true;
            r.IsActive = true;
            }
        }

    static DocumentManager()
        {
        Model.RegisterModelTypes(typeof(EMZMetadataObject).Assembly);
        }
    }
