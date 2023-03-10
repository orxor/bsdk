using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

namespace BinaryStudio.Security.Cryptography.AbstractSyntaxNotation
    {
    [DefaultProperty(nameof(SignatureAlgorithm))]
    public class Asn1SignatureAlgorithm : Asn1LinkObject
        {
        public Asn1ObjectIdentifier SignatureAlgorithm { get; }
        public virtual Oid HashAlgorithm { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Asn1Object UnderlyingObject { get { return base.UnderlyingObject; }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] protected internal override Boolean IsDecoded { get { return base.IsDecoded; }}
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] public override Boolean IsFailed  { get { return base.IsFailed;  }}

        protected internal Asn1SignatureAlgorithm(Asn1Object source)
            : base(source)
            {
            SignatureAlgorithm = (Asn1ObjectIdentifier)source[0];
            HashAlgorithm = GetHashAlgorithm(SignatureAlgorithm.ToString());
            }

        protected internal Asn1SignatureAlgorithm(Asn1ObjectIdentifier source)
            : base(source)
            {
            SignatureAlgorithm = source;
            HashAlgorithm = GetHashAlgorithm(SignatureAlgorithm.ToString());
            }

        private static readonly ReaderWriterLockSlim o = new ReaderWriterLockSlim();
        private static readonly IDictionary<String, Type> types = new Dictionary<String, Type>();
        public static Asn1SignatureAlgorithm From(Asn1SignatureAlgorithm source)
            {
            EnsureFactory();
            var key = source.SignatureAlgorithm.ToString();
            using (ReadLock(o)) {
                if (types.TryGetValue(key, out Type type)) {
                    var ctor = type.GetConstructor(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any,
                        new Type[] { typeof(Asn1Object) }, null);
                    if (ctor == null) { throw new MissingMemberException(); }
                    var r = (Asn1SignatureAlgorithm)ctor.Invoke(new Object[] { source.UnderlyingObject });
                    return r;
                    }
                }
            return source;
            }

        private static void EnsureFactory() {
            using (UpgradeableReadLock(o)) {
                if (types.Count == 0) {
                    using (WriteLock(o)) {
                        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(Asn1SignatureAlgorithm)))) {
                            var attribute = (Asn1SpecificObjectAttribute)type.GetCustomAttributes(typeof(Asn1SpecificObjectAttribute), false).FirstOrDefault();
                            if (attribute != null) {
                                types.Add(attribute.Key, type);
                                }
                            }
                        }
                    }
                }
            }

        public override String ToString()
            {
            return SignatureAlgorithm.ToString();
            }

        #region M:GetHashAlgorithm(String):Oid
        public static Oid GetHashAlgorithm(String oid) {
            String r = null;
            switch (oid) {
                #region ГОСТ Р 34.11-94
                case ObjectIdentifiers.szOID_CP_GOST_R3411_R3410EL:
                    {
                    r = ObjectIdentifiers.szOID_CP_GOST_R3411;
                    }
                    break;
                #endregion
                #region SHA1
                case ObjectIdentifiers.szOID_RSA_SHA1RSA:
                case ObjectIdentifiers.szOID_X957_SHA1DSA:
                case ObjectIdentifiers.szOID_DH_SINGLE_PASS_STDDH_SHA1_KDF:
                case ObjectIdentifiers.szOID_OIWSEC_dsaSHA1:
                case ObjectIdentifiers.szOID_OIWSEC_dsaCommSHA1:
                case ObjectIdentifiers.szOID_OIWSEC_sha1RSASign:
                case ObjectIdentifiers.szOID_ECDSA_SHA1:
                case ObjectIdentifiers.szOID_RSA_SSA_PSS:
                    {
                    r = ObjectIdentifiers.szOID_OIWSEC_sha1;
                    }
                    break;
                #endregion
                #region SHA256
                case ObjectIdentifiers.szOID_ECDSA_SHA256:
                case ObjectIdentifiers.szOID_DH_SINGLE_PASS_STDDH_SHA256_KDF:
                case ObjectIdentifiers.szOID_RSA_SHA256RSA:
                    {
                    r = ObjectIdentifiers.szOID_NIST_sha256;
                    }
                    break;
                #endregion
                #region ГОСТ Р 34.11-2012-256
                case ObjectIdentifiers.szOID_CP_GOST_R3410_12_256:
                case ObjectIdentifiers.szOID_tc26_gost_3410_12_256_paramSetA:
                case ObjectIdentifiers.szOID_CP_GOST_R3411_12_256_R3410:
                    {
                    r = ObjectIdentifiers.szOID_CP_GOST_R3411_12_256;
                    }
                    break;
                #endregion
                #region ГОСТ Р 34.11-2012-512
                case ObjectIdentifiers.szOID_tc26_gost_3410_12_512_paramSetA:
                case ObjectIdentifiers.szOID_tc26_gost_3410_12_512_paramSetB:
                case ObjectIdentifiers.szOID_tc26_gost_3410_12_512_paramSetC:
                case ObjectIdentifiers.szOID_CP_GOST_R3410_12_512:
                case ObjectIdentifiers.szOID_CP_GOST_R3411_12_512_R3410:
                    {
                    r = ObjectIdentifiers.szOID_CP_GOST_R3411_12_512;
                    }
                    break;
                #endregion
                #region SHA384
                case ObjectIdentifiers.szOID_ECDSA_SHA384:
                case ObjectIdentifiers.szOID_DH_SINGLE_PASS_STDDH_SHA384_KDF:
                case ObjectIdentifiers.szOID_RSA_SHA384RSA:
                    {
                    r = ObjectIdentifiers.szOID_NIST_sha384;
                    }
                    break;
                #endregion
                #region SHA512
                case ObjectIdentifiers.szOID_ECDSA_SHA512:
                case ObjectIdentifiers.szOID_RSA_SHA512RSA:
                    {
                    r = ObjectIdentifiers.szOID_NIST_sha512;
                    }
                    break;
                #endregion
                #region MD2
                case ObjectIdentifiers.szOID_RSA_MD2RSA :
                case ObjectIdentifiers.szOID_OIWDIR_md2:
                case ObjectIdentifiers.szOID_OIWDIR_md2RSA:
                case ObjectIdentifiers.szOID_OIWSEC_md2RSASign:
                    {
                    r = ObjectIdentifiers.szOID_RSA_MD2;
                    }
                    break;
                #endregion
                #region MD4
                case ObjectIdentifiers.szOID_OIWSEC_md4RSA:
                case ObjectIdentifiers.szOID_OIWSEC_md4RSA2:
                case ObjectIdentifiers.szOID_RSA_MD4RSA :
                    {
                    r = ObjectIdentifiers.szOID_RSA_MD4;
                    }
                    break;
                #endregion
                #region MD5
                case ObjectIdentifiers.szOID_OIWSEC_md5RSA:
                case ObjectIdentifiers.szOID_OIWSEC_md5RSASign:
                case ObjectIdentifiers.szOID_RSA_MD5RSA :
                    {
                    r = ObjectIdentifiers.szOID_RSA_MD5;
                    }
                    break;
                #endregion
                }
            if (r != null) {
                return ObjectIdentifiers.GetOID(r);
                }
            return null;
            }
        #endregion
        }
    }