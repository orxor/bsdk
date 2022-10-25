using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using BinaryStudio.PlatformComponents;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Zip;

namespace BinaryStudio.DirectoryServices
    {
    public abstract class DirectoryService : IDirectoryService
        {
        #region M:GetService(Object,Type):Object
        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="Source">An object that host services.</param>
        /// <param name="Service">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="Service"/>.-or- <see langword="null"/> if there is no service object of type <paramref name="Service"/>.</returns>
        public static Object GetService(Object Source, Type Service)
            {
            if (Service == null) { throw new ArgumentNullException(nameof(Service)); }
            if (Source == null) { return null; }
            try
                {
                if (Source.GetType() == Service) { return Source; }
                #region {IDirectoryService}
                if (Service == typeof(IDirectoryService)) {
                    if (Source is Uri Uri) {
                        switch (Uri.Scheme) {
                            case "folder": { return new LocalFolderService(Uri.LocalPath); }
                            case "file"  : { return GetService<IDirectoryService>(GetService<IFileService>(Source)); }
                            default:
                                {
                                throw new NotSupportedException();
                                }
                            }
                        }
                    if (Source is IFileService FileService) {
                        switch (Path.GetExtension(FileService.FileName).ToLowerInvariant()) {
                            case ".rar": { return new ArchiveService(FileService.FullName, RarArchive.Open(FileService.OpenRead()));      }
                            case ".jar": { return new ArchiveService(FileService.FullName, ZipArchive.Open(FileService.OpenRead()));      }
                            case ".zip": { return new ArchiveService(FileService.FullName, ZipArchive.Open(FileService.OpenRead()));      }
                            case ".7z" : { return new ArchiveService(FileService.FullName, SevenZipArchive.Open(FileService.OpenRead())); }
                            default:
                                {
                                throw new NotSupportedException();
                                }
                            }
                        }
                    throw new NotSupportedException();
                    }
                #endregion
                #region {IFileService}
                if (Service == typeof(IFileService)) {
                    if (Source is Uri Uri) {
                        switch (Uri.Scheme) {
                            case "file"  : { return new LocalFileService(Uri.LocalPath); }
                            default:
                                {
                                throw new NotSupportedException();
                                }
                            }
                        }
                    }
                #endregion
                }
            catch(Exception e)
                {
                e.Data["Service"] = Service.FullName;
                e.Data["Source"] = (Source is ISerializable)
                    ? Source
                    : Source.ToString();
                throw;
                }
            if (Service == Source.GetType()) { return Source; }
            return null;
            }
        #endregion
        #region M:GetService<T>(Object):T
        /// <summary>Gets the service object of the specified type.</summary>
        /// <typeparam name="T">An object that specifies the type of service object to get.</typeparam>
        /// <param name="Source">An object that host services.</param>
        /// <returns>A service object of type <cref name="T"/>.-or- <see langword="null"/> if there is no service object of type <cref name="T"/>.</returns>
        public static T GetService<T>(Object Source)
            {
            return (T)GetService(Source, typeof(T));
            }
        #endregion
        #region M:GetService<T>(Object,{out}T):Boolean
        /// <summary>Gets the service object of the specified type.</summary>
        /// <typeparam name="T">An object that specifies the type of service object to get.</typeparam>
        /// <param name="Source">An object that host services.</param>
        /// <param name="Target">A service object of type <cref name="T"/>.-or- <see langword="null"/> if there is no service object of type <cref name="T"/>.</param>
        /// <returns>Returns <see langword="true"/> if specified service resolved;otherwise <see langword="false"/>.</returns>
        public static Boolean GetService<T>(Object Source, out T Target)
            {
            Target = (T)GetService(Source, typeof(T));
            return (Target != null);
            }
        #endregion
        #region M:GetFiles:IEnumerable<IFileService>
        /// <summary>Returns the file services (including their paths) in the directory service.</summary>
        /// <returns>A list of file services.</returns>
        public abstract IEnumerable<IFileService> GetFiles();
        #endregion

        public static IEnumerable<IFileService> GetFiles(IDirectoryService Service,String SearchPattern,String ContainerPattern) {
            if (Service == null) { yield break; }
            if (SearchPattern == null) { throw new ArgumentNullException(nameof(SearchPattern)); }
            ContainerPattern = ContainerPattern ?? String.Empty;
            foreach (var FileService in GetFiles(Service,SearchPattern,ContainerPattern.Split(';'))) {
                yield return FileService;
                }
            }

        public static IEnumerable<IFileService> GetFiles(IDirectoryService Service,String SearchPattern,IList<String> ContainerPatterns) {
            if (Service == null) { yield break; }
            if (SearchPattern == null) { throw new ArgumentNullException(nameof(SearchPattern)); }
            ContainerPatterns = ContainerPatterns ?? EmptyArray<String>.Value;
            foreach (var FileService in Service.GetFiles()) {
                if (PathUtils.IsMatch(SearchPattern,FileService.FullName)) { yield return FileService; }
                if (PathUtils.IsMatch(ContainerPatterns,FileService.FullName)) {
                    var service = GetService<IDirectoryService>(FileService);
                    if (service != null) {
                        foreach (var NestedFileService in GetFiles(service,SearchPattern,ContainerPatterns)) {
                            yield return NestedFileService;
                            }
                        }
                    }
                }
            }
        }
    }