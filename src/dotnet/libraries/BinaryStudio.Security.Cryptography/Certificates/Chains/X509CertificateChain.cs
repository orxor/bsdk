using System;
using System.Collections;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class X509CertificateChain:
        IReadOnlyList<X509CertificateChainElement>,IX509CertificateChainStatus
        {
        private readonly IList<X509CertificateChainElement> source = new List<X509CertificateChainElement>();
        public CertificateChainErrorStatus ErrorStatus { get; }
        public CertificateChainInfoStatus InfoStatus { get; }
        public Int32 ChainIndex { get; }

        #region M:IReadOnlyList<X509CertificateChainElement>.GetEnumerator:IEnumerator<X509CertificateChainElement>
        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<X509CertificateChainElement> GetEnumerator() {
            return source.GetEnumerator();
            }
        #endregion
        #region M:IEnumerable.GetEnumerator:IEnumerator
        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
            }
        #endregion
        #region P:IReadOnlyList<X509CertificateChainElement>.this[Int32]:X509CertificateChainElement
        public X509CertificateChainElement this[Int32 index] { get {
            return source[index];
            }}
        #endregion
        #region P:IReadOnlyList<X509CertificateChainElement>.Count:Int32
        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public Int32 Count { get { return source.Count; }}
        #endregion

        /// <summary>Initializes a new instance of the <see cref="X509CertificateChain"/> class from specified source.</summary>
        /// <param name="source">Source of chain data.</param>
        /// <param name="index">Chain index.</param>
        internal unsafe X509CertificateChain(CERT_SIMPLE_CHAIN* source, Int32 index) {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            ChainIndex = index;
            ErrorStatus = source->TrustStatus.ErrorStatus;
            InfoStatus  = source->TrustStatus.InfoStatus;
            if ((source->ElementCount > 0) && (source->ElementArray != null)) {
                for (var i = 0; i < source->ElementCount; i++) {
                    this.source.Add(new X509CertificateChainElement(source->ElementArray[i], i));
                    }
                }
            }
        }
    }
