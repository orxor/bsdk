using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    internal class X509CertificateChainContext :
        IList<X509CertificateChain>
        {
        private unsafe CERT_CHAIN_CONTEXT* ChainContext = null;

        public X509CertificateChain this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(X509CertificateChain item)
            {
            throw new NotImplementedException();
            }

        public void Clear()
            {
            throw new NotImplementedException();
            }

        public bool Contains(X509CertificateChain item)
            {
            throw new NotImplementedException();
            }

        public void CopyTo(X509CertificateChain[] array, int arrayIndex)
            {
            throw new NotImplementedException();
            }

        public IEnumerator<X509CertificateChain> GetEnumerator()
            {
            throw new NotImplementedException();
            }

        public int IndexOf(X509CertificateChain item)
            {
            throw new NotImplementedException();
            }

        public void Insert(int index, X509CertificateChain item)
            {
            throw new NotImplementedException();
            }

        public bool Remove(X509CertificateChain item)
            {
            throw new NotImplementedException();
            }

        public void RemoveAt(int index)
            {
            throw new NotImplementedException();
            }

        IEnumerator IEnumerable.GetEnumerator()
            {
            throw new NotImplementedException();
            }
        }
    }
