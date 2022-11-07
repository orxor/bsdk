using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using BinaryStudio.PortableExecutable.Win32;
using BinaryStudio.IO;

namespace BinaryStudio.PortableExecutable
    {
    public class MetadataScope : IDisposable
        {
        internal interface IExternalMetadataLibrary
            {

            }

        private const UInt16 IMAGE_DOS_SIGNATURE = 0x5A4D;
        private const UInt32 MSFT_SIGNATURE = 0x5446534D;
        private const UInt32 SLTG_SIGNATURE = 0x47544c53;

        #region M:Dispose(Boolean)
        protected virtual void Dispose(Boolean disposing) {
            if (disposing) {
                }
            }
        #endregion
        #region M:Finalize
        ~MetadataScope() {
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

        static MetadataScope() {

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(MetadataObject)) && !i.IsAbstract)) {
                foreach (var attribute in type.GetCustomAttributes(typeof(MetadataObjectFactoryAttribute), false).OfType<MetadataObjectFactoryAttribute>()) {
                    Factories.Add(attribute);
                    }
                }
            }

        /// <summary>Loads metadata from specified file.</summary>
        /// <param name="filename">File name containing metadata.</param>
        /// <returns>Created or existing metadata object.</returns>
        public unsafe MetadataObject Load(String filename) {
            if (filename == null) { throw new ArgumentNullException(nameof(filename)); }
            if (String.IsNullOrWhiteSpace(filename)) { throw new ArgumentOutOfRangeException(nameof(filename)); }
            MetadataObject r = null;
            var mapping = new FileMappingMemory(new FileMapping(filename));
            using (new DisableWow64FileSystemRedirection()) {
                if (!File.Exists(filename)) { throw new FileNotFoundException(nameof(filename)); }
                using (UpgradeableReadLock(ObjectCacheLock)) {
                    /* TODO: Make file path absolute */
                    var identity = new MetadataObjectIdentity(filename, FileServiceGuid);
                    if (!objects.TryGetValue(identity, out r)) {
                        var type = Recognize(mapping);
                        if (type != null) {
                            var ctor = type.GetConstructor(BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public,null,
                                new []{
                                    typeof(MetadataScope),
                                    typeof(MetadataObjectIdentity)
                                    },null);
                            if (ctor == null) { throw new MissingMethodException(); }
                            using (WriteLock(ObjectCacheLock)) {
                                objects[identity] = r = (MetadataObject)ctor.Invoke(new Object[]{this, identity });
                                r.AttachFileMapping(mapping);
                                }
                            }
                        }
                    }
                }
            if (r == null) { return null; }
            Task.Factory.StartNew(() => r.Load(new [] { (IntPtr)mapping }, mapping.Mapping.Size)).Wait();
            return r;
            }

        public Task<MetadataObject> LoadAsync(String filename, CancellationToken cancellationToken = default(CancellationToken)) {
            if (filename == null) { throw new ArgumentNullException(nameof(filename)); }
            if (String.IsNullOrWhiteSpace(filename)) { throw new ArgumentOutOfRangeException(nameof(filename)); }
            #if NET40
            return Task.Factory.StartNew(()=>Load(filename),cancellationToken);
            #else
            return Task.Run(()=>Load(filename),cancellationToken);
            #endif
            }

        public ITypeLibraryTypeDescriptor TypeOf(VARTYPE source) {
            throw new NotImplementedException();
            }

        public ITypeLibraryTypeDescriptor TypeOf(String source) {
            throw new NotImplementedException();
            }

        public ITypeLibraryDescriptor LoadTypeLibrary(Guid g, Version v, SYSKIND o)
            {
            throw new NotImplementedException();
            }

        public ITypeLibraryDescriptor LoadTypeLibrary(String key) {
            throw new NotImplementedException();
            }

        internal ITypeLibraryDescriptor LoadTypeLibrary(String key, SYSKIND o) {
            throw new NotImplementedException();
            }

        private unsafe Type Recognize(FileMappingMemory source) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            var memory = (Byte*)source;
            if (*((UInt16*)source) == IMAGE_DOS_SIGNATURE) { return typeof(MZMetadataObject); }
            return null;
            }

        public void RegisterType(ITypeLibraryTypeDescriptor type) {
            throw new NotImplementedException();
            }

        private class ReadLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public ReadLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterReadLock();
                }

            public void Dispose()
                {
                o.ExitReadLock();
                o = null;
                }
            }

        private class UpgradeableReadLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public UpgradeableReadLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterUpgradeableReadLock();
                }

            public void Dispose()
                {
                o.ExitUpgradeableReadLock();
                o = null;
                }
            }

        private class WriteLockScope : IDisposable
            {
            private ReaderWriterLockSlim o;
            public WriteLockScope(ReaderWriterLockSlim o)
                {
                this.o = o;
                o.EnterWriteLock();
                }

            public void Dispose()
                {
                o.ExitWriteLock();
                o = null;
                }
            }

        protected internal static IDisposable ReadLock(ReaderWriterLockSlim o)            { return new ReadLockScope(o);            }
        protected internal static IDisposable WriteLock(ReaderWriterLockSlim o)           { return new WriteLockScope(o);           }
        protected internal static IDisposable UpgradeableReadLock(ReaderWriterLockSlim o) { return new UpgradeableReadLockScope(o); }

        private readonly IDictionary<MetadataObjectIdentity,MetadataObject> objects = new ConcurrentDictionary<MetadataObjectIdentity, MetadataObject>();
        private readonly ReaderWriterLockSlim ObjectCacheLock = new ReaderWriterLockSlim();
        private static readonly ISet<MetadataObjectFactoryAttribute> Factories = new HashSet<MetadataObjectFactoryAttribute>();
        public static readonly Guid FileServiceGuid = new Guid("{e34e7255-e143-4573-bb20-22236f17d591}");
        }
    }