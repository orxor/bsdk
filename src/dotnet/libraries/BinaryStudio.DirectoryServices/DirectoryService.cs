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
        public static T GetService<T>(Object source)
            {
            return (T)GetService(source, typeof(T));
            }
        #endregion
        #region M:GetService<T>(Object,{out}T):Boolean
        public static Boolean GetService<T>(Object source, out T r)
            {
            r = (T)GetService(source, typeof(T));
            return r != null;
            }
        #endregion

        public abstract IEnumerable<IFileService> GetFiles(String SearchPattern, DirectoryServiceSearchOptions SearchOption);
        }
    }