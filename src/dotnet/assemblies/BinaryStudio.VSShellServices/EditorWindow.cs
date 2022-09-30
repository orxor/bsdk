using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using BinaryStudio.PlatformUI.Models;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Constants = Microsoft.VisualStudio.OLE.Interop.Constants;
using IDataObject = Microsoft.VisualStudio.OLE.Interop.IDataObject;
using Type = System.Type;

namespace BinaryStudio.VSShellServices
    {
    public abstract class EditorWindow : WindowPane,
        IOleCommandTarget,
        IVsPersistDocData,
        IPersistFileFormat,
        IVsToolboxUser
        {
        /// <summary>
        /// Gets extended rich text box that are hosted.
        /// This is a required override from the Microsoft.VisualStudio.Shell.WindowPane class.
        /// </summary>
        /// <remarks>The resultant handle can be used with Win32 API calls.</remarks>
        public override IWin32Window Window { get { return ElementHost; }}
        protected String FileName { get;private set; }
        protected abstract Int32 FormatIndex { get; }
        protected abstract String FormatList { get; }
        protected internal Guid FactoryClassId { get;set; }
        protected Object DataContext { get;private set; }
        protected Object ModelContext { get;private set; }
        protected ContentControl ElementHostControl { get;private set; }

        protected EditorWindow()
            {
            PrivateInit();
            }

        #region M:IVsPersistDocData.GetGuidEditorType({out}Guid):Int32
        /// <summary>Returns the unique identifier of the editor factory that created the IVsPersistDocData object.</summary>
        /// <param name="ClassId">[out] Pointer to the class identifier of the editor type.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.GetGuidEditorType(out Guid ClassId)
            {
            ThreadHelper.ThrowIfNotOnUIThread();
            return ((IPersistFileFormat)this).GetClassID(out ClassId);
            }
        #endregion
        #region M:IVsPersistDocData.IsDocDataDirty({out}Int32):Int32
        /// <summary>Determines whether the document data has changed since the last save.</summary>
        /// <param name="Dirty">[out] <see langword="true" /> if the document data has been changed.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.IsDocDataDirty(out Int32 Dirty)
            {
            ThreadHelper.ThrowIfNotOnUIThread();
            return ((IPersistFileFormat)this).IsDirty(out Dirty);
            }
        #endregion
        #region M:IVsPersistDocData.SetUntitledDocPath(String):Int32
        /// <summary>Sets the initial name (or path) for unsaved, newly created document data.</summary>
        /// <param name="DocDataPath">[in] String indicating the path of the document. Most editors can ignore this parameter. It exists for historical reasons.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.SetUntitledDocPath(String DocDataPath)
            {
            ThreadHelper.ThrowIfNotOnUIThread();
            return ((IPersistFileFormat)this).InitNew(0);
            }
        #endregion
        #region M:IVsPersistDocData.LoadDocData(String):Int32
        /// <summary>Loads the document data from a given MkDocument.</summary>
        /// <param name="MkDocument">[in] Path to the document file name to be loaded.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.LoadDocData(String MkDocument)
            {
            ThreadHelper.ThrowIfNotOnUIThread();
            return ((IPersistFileFormat)this).Load(MkDocument, 0, 0);
            }
        #endregion
        #region M:IVsPersistDocData.SaveDocData(VSSAVEFLAGS,{out}String,{out}Int32):Int32
        /// <summary>Saves the document data.</summary>
        /// <param name="Save">[in] Flags whose values are taken from the <see cref="T:Microsoft.VisualStudio.Shell.Interop.VSSAVEFLAGS" /> enumeration.</param>
        /// <param name="MkDocumentNew">[out] Pointer to the path to the new document.</param>
        /// <param name="SaveCanceled">[out] <see langword="true" /> if the document was not saved.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.SaveDocData(VSSAVEFLAGS Save, out String MkDocumentNew, out Int32 SaveCanceled)
            {
            MkDocumentNew = null;
            SaveCanceled = 1;
            return (Int32)Constants.OLECMDERR_E_NOTSUPPORTED;
            }
        #endregion
        #region M:IVsPersistDocData.Close:Int32
        /// <summary>Closes the IVsPersistDocData object.</summary>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.Close() {
            if (ElementHost != null) {
                ElementHost.Dispose();
                ElementHost = null;
                }
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IVsPersistDocData.OnRegisterDocData(UInt32,IVsHierarchy,UInt32):Int32
        /// <summary>Called by the Running Document Table (RDT) when it registers the document data in the RDT.</summary>
        /// <param name="DocCookie">[in] Abstract handle for the document to be registered. See the <see langword="VSDOCCOOKIE" /> datatype.</param>
        /// <param name="HierNew">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface.</param>
        /// <param name="ItemIdNew">[in] Item identifier of the document to be registered from VSITEM.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.OnRegisterDocData(UInt32 DocCookie, IVsHierarchy HierNew, UInt32 ItemIdNew)
            {
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IVsPersistDocData.RenameDocData(UInt32,IVsHierarchy,UInt32,String):Int32
        /// <summary>Renames the document data.</summary>
        /// <param name="Attribs">[in] File attribute of the document data to be renamed. See the data type <see cref="T:Microsoft.VisualStudio.Shell.Interop.__VSRDTATTRIB" />.</param>
        /// <param name="HierNew">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface of the document being renamed.</param>
        /// <param name="ItemIdNew">[in] Item identifier of the document being renamed. See the data type <see langword="VSITEMID" />.</param>
        /// <param name="MkDocumentNew">[in] Path to the document being renamed.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.RenameDocData(UInt32 Attribs, IVsHierarchy HierNew, UInt32 ItemIdNew, String MkDocumentNew)
            {
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IVsPersistDocData.IsDocDataReloadable({out}Int32):Int32
        /// <summary>Determines whether the document data can be reloaded.</summary>
        /// <param name="Reloadable">[out] <see langword="true"/> if the document data can be reloaded.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.IsDocDataReloadable(out Int32 Reloadable)
            {
            Reloadable = 1;
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IVsPersistDocData.ReloadDocData(UInt32):Int32
        /// <summary>Reloads the document data and in the process determines whether to ignore a subsequent file change.</summary>
        /// <param name="Flags">[in] Flag indicating whether to ignore the next file change when reloading the document data. See the data type <see cref="T:Microsoft.VisualStudio.Shell.Interop._VSRELOADDOCDATA" />.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        Int32 IVsPersistDocData.ReloadDocData(UInt32 Flags)
            {
            ThreadHelper.ThrowIfNotOnUIThread();
            return ((IPersistFileFormat)this).Load(null, Flags, 0);
            }
        #endregion
        #region M:IPersist.GetClassID({out}Guid):Int32
        /// <summary>Retrieves the class identifier (CLSID) of an object.</summary>
        /// <param name="ClassId">[out] Pointer to the location of the CLSID on return.</param>
        /// <returns>S_OK if the method succeeds.</returns>
        Int32 IPersist.GetClassID(out Guid ClassId)
            {
            ClassId = FactoryClassId;
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IPersistFileFormat.IsDirty({out}Int32):Int32
        /// <summary>Determines whether an object has changed since being saved to its current file.</summary>
        /// <param name="Dirty">[out] <see langword="true"/> if the document content changed.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        Int32 IPersistFileFormat.IsDirty(out Int32 Dirty)
            {
            Dirty = 0;
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IPersistFileFormat.InitNew(UInt32):Int32
        /// <summary>Instructs the object to initialize itself in the untitled state.</summary>
        /// <param name="FormatIndex">[in] Index value that indicates the current format of the file. The <paramref name="FormatIndex" /> parameter controls the beginning format of the file. The caller should pass DEF_FORMAT_INDEX if the object is to choose its default format. If this parameter is non-zero, then it is interpreted as the index into the list of formats, as returned by a call to <see cref="M:Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat.GetFormatList(System.String@)" />. An index value of 0 indicates the first format, 1 the second format, and so on.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        Int32 IPersistFileFormat.InitNew(UInt32 FormatIndex)
            {
            return (Int32)Constants.OLECMDERR_E_NOTSUPPORTED;
            }
        #endregion
        #region M:IPersistFileFormat.Load(String,UInt32,Int32):Int32
        /// <summary>Opens a specified file and initializes an object from the file contents.</summary>
        /// <param name="filename">[in] Pointer to the name of the file to load, which, for an existing file, should always include the full path.</param>
        /// <param name="Mode">[in] File format mode. If zero, the object uses the usual defaults as if the user had opened the file.</param>
        /// <param name="ReadOnly">[in] <see langword="true"/> indicates that the file should be opened as read-only.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        public virtual Int32 Load(String filename, UInt32 Mode, Int32 ReadOnly)
            {
            FileName = filename;
            DataContext = LoadDataContext();
            if (DataContext == null) { return VSConstants.E_FAIL; }
            ModelContext = null;
            if (Types.TryGetValue(DataContext.GetType(), out var type)) {
                var ctor = type.GetConstructor(BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public,null,
                    new []{
                        DataContext.GetType()
                        },null);
                if (ctor != null) {
                    ModelContext = ctor.Invoke(new Object[]{ DataContext });
                    }
                }
            if (ModelContext == null) {
                var interfaces = DataContext.GetType().GetInterfaces();
                if (interfaces.Length > 0) {
                    for (var i = interfaces.Length-1; i >= 0;i--) {
                        if (Types.TryGetValue(interfaces[i], out type)) {
                            var ctor = type.GetConstructor(BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public,null,
                                new []{
                                    DataContext.GetType()
                                    },null);
                            if (ctor != null) {
                                ModelContext = ctor.Invoke(new Object[]{ DataContext });
                                break;
                                }
                            }
                        }
                    }
                }
            ElementHostControl.Content = ModelContext ?? DataContext;
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IPersistFileFormat.Save(String,Int32,UInt32):Int32
        /// <summary>Saves a copy of the object into the specified file.</summary>
        /// <param name="Filename">[in] Pointer to the file name. The <paramref name="Filename"/> parameter can be <see langword="null" />; it instructs the object to save using its current file. If the object is in the untitled state and <see langword="null" /> is passed as the <paramref name="Filename" />, the object returns <see cref="F:Microsoft.VisualStudio.VSConstants.E_INVALIDARG" />. You must specify a valid file name parameter in this situation.</param>
        /// <param name="Remember">[in] Boolean value that indicates whether the <paramref name="Filename"/> parameter is to be used as the current working file. If <see langword="true"/>, <paramref name="Filename" /> becomes the current file and the object should clear its dirty flag after the save. If <see langword="false" />, this save operation is a Save a Copy As operation. In this case, the current file is unchanged and the object does not clear its dirty flag. If <paramref name="Filename" /> is <see langword="null" />, the implementation ignores the <paramref name="Remember" /> flag.</param>
        /// <param name="FormatIndex">[in] Value that indicates the format in which the file will be saved. The caller passes DEF_FORMAT_INDEX if the object is to choose its default (current) format. If set to non-zero, the value is interpreted as the index into the list of formats, as returned by a call to the method <see cref="M:Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat.GetFormatList(System.String@)" />. An index value of 0 indicates the first format, 1 the second format, and so on.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        Int32 IPersistFileFormat.Save(String Filename, Int32 Remember, UInt32 FormatIndex)
            {
            return (Int32)Constants.OLECMDERR_E_NOTSUPPORTED;
            }
        #endregion
        #region M:IPersistFileFormat.SaveCompleted(String):Int32
        /// <summary>Notifies the object that it has concluded the Save transaction and that the object can write to its file.</summary>
        /// <param name="Filename">[in] Pointer to the file name.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        Int32 IPersistFileFormat.SaveCompleted(String Filename)
            {
            return (Int32)Constants.OLECMDERR_E_NOTSUPPORTED;
            }
        #endregion
        #region M:IPersistFileFormat.GetCurFile({out}String,{out}UInt32):Int32
        /// <summary>Returns the path to an object's current working file, or, if there is not a current working file, the object's default file name prompt.</summary>
        /// <param name="Filename">[out] Pointer to the file name. If the object has a valid file name, the file name is returned as the <paramref name="Filename" /> out parameter. If the object is in the untitled state, <see langword="null" /> is returned as the <paramref name="Filename" /> out parameter.
        /// Note   This result differs from that of the standard <see cref="M:Microsoft.VisualStudio.OLE.Interop.IPersistFile.GetCurFile(System.String@)" /> method, which returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_FALSE" /> and a "Save As" prompt string.</param>
        /// <param name="FormatIndex">[out] Value that indicates the current format of the file. This value is interpreted as a zero-based index into the list of formats, as returned by a call to <see cref="M:Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat.GetFormatList(System.String@)" />. An index value of zero indicates the first format, 1 the second format, and so on. If the object supports only a single format, it returns zero. Subsequently, it returns a single element in its format list through a call to <see cref="M:Microsoft.VisualStudio.Shell.Interop.IPersistFileFormat.GetFormatList(System.String@)" />.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IPersistFileFormat.GetCurFile(out String Filename, out UInt32 FormatIndex)
            {
            Filename = FileName;
            FormatIndex = (UInt32)this.FormatIndex;
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IPersistFileFormat.GetFormatList({out}String):Int32
        /// <summary>
        /// Provides the caller with the information necessary to open the standard common "Save As" dialog box. 
        /// This returns an enumeration of supported formats, from which the caller selects the appropriate format. 
        /// Each string for the format is terminated with a newline (\n) character. 
        /// The last string in the buffer must be terminated with the newline character as well. 
        /// The first string in each pair is a display string that describes the filter, such as "Text Only 
        /// (*.txt)". The second string specifies the filter pattern, such as "*.txt". To specify multiple filter 
        /// patterns for a single display string, use a semicolon to separate the patterns: "*.htm;*.html;*.asp". 
        /// A pattern string can be a combination of valid file name characters and the asterisk (*) wildcard character. 
        /// Do not include spaces in the pattern string. The following string is an example of a file pattern string: 
        /// "HTML File (*.htm; *.html; *.asp)\n*.htm;*.html;*.asp\nText File (*.txt)\n*.txt\n."
        /// </summary>
        /// <param name="FormatList">Pointer to a string that contains pairs of format filter strings.</param>
        /// <returns>S_OK if the method succeeds.</returns>
        Int32 IPersistFileFormat.GetFormatList(out String FormatList)
            {
            FormatList = this.FormatList;
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IPersistFileFormat.GetClassID({out}Guid):Int32
        /// <summary />
        /// <param name="ClassId">[out] Points to the location of the CLSID on return. The CLSID is a globally unique identifier (GUID) that uniquely represents an object class that defines the code that can manipulate the object's data.</param>
        Int32 IPersistFileFormat.GetClassID(out Guid ClassId)
            {
            ThreadHelper.ThrowIfNotOnUIThread();
            ((IPersist)this).GetClassID(out ClassId);
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IVsToolboxUser.IsSupported(IDataObject):Int32
        /// <summary>Determines whether the Toolbox user supports the referenced data object.</summary>
        /// <param name="DO">[in] Data object to be supported.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"/>. If it fails, it returns an error code.</returns>
        Int32 IVsToolboxUser.IsSupported(IDataObject DO)
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:IVsToolboxUser.ItemPicked(IDataObject):Int32
        /// <summary>Sends notification that an item in the Toolbox is selected through a click, or by pressing ENTER.</summary>
        /// <param name="DO">[in] Data object that is selected.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsToolboxUser.ItemPicked(IDataObject DO)
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:IOleCommandTarget.QueryStatus({ref}Guid,UInt32,OLECMD[],IntPtr):Int32
        /// <summary>Queries the object for the status of one or more commands generated by user interface events.</summary>
        /// <param name="pguidCmdGroup">The GUID of the command group.</param>
        /// <param name="cCmds">The number of commands in <paramref name="prgCmds"/>.</param>
        /// <param name="prgCmds">An array of <see cref="T:Microsoft.VisualStudio.OLE.Interop.OLECMD"/> structures that indicate the commands for which the caller needs status information. This method fills the <cref name="Microsoft.VisualStudio.OLE.Interop.OLECMD.cmdf"/> member of each structure with values taken from the <see cref="T:Microsoft.VisualStudio.OLE.Interop.OLECMDF"/> enumeration.</param>
        /// <param name="pCmdText">An <see cref="T:Microsoft.VisualStudio.OLE.Interop.OLECMDTEXT"/> structure in which to return name and/or status information of a single command. This parameter can be null to indicate that the caller does not need this information.</param>
        /// <returns>This method returns S_OK on success. Other possible return values include the following.Return codeDescriptionE_FAILThe operation failed.E_UNEXPECTEDAn unexpected error has occurred.E_POINTERThe <paramref name="prgCmds"/> argument is null.OLECMDERR_E_UNKNOWNGROUPThe <paramref name="pguidCmdGroup"/> parameter is not null but does not specify a recognized command group.</returns>
        Int32 IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, UInt32 cCmds, OLECMD[] prgCmds, IntPtr pCmdText) {
            if (prgCmds == null || cCmds != 1) { return VSConstants.E_INVALIDARG; }
            OLECMDF cmdf = 0;
            if (pguidCmdGroup == VSConstants.GUID_VSStandardCommandSet97) {
                switch ((VSConstants.VSStd97CmdID)prgCmds[0].cmdID) {
                    case VSConstants.VSStd97CmdID.Save:
                    case VSConstants.VSStd97CmdID.SaveAs:
                    case VSConstants.VSStd97CmdID.SaveProjectItem:
                    case VSConstants.VSStd97CmdID.SaveProjectItemAs:
                        {
                        cmdf = OLECMDF.OLECMDF_SUPPORTED;
                        }
                        break;
                    }
                }
            if ((cmdf & OLECMDF.OLECMDF_SUPPORTED) != OLECMDF.OLECMDF_SUPPORTED) { return (Int32)Constants.OLECMDERR_E_NOTSUPPORTED; }
            prgCmds[0].cmdf = (UInt32)cmdf;
            return VSConstants.S_OK;
            }
        #endregion

        #region M:PrivateInit
        private void PrivateInit()
            {
            ElementHostControl = CreateElementHostControl();
            ElementHost = new ElementHost{
                Dock = DockStyle.Fill,
                Child = ElementHostControl
                };
            }
        #endregion
        #region M:Initialize
        /// <summary>
        /// This method is called when the pane is sited with a non null service provider.
        /// Here is where you can do all the initialization that requare access to
        /// services provided by the shell.
        /// </summary>
        protected override void Initialize()
            {
            }
        #endregion
        #region M:Dispose(Boolean)
        /// <devdoc>Called when this window pane is being disposed.</devdoc>
        protected override void Dispose(Boolean disposing) {
            if (ElementHost != null) {
                ElementHost.Child = null;
                ElementHost.Dispose();
                ElementHost = null;
                }
            ElementHostControl = null;
            base.Dispose(disposing);
            }
        #endregion

        static EditorWindow()
            {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(NotifyPropertyChangedDispatcherObject)) && !i.IsAbstract)) {
                foreach (var attribute in type.GetCustomAttributes(typeof(ModelAttribute), false).OfType<ModelAttribute>()) {
                    Types[attribute.Type] = type;
                    }
                }
            }

        #region M:CreateElementHostControl:ContentControl
        protected virtual ContentControl CreateElementHostControl()
            {
            return new ContentControl();
            }
        #endregion

        protected abstract Object LoadDataContext();

        private ElementHost ElementHost;
        private static readonly IDictionary<Type,Type> Types = new Dictionary<Type,Type>();
        }
    }