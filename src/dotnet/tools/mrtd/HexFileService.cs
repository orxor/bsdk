﻿using System;
using System.IO;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DirectoryServices;

internal class HexFileService : IFileService
    {
    public Byte[] Body { get; }
    public String IdentifyDocumentId { get; }
    public String Folder { get; }
    public Int32 FileIndex { get; }

    public HexFileService(Byte[] body, String fileid, Int32 fileindex, String folder)
        {
        Body = body;
        FileIndex = fileindex;
        Folder = folder;
        if (!String.IsNullOrWhiteSpace(fileid)) {
            IdentifyDocumentId = fileid.ToLowerInvariant();
            IdentifyDocumentId = (new String('0', Math.Max(10-IdentifyDocumentId.Length, 0))) + IdentifyDocumentId;
            }
        else
            {
            IdentifyDocumentId = FileIndex.ToString("x10");
            }
        }

    public String FileName { get {
        return (String.IsNullOrWhiteSpace(EFName))
            ? $"{IdentifyDocumentId}.hex"
            : $"{IdentifyDocumentId}{{{EFName}}}.hex";
        }}

    public String FullName { get { return Path.Combine(Folder,FileName); }}
    public String DocumentCategoryName { get; set; }
    public String CountryName { get; set; }
    public String CountryICAO { get; set; }
    public String Year { get; set; }
    public String Month { get; set; }
    public String RegisterCode { get; set; }
    public String RegisterNumber { get; set; }
    public String IssueDate { get; set; }
    public String ValidToDate { get; set; }
    public String InscribeId { get; set; }
    public String DataFormatId { get; set; }
    public String DataTypeId { get; set; }
    public String Order { get; set; }
    public String Dense { get; set; }
    public Int32? XmlSize { get;set; }
    public String EFName { get;set; }

    Byte[] IFileService.ReadAllBytes()
        {
        return Body ?? new Byte[0];
        }

    Stream IFileService.OpenRead()
        {
        return new MemoryStream(Body ?? new Byte[0]);
        }

    /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
    /// <param name="target">The name of the destination file. This cannot be a directory.</param>
    /// <param name="overwrite"><see langword="true"/> if the destination file can be overwritten; otherwise, <see langword="false"/>.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. -or-  <paramref name="target"/> is read-only.</exception>
    /// <exception cref="T:System.ArgumentException"><paramref name="target"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.  -or-  <paramref name="target"/> specifies a directory.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="target"/> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException"><paramref name="target"/> exists and <paramref name="overwrite"/> is <see langword="false"/>. -or- An I/O error has occurred.</exception>
    /// <exception cref="T:System.NotSupportedException"><paramref name="target"/> is in an invalid format.</exception>
    void IFileService.CopyTo(String target, Boolean overwrite) {
        if (target == null) { throw new ArgumentNullException(nameof(target)); }
        try
            {
            using (var SourceStream = ((IFileService)this).OpenRead()) {
                if (File.Exists(target)) {
                    if (!overwrite) { throw new IOException(); }
                    File.Delete(target);
                    }
                var folder = Path.GetDirectoryName(target);
                if (!String.IsNullOrEmpty(folder) && !Directory.Exists(folder)) { Directory.CreateDirectory(folder); }
                using (var TargetStream = File.OpenWrite(target)) {
                    SourceStream.CopyTo(TargetStream);
                    }
                }
            }
        catch (Exception e)
            {
            e.Add("Target", target);
            e.Add("Overwrite", overwrite);
            throw;
            }
        }

    /// <summary>Move an existing file to a new file. Overwriting a file of the same name when <paramref name="overwrite"/> is <see langword="true"/>.</summary>
    /// <param name="target">The name of the destination file. This cannot be a directory.</param>
    /// <param name="overwrite"><see langword="true"/> if the destination file can be overwritten; otherwise, <see langword="false"/>.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission. -or-  <paramref name="target"/> is read-only.</exception>
    /// <exception cref="T:System.ArgumentException"><paramref name="target"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars"/>.  -or-  <paramref name="target"/> specifies a directory.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="target"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The path specified in <paramref name="target"/> is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException"><paramref name="target"/> exists and <paramref name="overwrite"/> is <see langword="false"/>. -or- An I/O error has occurred.</exception>
    /// <exception cref="T:System.NotSupportedException"><paramref name="target"/> is in an invalid format.</exception>
    void IFileService.MoveTo(String target, Boolean overwrite) {
        ((IFileService)this).CopyTo(target, overwrite);
        }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override String ToString()
        {
        return FullName;
        }
    }
