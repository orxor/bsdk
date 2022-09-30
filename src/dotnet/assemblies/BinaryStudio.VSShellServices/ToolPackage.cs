using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace BinaryStudio.VSShellServices
    {
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    public class ToolPackage : AsyncPackage, ICustomTypeDescriptor
        {
        #region M:InitializeAsync(CancellationToken,IProgress<ServiceProgressData>):Task
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress) {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            var ToolWindowId = 0;
            var CommandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            foreach (var type in GetType().Assembly.GetTypes().Where(i => !i.IsAbstract)) {
                foreach (var attribute in type.GetCustomAttributes(typeof(ToolWindowCommandAttribute), false).OfType<ToolWindowCommandAttribute>()) {
                    var ToolWindowType = type;
                    CommandService.AddCommand(new ToolMenuCommand(Execute, new CommandID(Guid.Parse(attribute.CommandSet), attribute.CommandId), ToolWindowType));
                    ToolWindowId++;
                    }
                }
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetAttributes:AttributeCollection
        /// <summary>Returns a collection of custom attributes for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.ComponentModel.AttributeCollection"/> containing the attributes for this object.</returns>
        AttributeCollection ICustomTypeDescriptor.GetAttributes()
            {
            return new AttributeCollection(GetAttributes().ToArray());
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetClassName:String
        /// <summary>Returns the class name of this instance of a component.</summary>
        /// <returns>The class name of the object, or <see langword="null"/> if the class does not have a name.</returns>
        String ICustomTypeDescriptor.GetClassName()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetComponentName:String
        String ICustomTypeDescriptor.GetComponentName()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetConverter:TypeConverter
        /// <summary>Returns a type converter for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.TypeConverter"/> that is the converter for this object, or <see langword="null"/> if there is no <see cref="T:System.ComponentModel.TypeConverter"/> for this object.</returns>
        TypeConverter ICustomTypeDescriptor.GetConverter()
            {
            return TypeDescriptor.GetConverter(GetType());
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetDefaultEvent:EventDescriptor
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetDefaultProperty:PropertyDescriptor
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetEditor(Type):Object
        Object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetEvents):EventDescriptorCollection
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetEvents(Attribute[]):EventDescriptorCollection
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
            {
            throw new NotImplementedException();
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetProperties:PropertyDescriptorCollection
        /// <summary>Returns the properties for this instance of a component.</summary>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties for this component instance.</returns>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() {
            return new PropertyDescriptorCollection(GetProperties().ToArray());
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetProperties(Attribute[]):PropertyDescriptorCollection
        /// <summary>Returns the properties for this instance of a component using the attribute array as a filter.</summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that is used as a filter.</param>
        /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the filtered properties for this component instance.</returns>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes) {
            return new PropertyDescriptorCollection(GetProperties(attributes).ToArray());
            }
        #endregion
        #region M:ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor):Object
        /// <summary>Returns an object that contains the property described by the specified property descriptor.</summary>
        /// <param name="descriptor">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the property whose owner is to be found.</param>
        /// <returns>An <see cref="T:System.Object"/> that represents the owner of the specified property.</returns>
        Object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor descriptor)
            {
            return GetPropertyOwner(descriptor);
            }
        #endregion
        #region M:GetPropertyOwner(PropertyDescriptor):Object
        /// <summary>Returns an object that contains the property described by the specified property descriptor.</summary>
        /// <param name="descriptor">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that represents the property whose owner is to be found.</param>
        /// <returns>An <see cref="T:System.Object"/> that represents the owner of the specified property.</returns>
        protected virtual Object GetPropertyOwner(PropertyDescriptor descriptor) {
            return this;
            }
        #endregion
        #region M:GetProperties:IEnumerable<PropertyDescriptor>
        protected virtual IEnumerable<PropertyDescriptor> GetProperties() {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(GetType())) {
                yield return descriptor;
                }
            }
        #endregion
        #region M:GetProperties(Attribute[]):IEnumerable<PropertyDescriptor>
        protected virtual IEnumerable<PropertyDescriptor> GetProperties(Attribute[] attributes) {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(GetType(), attributes)) {
                yield return descriptor;
                }
            }
        #endregion
        #region M:GetAttributes:IEnumerable<Attribute>
        /// <summary>Returns a collection of custom attributes for this instance of a component.</summary>
        /// <returns>An <see cref="T:System.ComponentModel.AttributeCollection"/> containing the attributes for this object.</returns>
        protected virtual IEnumerable<Attribute> GetAttributes() {
            foreach (Attribute attribute in TypeDescriptor.GetAttributes(GetType())) {
                yield return attribute;
                }
            }
        #endregion
        #region M:CreateToolWindow(Type,Int32,Object):WindowPane
        /// <summary>
        /// Create a tool window of the specified type with the specified ID.
        /// </summary>
        /// <param name="toolWindowType">Type of the window to be created</param>
        /// <param name="id">Instance ID or 0 for single instance toolwindows</param>
        /// <param name="context">Tool window creation context (passed to <see cref="T:Microsoft.VisualStudio.Shell.ToolWindowPane" /> constructor)</param>
        /// <returns>An instance of a class derived from <see cref="T:Microsoft.VisualStudio.Shell.ToolWindowPane" /></returns>
        protected override WindowPane CreateToolWindow(Type toolWindowType, Int32 id, Object context)
            {
            if (toolWindowType == null) { throw new ArgumentNullException(nameof(toolWindowType)); }
            if (!toolWindowType.IsSubclassOf(typeof(WindowPane))) { throw new ArgumentOutOfRangeException(nameof(toolWindowType)); }
            if (id < 0) { throw new ArgumentOutOfRangeException(nameof(id)); }
            return base.CreateToolWindow(toolWindowType, id, context);
            }
        #endregion
        #region M:CreateToolWindow({ref}Guid,Int32):Int32
        /// <devdoc>
        /// Create a tool window of the specified type with the specified ID.
        /// </devdoc>
        /// <param name="toolWindowType">Type of the window to be created</param>
        /// <param name="id">Instance ID or 0 for single instance toolwindows</param>
        /// <returns>HRESULT for toolwindow creation</returns>
        protected override Int32 CreateToolWindow(ref Guid toolWindowType, Int32 id)
            {
            return base.CreateToolWindow(ref toolWindowType, id);
            }
        #endregion
        #region M:Execute(Object,EventArgs)
        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(Object sender, EventArgs e) {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (sender is ToolMenuCommand command) {
                var window = FindToolWindow(command.ToolWindowType, 0, true);
                if ((null != window) && (null != window.Frame)) {
                    var windowFrame = (IVsWindowFrame)window.Frame;
                    ErrorHandler.ThrowOnFailure(windowFrame.Show());
                    }
                }
            }
        #endregion

        static ToolPackage()
            {
            }
        }
    }