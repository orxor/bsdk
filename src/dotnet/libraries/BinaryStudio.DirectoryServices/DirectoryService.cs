using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
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
                if (Service == typeof(IDirectoryService)) {
                    if (Source is IDirectoryService folder) { return folder; }
                    if (Source is Uri uri) {
                        if (uri.Scheme == "file") {
                            if (File.Exists(uri.LocalPath)) {
                                switch (Path.GetExtension(uri.LocalPath).ToLower()) {
                                    case ".rar": { return new ArchiveService(uri.LocalPath, RarArchive.Open(uri.LocalPath));      }
                                    case ".jar":
                                    case ".zip": { return new ArchiveService(uri.LocalPath, ZipArchive.Open(uri.LocalPath));      }
                                    case ".7z" : { return new ArchiveService(uri.LocalPath, SevenZipArchive.Open(uri.LocalPath)); }
                                    default:
                                        {
                                        return null;
                                        }
                                    }
                                }
                            }
                        }
                    else if (Source is String StringValue) {
                        if (StringValue.StartsWith("file://")) { StringValue = StringValue.Substring(7); }
                        if (File.Exists(StringValue)) {
                            switch (Path.GetExtension(StringValue).ToLower()) {
                                case ".rar": { return new ArchiveService(StringValue, RarArchive.Open(StringValue));      }
                                case ".jar":
                                case ".zip": { return new ArchiveService(StringValue, ZipArchive.Open(StringValue));      }
                                case ".7z" : { return new ArchiveService(StringValue, SevenZipArchive.Open(StringValue)); }
                                default:
                                    {
                                    return null;
                                    }
                                }
                            }
                        }
                    }
                }
            catch(Exception e)
                {
                e.Data["Service"] = Service.FullName;
                if (Source is ISerializable)
                    {
                    e.Data["Source"] = Source;
                    }
                else
                    {
                    e.Data["Source"] = Source.ToString();
                    }
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

        /// <summary>Returns the names of file services (including their paths) that match the specified search pattern in the specified directory service, using a value to determine whether to search subdirectories.</summary>
        /// <param name="SearchPattern">The search string to match against the names of file services.</param>
        /// <param name="SearchOption">One of the <see cref="DirectoryServiceSearchOptions"/> values that specifies whether the search operation should include all subdirectories or only the current directory service.</param>
        /// <returns>A list containing the names of file services in the specified directory service that match the specified search pattern. File service names include the full path.</returns>
        public abstract IEnumerable<IFileService> GetFiles(String SearchPattern, DirectoryServiceSearchOptions SearchOption);
        }
    }