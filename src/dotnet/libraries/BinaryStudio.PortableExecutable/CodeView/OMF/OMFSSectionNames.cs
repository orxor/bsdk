using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using BinaryStudio.PortableExecutable.CodeView;

// ReSharper disable UnusedMember.Global
// ReSharper disable LocalVariableHidesMember

namespace BinaryStudio.PortableExecutable
    {
    using static COFFDataOperations;
    [OMFSSectionIndex(OMFSSectionIndex.Names)]
    internal class OMFSSectionNames : OMFSSection, IList<String>,ICodeViewNameTable
        {
        private readonly IList<String> Values = new List<String>();
        public OMFSSectionNames(OMFDirectory Directory)
            : base(Directory)
            {
            }
        public override OMFSSectionIndex SectionIndex { get { return OMFSSectionIndex.Names; }}
        public override unsafe OMFSSection Analyze(Byte* BaseAddress, Byte* Source, Int32 Size)
            {
            if (BaseAddress == null) { throw new ArgumentNullException(nameof(BaseAddress)); }
            if (Source == null) { throw new ArgumentNullException(nameof(Source)); }
            var Count = ReadInt32(ref Source);
            for (var i = 0; i < Count; i++) {
                ReadByte(ref Source);
                Values.Add(ReadZeroTerminatedString(ref Source, Encoding.ASCII));
                }
            return this;
            }

        #region M:IEnumerable<String>.GetEnumerator:IEnumerator<String>
        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        /// <filterpriority>1</filterpriority>
        IEnumerator<String> IEnumerable<String>.GetEnumerator()
            {
            return Values.GetEnumerator();
            }
        #endregion
        #region M:IEnumerable.GetEnumerator:IEnumerator
        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
            {
            return Values.GetEnumerator();
            }
        #endregion
        #region M:ICollection<String>.Add(String)
        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        void ICollection<String>.Add(String item)
            {
            throw new NotSupportedException();
            }
        #endregion
        #region M:ICollection<String>.Clear
        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        void ICollection<String>.Clear()
            {
            throw new NotSupportedException();
            }
        #endregion
        #region M:ICollection<String>.Contains(String):Boolean
        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.</summary>
        /// <returns>true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.</returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        Boolean ICollection<String>.Contains(String item)
            {
            return Values.Contains(item);
            }
        #endregion
        #region M:ICollection<String>.CopyTo(String[],Int32)
        /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        void ICollection<String>.CopyTo(String[] array, Int32 arrayIndex)
            {
            Values.CopyTo(array,arrayIndex);
            }
        #endregion
        #region M:ICollection<String>.Remove(String):Boolean
        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        Boolean ICollection<String>.Remove(String item)
            {
            throw new NotSupportedException();
            }
        #endregion
        #region P:ICollection<String>.Count:Int32
        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        public Int32 Count { get {
            return Values.Count;
            }}
        #endregion
        #region P:ICollection<String>.IsReadOnly:Boolean
        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean ICollection<String>.IsReadOnly { get {
            return true;
            }}
        #endregion
        #region M:IList<String>.IndexOf(String):Int32
        /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.</summary>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        Int32 IList<String>.IndexOf(String item)
            {
            return Values.IndexOf(item);
            }
        #endregion
        #region M:IList<String>.Insert(Int32,String)
        /// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        void IList<String>.Insert(Int32 index, String item)
            {
            throw new NotSupportedException();
            }
        #endregion
        #region M:IList<String>.RemoveAt(Int32)
        /// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.</summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        void IList<String>.RemoveAt(Int32 index)
            {
            throw new NotSupportedException();
            }
        #endregion
        #region P:IList<String>.this[Int32]:String
        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <returns>The element at the specified index.</returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        String IList<String>.this[Int32 index]
            {
            get { return Values[index]; }
            set { throw new NotSupportedException(); }
            }
        #endregion

        public String this[Int32 Index] { get {
            return Values[Index];
            }}

        public override void WriteTo(TextWriter Writer, String LinePrefix, FileDumpFlags Flags) {
            if (Writer == null) { throw new ArgumentNullException(nameof(Writer)); }
            var i = 1;
            foreach (var o in Values) {
                Writer.WriteLine("{0}{1}:{2}",LinePrefix, i.ToString("x8"), o);
                i++;
                }
            }
        }
    }