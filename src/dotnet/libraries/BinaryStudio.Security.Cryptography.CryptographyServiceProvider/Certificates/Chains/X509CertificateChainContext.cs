using System;
using System.Collections;
using System.Collections.Generic;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class X509CertificateChainContext :
        IReadOnlyList<X509CertificateChain>,IX509CertificateChainStatus
        {
        internal unsafe CERT_CHAIN_CONTEXT* ChainContext = null;
        private readonly IList<X509CertificateChain> source = new List<X509CertificateChain>();

        #region M:IReadOnlyList<X509CertificateChain>.GetEnumerator:IEnumerator<X509CertificateChain>
        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<X509CertificateChain> GetEnumerator() {
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
        #region P:IReadOnlyList<X509CertificateChain>.this[Int32]:X509CertificateChain
        public X509CertificateChain this[Int32 index] { get {
            return source[index];
            }}
        #endregion
        #region P:IReadOnlyList<X509CertificateChain>.Count:Int32
        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public Int32 Count { get { return source.Count; }}
        #endregion

        public CertificateChainErrorStatus ErrorStatus { get; }
        public CertificateChainInfoStatus  InfoStatus  { get; }

        ///// <summary>Initializes a new instance of the <see cref="X509CertificateChainContext"/> class from specified source.</summary>
        ///// <param name="context">Source of chain context.</param>
        //internal unsafe X509CertificateChainContext(ref CERT_CHAIN_CONTEXT context)
        //    {
        //    ErrorStatus = context.TrustStatus.ErrorStatus;
        //    InfoStatus  = context.TrustStatus.InfoStatus;
        //    if ((context.ChainCount > 0) && (context.ChainArray != null)) {
        //        for (var i = 0; i < context.ChainCount; i++) {
        //            var chain = context.ChainArray[i];
        //            if (chain != null)
        //                {
        //                source.Add(new X509CertificateChain(chain, i));
        //                }
        //            }
        //        }
        //    }

        /// <summary>Initializes a new instance of the <see cref="X509CertificateChainContext"/> class from specified source.</summary>
        /// <param name="context">Source of chain context.</param>
        internal unsafe X509CertificateChainContext(CERT_CHAIN_CONTEXT* context) {
            ChainContext = context;
            ErrorStatus = context->TrustStatus.ErrorStatus;
            InfoStatus  = context->TrustStatus.InfoStatus;
            if ((context->ChainCount > 0) && (context->ChainArray != null)) {
                for (var i = 0; i < context->ChainCount; i++) {
                    var chain = context->ChainArray[i];
                    if (chain != null)
                        {
                        source.Add(new X509CertificateChain(chain, i));
                        }
                    }
                }
            }
        }
    }
