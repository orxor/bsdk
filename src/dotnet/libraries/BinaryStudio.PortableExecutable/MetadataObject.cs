using System;
using System.Threading;
using System.Threading.Tasks;
using BinaryStudio.IO;
using BinaryStudio.Serialization;
using Newtonsoft.Json;

namespace BinaryStudio.PortableExecutable
    {
    public class MetadataObject : IDisposable, IServiceProvider, IJsonSerializable
        {
        public virtual MetadataObjectState State { get { return state; }}
        public MetadataScope Scope { get; }
        public MetadataObjectIdentity Identity { get; }
        public FileMappingMemory Mapping { get;private set; }

        protected MetadataObject(MetadataScope scope, MetadataObjectIdentity identity)
            {
            if (scope == null)    { throw new ArgumentNullException(nameof(scope));    }
            Scope = scope;
            Identity = identity;
            }

        #region M:Dispose(Boolean)
        /// <summary>Releases the unmanaged resources used by the <see cref="MetadataObject"/> and optionally releases the managed resources.</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing) {
            state = MetadataObjectState.Disposed;
            if (!Disposed) {
                try
                    {
                    if (disposing) {
                        Mapping = null;
                        }
                    }
                finally
                    {
                    Disposed = true;
                    }
                }
            }
        #endregion
        #region M:Finalize
        ~MetadataObject() {
            Dispose(false);
            }
        #endregion
        #region M:IDisposable.Dispose
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }
        #endregion
        #region M:IServiceProvider.GetService(Type):Object
        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="type">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="type"/>. -or- <see langword="null" /> if there is no service object of type <paramref name="type"/>.</returns>
        public virtual Object GetService(Type type)
            {
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (type == GetType()) { return this; }
            return null;
            }
        #endregion

        #region M:LoadAsync(IntPtr[],Int64,CancellationToken):Task
        public Task LoadAsync(IntPtr[] source,Int64 length, CancellationToken cancellationToken = default(CancellationToken))
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source.Length == 0) { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
            if (State != MetadataObjectState.Pending) { throw new InvalidOperationException(); }
            #if NET40
            return Task.Factory.StartNew(delegate{ Load(source,length); },cancellationToken);
            #else
            return Task.Run(delegate{ Load(source,length); },cancellationToken);
            #endif
            }
        #endregion
        #region M:Load(IntPtr[],Int64)
        public void Load(IntPtr[] source,Int64 length)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (source.Length == 0) { throw new ArgumentOutOfRangeException(nameof(source)); }
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
            if (State != MetadataObjectState.Pending) { throw new InvalidOperationException(); }
            try
                {
                state = MetadataObjectState.Loading;
                LoadCore(source, length);
                state = MetadataObjectState.Loaded;
                }
            catch
                {
                state = MetadataObjectState.Failed;
                }
            }
        #endregion
        #region M:LoadCore(IntPtr[],Int64)
        /// <summary>Loads content from specified source.</summary>
        /// <param name="source">Content specific source addresses depending on its type.</param>
        /// <param name="length">Length of content.</param>
        protected virtual void LoadCore(IntPtr[] source,Int64 length)
            {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
            }
        #endregion
        #region M:AttachFileMapping(FileMappingMemory)
        public virtual void AttachFileMapping(FileMappingMemory mapping)
            {
            Mapping = mapping;
            }
        #endregion
        #region M:WriteTo(IJsonWriter)
        public virtual void WriteTo(IJsonWriter writer)
            {
            }
        #endregion

        private MetadataObjectState state = MetadataObjectState.Pending;
        private Boolean Disposed;
        }
    }