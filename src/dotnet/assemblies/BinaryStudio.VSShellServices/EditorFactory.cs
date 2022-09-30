using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace BinaryStudio.VSShellServices
    {
    public abstract class EditorFactory : IVsEditorFactory, IDisposable
        {
        private ServiceProvider VSServiceProvider;

        /// <summary>Used by the editor factory architecture to create editors that support data/view separation.</summary>
        protected abstract EditorWindow CreateEditorInstance();

        #region M:IVsEditorFactory.CreateEditorInstance:Int32
        /// <summary>Used by the editor factory architecture to create editors that support data/view separation.</summary>
        /// <param name="grfCreateDoc">[in] Flags whose values are taken from the <see cref="T:Microsoft.VisualStudio.Shell.Interop.__VSCREATEEDITORFLAGS" /> enumeration that defines the conditions under which to create the editor. Only open and silent flags are valid.</param>
        /// <param name="pszMkDocument">[in] String form of the moniker identifier of the document in the project system. In the case of documents that are files, this is always the path to the file. This parameter can also be used to specify documents that are not files. For example, in a database-oriented project, this parameter could contain a string that refers to records in a table.</param>
        /// <param name="pszPhysicalView">[in] Name of the physical view. See Remarks for details.</param>
        /// <param name="pvHier">[in] Pointer to the <see cref="T:Microsoft.VisualStudio.Shell.Interop.IVsHierarchy" /> interface.</param>
        /// <param name="itemid">[in] Item identifier of this editor instance.</param>
        /// <param name="punkDocDataExisting">[in] Must be the <paramref name="ppunkDocData" /> object that is registered in the Running Document Table (RDT). This parameter is used to determine if a document buffer (<see langword="DocData" /> object) has already been created. When an editor factory is asked to create a secondary view, then this parameter will be non-null indicating that there is no document buffer. If the file is open, return VS_E_INCOMPATIBLEDOCDATA and the environment will ask the user to close it.</param>
        /// <param name="ppunkDocView">[out] Pointer to the <see langword="IUnknown " />interface for the <see langword="DocView" /> object. Returns <see langword="null" /> if an external editor exists, otherwise returns the view of the document.</param>
        /// <param name="ppunkDocData">[out] Pointer to the <see langword="IUnknown" /> interface for the <see langword="DocData" /> object. Returns the buffer for the document.</param>
        /// <param name="pbstrEditorCaption">[out] Initial caption defined by the document editor for the document window. This is typically a string enclosed in square brackets, such as "[Form]". This value is passed as an input parameter to the <see cref="M:Microsoft.VisualStudio.Shell.Interop.IVsUIShell.CreateDocumentWindow(System.UInt32,System.String,Microsoft.VisualStudio.Shell.Interop.IVsUIHierarchy,System.UInt32,System.IntPtr,System.IntPtr,System.Guid@,System.String,System.Guid@,Microsoft.VisualStudio.OLE.Interop.IServiceProvider,System.String,System.String,System.Int32[],Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@)" /> method. If the file is [ReadOnly] the caption will be set during load of the file.</param>
        /// <param name="pguidCmdUI">[out] Returns the Command UI GUID. This GUID is active when this editor is activated. Any UI element that is visible in the editor has to use this GUID. This GUID is used in the .ctc file in the satellite DLL where it indicates which menus and toolbars should be displayed when the document is active.</param>
        /// <param name="pgrfCDW">[out, retval] enum of type <see cref="T:Microsoft.VisualStudio.Shell.Interop.__VSEDITORCREATEDOCWIN" />. These flags are passed to <see cref="M:Microsoft.VisualStudio.Shell.Interop.IVsUIShell.CreateDocumentWindow(System.UInt32,System.String,Microsoft.VisualStudio.Shell.Interop.IVsUIHierarchy,System.UInt32,System.IntPtr,System.IntPtr,System.Guid@,System.String,System.Guid@,Microsoft.VisualStudio.OLE.Interop.IServiceProvider,System.String,System.String,System.Int32[],Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame@)" />.</param>
        /// <returns>If the document has a format that cannot be opened in the editor, <see cref="F:Microsoft.VisualStudio.Shell.Interop.VSErrorCodes.VS_E_UNSUPPORTEDFORMAT" /> is returned.If the document is open in an incompatible editor (or <see cref="F:Microsoft.VisualStudio.VSConstants.E_NOINTERFACE" />), <see cref="F:Microsoft.VisualStudio.Shell.Interop.VSErrorCodes.VS_E_INCOMPATIBLEDOCDATA" /> is returned.If the file could not be opened for any other reason, another HRESULT error code is returned.</returns>
        Int32 IVsEditorFactory.CreateEditorInstance(UInt32 grfCreateDoc, String pszMkDocument, String pszPhysicalView, IVsHierarchy pvHier,
            UInt32 itemid, IntPtr punkDocDataExisting, out IntPtr ppunkDocView, out IntPtr ppunkDocData,
            out String pbstrEditorCaption, out Guid pguidCmdUI, out Int32 pgrfCDW)
            {
            ppunkDocView = IntPtr.Zero;
            ppunkDocData = IntPtr.Zero;
            pguidCmdUI = GetType().GUID;
            pgrfCDW = 0;
            pbstrEditorCaption = null;
            if ((grfCreateDoc & (VSConstants.CEF_OPENFILE | VSConstants.CEF_SILENT)) == 0) { return VSConstants.E_INVALIDARG; }
            if (punkDocDataExisting != IntPtr.Zero) { return VSConstants.VS_E_INCOMPATIBLEDOCDATA; }
            var Editor = CreateEditorInstance();
            ppunkDocView = Marshal.GetIUnknownForObject(Editor);
            ppunkDocData = Marshal.GetIUnknownForObject(Editor);
            pbstrEditorCaption = String.Empty;
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IVsEditorFactory.SetSite(IServiceProvider):Int32
        /// <summary>Initializes an editor in the environment.</summary>
        /// <param name="psp">[in] Pointer to the <see cref="T:System.IServiceProvider" /> interface which can be used by the factory to obtain other interfaces.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsEditorFactory.SetSite(IServiceProvider psp)
            {
            VSServiceProvider = new ServiceProvider(psp);
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IVsEditorFactory.Close:Int32
        /// <summary>Releases all cached interface pointers and unregisters any event sinks.</summary>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsEditorFactory.Close()
            {
            return VSConstants.S_OK;
            }
        #endregion
        #region M:IVsEditorFactory.MapLogicalView({ref}Guid,{out}String):Int32
        /// <summary>Maps a logical view to a physical view.</summary>
        /// <param name="rguidLogicalView">[in] Unique identifier of the logical view.</param>
        /// <param name="pbstrPhysicalView">[out, retval] Pointer to the physical view to which the logical view is to be mapped.</param>
        /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
        Int32 IVsEditorFactory.MapLogicalView(ref Guid rguidLogicalView, out String pbstrPhysicalView)
            {
            pbstrPhysicalView = null;   // initialize out parameter
            return (VSConstants.LOGVIEWID_Primary == rguidLogicalView)
                ? VSConstants.S_OK
                : VSConstants.E_NOTIMPL;
            }
        #endregion
        #region M:Finalize
        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~EditorFactory()
            {
            Dispose(false);
            }
        #endregion

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
            {
            Dispose(true);
            }

        protected virtual void Dispose(Boolean disposing)
            {
            }
        }
    }
