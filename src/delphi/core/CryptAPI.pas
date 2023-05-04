unit CryptAPI;

interface

uses
  Windows,Classes,Json;

{$DEFINE CERT_CHAIN_PARA_HAS_EXTRA_FIELDS}

type
  {$IF DEFINED(VER140) OR DEFINED(VER150)}
  ULONG_PTR=LongWord;
  {$IFEND}

  HCRYPTPROV = ULONG_PTR;
  HCERTSTORE = Pointer;
  HCERTCHAINENGINE = Pointer;
  HCRYPTMSG=Pointer;
  LPFILETIME=^FILETIME;

  (*  CRYPTOAPI BLOB definitions *)
  DATA_BLOB=record
    cbData:DWORD;
    pbData:packed array of Byte;
  end;
  CRYPT_INTEGER_BLOB=DATA_BLOB;
  CRYPT_UINT_BLOB=DATA_BLOB;
  CRYPT_OBJID_BLOB=DATA_BLOB;
  CERT_NAME_BLOB=DATA_BLOB;
  CERT_RDN_VALUE_BLOB=DATA_BLOB;
  CERT_BLOB=DATA_BLOB;
  CRL_BLOB=DATA_BLOB;
  CRYPT_DATA_BLOB=DATA_BLOB;
  CRYPT_HASH_BLOB=DATA_BLOB;
  CRYPT_DIGEST_BLOB=DATA_BLOB;
  CRYPT_DER_BLOB=DATA_BLOB;
  CRYPT_ATTR_BLOB=DATA_BLOB;
  PCRYPT_INTEGER_BLOB=^CRYPT_INTEGER_BLOB;
  PCRYPT_UINT_BLOB=^CRYPT_UINT_BLOB;
  PCRYPT_OBJID_BLOB=^CRYPT_OBJID_BLOB;
  PCERT_NAME_BLOB=^CERT_NAME_BLOB;
  PCERT_RDN_VALUE_BLOB=^CERT_RDN_VALUE_BLOB;
  PCERT_BLOB=^CERT_BLOB;
  PCRL_BLOB=^CRL_BLOB;
  PDATA_BLOB=^DATA_BLOB;
  PCRYPT_DATA_BLOB=^CRYPT_DATA_BLOB;
  PCRYPT_HASH_BLOB=^CRYPT_HASH_BLOB;
  PCRYPT_DIGEST_BLOB=^CRYPT_DIGEST_BLOB;
  PCRYPT_DER_BLOB=^CRYPT_DER_BLOB;
  PCRYPT_ATTR_BLOB=^CRYPT_ATTR_BLOB;

  CERT_SYSTEM_STORE_INFO=record
    cbSize:LongWord;
  end;
  PCERT_SYSTEM_STORE_INFO=^CERT_SYSTEM_STORE_INFO;

  CRYPT_ALGORITHM_IDENTIFIER=record
    pszObjId:LPSTR;
    Parameters:CRYPT_OBJID_BLOB;
  end;
  PCRYPT_ALGORITHM_IDENTIFIER=^CRYPT_ALGORITHM_IDENTIFIER;

  CERT_PHYSICAL_STORE_INFO=record
    cbSize:DWORD;
    pszOpenStoreProvider:LPSTR;     // REG_SZ
    dwOpenEncodingType:DWORD;       // REG_DWORD
    dwOpenFlags:DWORD;              // REG_DWORD
    OpenParameters:CRYPT_DATA_BLOB; // REG_BINARY
    dwFlags:DWORD;                  // REG_DWORD
    dwPriority:DWORD;               // REG_DWORD
  end;
  PCERT_PHYSICAL_STORE_INFO=^CERT_PHYSICAL_STORE_INFO;

  CERT_EXTENSION=record
    pszObjId:LPSTR;
    fCritical:BOOL;
    Value:CRYPT_OBJID_BLOB;
  end;
  PCERT_EXTENSION=^CERT_EXTENSION;

  CRL_ENTRY=record
    SerialNumber:CRYPT_INTEGER_BLOB;
    RevocationDate:FILETIME;
    cExtension:Integer;
    rgExtension:packed array of CERT_EXTENSION;
  end;
  PCRL_ENTRY=^CRL_ENTRY;

  CRL_INFO=record
    dwVersion:DWORD;
    SignatureAlgorithm:CRYPT_ALGORITHM_IDENTIFIER;
    Issuer:CERT_NAME_BLOB;
    ThisUpdate:FILETIME;
    NextUpdate:FILETIME;
    cCRLEntry:DWORD;
    rgCRLEntry:packed array of CRL_ENTRY;
    cExtension:DWORD;
    rgExtension:packed array of CERT_EXTENSION;
  end;
  PCRL_INFO=^CRL_INFO;

  CRL_CONTEXT=record
    dwCertEncodingType:DWORD;
    pbCrlEncoded:PBYTE;
    cbCrlEncoded:DWORD;
    pCrlInfo:PCRL_INFO;
    hCertStore:HCERTSTORE;
  end;
  PCRL_CONTEXT=^CRL_CONTEXT;
  PCCRL_CONTEXT=PCRL_CONTEXT;

  CRYPT_BIT_BLOB=record
    cbData:DWORD;
    pbData:PBYTE;
    cUnusedBits:DWORD;
  end;
  PCRYPT_BIT_BLOB=^CRYPT_BIT_BLOB;

  CERT_PUBLIC_KEY_INFO=record
    Algorithm:CRYPT_ALGORITHM_IDENTIFIER;
    PublicKey:CRYPT_BIT_BLOB;
  end;
  PCERT_PUBLIC_KEY_INFO=^CERT_PUBLIC_KEY_INFO;

const
  CERT_STRONG_SIGN_SERIALIZED_INFO_CHOICE:LongWord=1;
  CERT_STRONG_SIGN_OID_INFO_CHOICE       :LongWord=2;

type
  CERT_STRONG_SIGN_SERIALIZED_INFO=record
    dwFlags:DWORD;
    pwszCNGSignHashAlgids:LPWSTR;
    pwszCNGPubKeyMinBitLengths:LPWSTR;
  end;
  PCERT_STRONG_SIGN_SERIALIZED_INFO=^CERT_STRONG_SIGN_SERIALIZED_INFO;

  CERT_STRONG_SIGN_PARA=record
    cbSize:DWORD;
    dwInfoChoice:DWORD;
    case Integer of
      0:(pvInfo:Pointer);
      1:(pSerializedInfo:PCERT_STRONG_SIGN_SERIALIZED_INFO);
      2:(pszOID:LPSTR);
  end;
  PCERT_STRONG_SIGN_PARA=^CERT_STRONG_SIGN_PARA;
  PCCERT_STRONG_SIGN_PARA=PCERT_STRONG_SIGN_PARA;

  CERT_INFO=record
    dwVersion:DWORD;
    SerialNumber:CRYPT_INTEGER_BLOB;
    SignatureAlgorithm:CRYPT_ALGORITHM_IDENTIFIER;
    Issuer:CERT_NAME_BLOB;
    NotBefore:FILETIME;
    NotAfter:FILETIME;
    Subject:CERT_NAME_BLOB;
    SubjectPublicKeyInfo:CERT_PUBLIC_KEY_INFO;
    IssuerUniqueId:CRYPT_BIT_BLOB;
    SubjectUniqueId:CRYPT_BIT_BLOB;
    cExtension:DWORD;
    rgExtension:PCERT_EXTENSION;
  end;
  PCERT_INFO=^CERT_INFO;

  (*
  Certificate context.
  A certificate context contains both the encoded and decoded representation
  of a certificate. A certificate context returned by a cert store function
  must be freed by calling the CertFreeCertificateContext function. The
  CertDuplicateCertificateContext function can be called to make a duplicate
  copy (which also must be freed by calling CertFreeCertificateContext).
  *)
  CERT_CONTEXT=record
    dwCertEncodingType:DWORD;
    pbCertEncoded:PBYTE;
    cbCertEncoded:DWORD;
    pCertInfo:PCERT_INFO;
    hCertStore:HCERTSTORE;
  end;
  PCERT_CONTEXT=^CERT_CONTEXT;
  PCCERT_CONTEXT=PCERT_CONTEXT;

  (* CTL Usage. Also used for EnhancedKeyUsage extension. *)
  CTL_USAGE=record
    cUsageIdentifier:DWORD;
    rgpszUsageIdentifier:packed array of LPSTR;
  end;
  PCTL_USAGE=^CTL_USAGE;
  PCCTL_USAGE=PCTL_USAGE;
  CERT_ENHKEY_USAGE=CTL_USAGE;
  PCERT_ENHKEY_USAGE=^CERT_ENHKEY_USAGE;
  PCCERT_ENHKEY_USAGE=PCERT_ENHKEY_USAGE;

// When building a chain, the there are various parameters used for finding
// issuing certificates and trust lists.  They are identified in the
// following structure

const
  USAGE_MATCH_TYPE_AND:LongWord=$00000000;
  USAGE_MATCH_TYPE_OR :LongWord=$00000001;

type
  CERT_USAGE_MATCH=record
    dwType:DWORD;
    Usage:CERT_ENHKEY_USAGE;
  end;
  PCERT_USAGE_MATCH=^CERT_USAGE_MATCH;

  CERT_CHAIN_PARA=record
    cbSize:DWORD;
    RequestedUsage:CERT_USAGE_MATCH;
    {$IFDEF CERT_CHAIN_PARA_HAS_EXTRA_FIELDS}
    RequestedIssuancePolicy:CERT_USAGE_MATCH;
    dwUrlRetrievalTimeout:DWORD;
    fCheckRevocationFreshnessTime:Boolean;
    dwRevocationFreshnessTime:DWORD;
    pftCacheResync:LPFILETIME;
    pStrongSignPara:PCCERT_STRONG_SIGN_PARA;
    dwStrongSignFlags:DWORD;
    {$ENDIF}
  end;
  PCERT_CHAIN_PARA=^CERT_CHAIN_PARA;

const
  CERT_CHAIN_STRONG_SIGN_DISABLE_END_CHECK_FLAG  :LongWord=$00000001;
  CERT_CHAIN_REVOCATION_CHECK_END_CERT           :LongWord=$10000000;
  CERT_CHAIN_REVOCATION_CHECK_CHAIN              :LongWord=$20000000;
  CERT_CHAIN_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT :LongWord=$40000000;
  CERT_CHAIN_REVOCATION_CHECK_CACHE_ONLY         :LongWord=$80000000;
  CERT_CHAIN_REVOCATION_ACCUMULATIVE_TIMEOUT     :LongWord=$08000000;
  CERT_CHAIN_REVOCATION_CHECK_OCSP_CERT          :LongWord=$04000000;
  CERT_CHAIN_DISABLE_PASS1_QUALITY_FILTERING     :LongWord=$00000040;
  CERT_CHAIN_RETURN_LOWER_QUALITY_CONTEXTS       :LongWord=$00000080;
  CERT_CHAIN_DISABLE_AUTH_ROOT_AUTO_UPDATE       :LongWord=$00000100;
  CERT_CHAIN_TIMESTAMP_TIME                      :LongWord=$00000200;
  CERT_CHAIN_ENABLE_PEER_TRUST                   :LongWord=$00000400;
  CERT_CHAIN_DISABLE_MY_PEER_TRUST               :LongWord=$00000800;
  CERT_CHAIN_DISABLE_MD2_MD4                     :LongWord=$00001000;
  CERT_CHAIN_DISABLE_AIA                         :LongWord=$00002000;
  CERT_CHAIN_HAS_MOTW                            :LongWord=$00004000;
  CERT_CHAIN_ONLY_ADDITIONAL_AND_AUTH_ROOT       :LongWord=$00008000;
  CERT_CHAIN_OPT_IN_WEAK_SIGNATURE               :LongWord=$00010000;

type
  CERT_TRUST_STATUS=record
    dwErrorStatus:DWORD;
    dwInfoStatus:DWORD;
  end;
  PCERT_TRUST_STATUS=^CERT_TRUST_STATUS;

const
  (* The following are error status bits *)
  CERT_TRUST_NO_ERROR                             :LongWord=$00000000;
  CERT_TRUST_IS_NOT_TIME_VALID                    :LongWord=$00000001;
  CERT_TRUST_IS_NOT_TIME_NESTED                   :LongWord=$00000002;
  CERT_TRUST_IS_REVOKED                           :LongWord=$00000004;
  CERT_TRUST_IS_NOT_SIGNATURE_VALID               :LongWord=$00000008;
  CERT_TRUST_IS_NOT_VALID_FOR_USAGE               :LongWord=$00000010;
  CERT_TRUST_IS_UNTRUSTED_ROOT                    :LongWord=$00000020;
  CERT_TRUST_REVOCATION_STATUS_UNKNOWN            :LongWord=$00000040;
  CERT_TRUST_IS_CYCLIC                            :LongWord=$00000080;
  CERT_TRUST_INVALID_EXTENSION                    :LongWord=$00000100;
  CERT_TRUST_INVALID_POLICY_CONSTRAINTS           :LongWord=$00000200;
  CERT_TRUST_INVALID_BASIC_CONSTRAINTS            :LongWord=$00000400;
  CERT_TRUST_INVALID_NAME_CONSTRAINTS             :LongWord=$00000800;
  CERT_TRUST_HAS_NOT_SUPPORTED_NAME_CONSTRAINT    :LongWord=$00001000;
  CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT      :LongWord=$00002000;
  CERT_TRUST_HAS_NOT_PERMITTED_NAME_CONSTRAINT    :LongWord=$00004000;
  CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT         :LongWord=$00008000;
  CERT_TRUST_IS_OFFLINE_REVOCATION                :LongWord=$01000000;
  CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY             :LongWord=$02000000;
  CERT_TRUST_IS_EXPLICIT_DISTRUST                 :LongWord=$04000000;
  CERT_TRUST_HAS_NOT_SUPPORTED_CRITICAL_EXT       :LongWord=$08000000;
  CERT_TRUST_HAS_WEAK_SIGNATURE                   :LongWord=$00100000;
  CERT_TRUST_HAS_WEAK_HYGIENE                     :LongWord=$00200000;
  CERT_TRUST_IS_PARTIAL_CHAIN                     :LongWord=$00010000;
  CERT_TRUST_CTL_IS_NOT_TIME_VALID                :LongWord=$00020000;
  CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID           :LongWord=$00040000;
  CERT_TRUST_CTL_IS_NOT_VALID_FOR_USAGE           :LongWord=$00080000;

  (* The following are info status bits *)
  CERT_TRUST_HAS_EXACT_MATCH_ISSUER               :LongWord=$00000001;
  CERT_TRUST_HAS_KEY_MATCH_ISSUER                 :LongWord=$00000002;
  CERT_TRUST_HAS_NAME_MATCH_ISSUER                :LongWord=$00000004;
  CERT_TRUST_IS_SELF_SIGNED                       :LongWord=$00000008;
  CERT_TRUST_AUTO_UPDATE_CA_REVOCATION            :LongWord=$00000010;
  CERT_TRUST_AUTO_UPDATE_END_REVOCATION           :LongWord=$00000020;
  CERT_TRUST_NO_OCSP_FAILOVER_TO_CRL              :LongWord=$00000040;
  CERT_TRUST_IS_KEY_ROLLOVER                      :LongWord=$00000080;
  CERT_TRUST_SSL_HANDSHAKE_OCSP                   :LongWord=$00040000;
  CERT_TRUST_SSL_TIME_VALID_OCSP                  :LongWord=$00080000;
  CERT_TRUST_SSL_RECONNECT_OCSP                   :LongWord=$00100000;
  CERT_TRUST_HAS_PREFERRED_ISSUER                 :LongWord=$00000100;
  CERT_TRUST_HAS_ISSUANCE_CHAIN_POLICY            :LongWord=$00000200;
  CERT_TRUST_HAS_VALID_NAME_CONSTRAINTS           :LongWord=$00000400;
  CERT_TRUST_IS_PEER_TRUSTED                      :LongWord=$00000800;
  CERT_TRUST_HAS_CRL_VALIDITY_EXTENDED            :LongWord=$00001000;
  CERT_TRUST_IS_FROM_EXCLUSIVE_TRUST_STORE        :LongWord=$00002000;
  CERT_TRUST_IS_CA_TRUSTED                        :LongWord=$00004000;
  CERT_TRUST_HAS_AUTO_UPDATE_WEAK_SIGNATURE       :LongWord=$00008000;
  CERT_TRUST_HAS_ALLOW_WEAK_SIGNATURE             :LongWord=$00020000;
  CERT_TRUST_IS_COMPLEX_CHAIN                     :LongWord=$00010000;
  CERT_TRUST_SSL_TIME_VALID                       :LongWord=$01000000;
  CERT_TRUST_NO_TIME_CHECK                        :LongWord=$02000000;

type
  CERT_REVOCATION_CRL_INFO=record
    cbSize:DWORD;
    pBaseCrlContext:PCCRL_CONTEXT;
    pDeltaCrlContext:PCCRL_CONTEXT;
    pCrlEntry:PCRL_ENTRY;
    fDeltaCrlEntry:BOOL;
  end;
  PCERT_REVOCATION_CRL_INFO=^CERT_REVOCATION_CRL_INFO;

  CERT_REVOCATION_INFO=record
    cbSize:DWORD;
    dwRevocationResult:DWORD;
    pszRevocationOid:LPCSTR;
    pvOidSpecificInfo:Pointer;
    fHasFreshnessTime:BOOL;
    dwFreshnessTime:DWORD;
    pCrlInfo:PCERT_REVOCATION_CRL_INFO;
  end;
  PCERT_REVOCATION_INFO=^CERT_REVOCATION_INFO;

  CERT_CHAIN_ELEMENT=record
    cbSize:DWORD;
    pCertContext:PCCERT_CONTEXT;
    TrustStatus:CERT_TRUST_STATUS;
    pRevocationInfo:PCERT_REVOCATION_INFO;
    pIssuanceUsage:PCERT_ENHKEY_USAGE;
    pApplicationUsage:PCERT_ENHKEY_USAGE;
    pwszExtendedErrorInfo:LPCWSTR;
  end;
  PCERT_CHAIN_ELEMENT=^CERT_CHAIN_ELEMENT;
  PCCERT_CHAIN_ELEMENT=PCERT_CHAIN_ELEMENT;

  CRYPT_ATTRIBUTE=record
    pszObjId:LPSTR;
    cValue:DWORD;
    rgValue:PCRYPT_ATTR_BLOB;
  end;
  PCRYPT_ATTRIBUTE=^CRYPT_ATTRIBUTE;

  CTL_ENTRY=record
    SubjectIdentifier:CRYPT_DATA_BLOB;
    cAttribute:DWORD;
    rgAttribute:PCRYPT_ATTRIBUTE;
  end;
  PCTL_ENTRY=^CTL_ENTRY;

  CTL_INFO=record
    dwVersion:DWORD;
    SubjectUsage:CTL_USAGE;
    ListIdentifier:CRYPT_DATA_BLOB;
    SequenceNumber:CRYPT_INTEGER_BLOB;
    ThisUpdate:FILETIME;
    NextUpdate:FILETIME;
    SubjectAlgorithm:CRYPT_ALGORITHM_IDENTIFIER;
    cCTLEntry:DWORD;
    rgCTLEntry:packed array of CTL_ENTRY;
    cExtension:DWORD;
    rgExtension:PCERT_EXTENSION;
  end;
  PCTL_INFO=^CTL_INFO;

  CTL_CONTEXT=record
    dwMsgAndCertEncodingType:DWORD;
    pbCtlEncoded:PBYTE;
    cbCtlEncoded:DWORD;
    pCtlInfo:PCTL_INFO;
    hCertStore:HCERTSTORE;
    hCryptMsg:HCRYPTMSG;
    pbCtlContent:PBYTE;
    cbCtlContent:DWORD;
  end;
  PCTL_CONTEXT=^CTL_CONTEXT;
  PCCTL_CONTEXT=PCTL_CONTEXT;

  CERT_TRUST_LIST_INFO=record
    cbSize:DWORD;
    pCtlEntry:PCTL_ENTRY;
    pCtlContext:PCCTL_CONTEXT;
  end;
  PCERT_TRUST_LIST_INFO=^CERT_TRUST_LIST_INFO;

  CERT_SIMPLE_CHAIN=record
    cbSize:DWORD;
    TrustStatus:CERT_TRUST_STATUS;
    cElement:DWORD;
    rgpElement:packed array of PCERT_CHAIN_ELEMENT;
    pTrustListInfo:PCERT_TRUST_LIST_INFO;
    fHasRevocationFreshnessTime:Boolean;
    dwRevocationFreshnessTime:DWORD;
  end;
  PCERT_SIMPLE_CHAIN=^CERT_SIMPLE_CHAIN;
  PCCERT_SIMPLE_CHAIN=PCERT_SIMPLE_CHAIN;

  PCERT_CHAIN_CONTEXT=^CERT_CHAIN_CONTEXT;
  PCCERT_CHAIN_CONTEXT=PCERT_CHAIN_CONTEXT;
  CERT_CHAIN_CONTEXT=record
    cbSize:DWORD;
    TrustStatus:CERT_TRUST_STATUS;
    cChain:DWORD;
    rgpChain:packed array of PCERT_SIMPLE_CHAIN;
    cLowerQualityChainContext:DWORD;
    rgpLowerQualityChainContext:packed array of PCCERT_CHAIN_CONTEXT;
    fHasRevocationFreshnessTime:Boolean;
    dwRevocationFreshnessTime:DWORD;
    dwCreateFlags:DWORD;
    ChainId:TGUID;
  end;

  PFN_CERT_ENUM_SYSTEM_STORE = function(pwszStoreLocation:PWideChar;dwFlags:LongWord;pStoreInfo:PCERT_SYSTEM_STORE_INFO;pvReserved,pvArg:Pointer):Boolean;stdcall;
  PFN_CERT_ENUM_PHYSICAL_STORE = function(pvSystemStore:Pointer;dwFlags:DWORD;pwszStoreName:PWideChar;pStoreInfo:PCERT_PHYSICAL_STORE_INFO;pvReserved,pvArg:Pointer):Boolean;stdcall;
  PFN_CERT_ENUM_SYSTEM_STORE_LOCATION= function(pwszStoreLocation:LPCWSTR;dwFlags:DWORD;pvReserved,pvArg:Pointer):Boolean;stdcall;

function CertEnumSystemStore(dwFlags:LongWord;pvSystemStoreLocationPara,pvArg:Pointer;pfnEnum:PFN_CERT_ENUM_SYSTEM_STORE):Boolean;stdcall;
function CertEnumPhysicalStore(pvSystemStore:Pointer;dwFlags:DWORD;pvArg:Pointer;pfnEnum:PFN_CERT_ENUM_PHYSICAL_STORE):Boolean;stdcall;
function CertEnumSystemStoreLocation(dwFlags:DWORD;pvArg:Pointer;pfnEnum:PFN_CERT_ENUM_SYSTEM_STORE_LOCATION):Boolean;stdcall;
function CertOpenStore(lpszStoreProvider:LPCSTR;dwEncodingType:DWORD;hCryptProv:HCRYPTPROV;dwFlags:DWORD;pvPara:Pointer):HCERTSTORE;stdcall;
function CertCloseStore(hCertStore:HCERTSTORE;dwFlags:DWORD):Boolean;stdcall;
function CertEnumCRLsInStore(hCertStore:HCERTSTORE;pPrevCrlContext:PCCRL_CONTEXT):PCCRL_CONTEXT;stdcall;
function CertEnumCertificatesInStore(hCertStore:HCERTSTORE;pPrevCertContext:PCCERT_CONTEXT):PCCERT_CONTEXT;stdcall;
function CertGetCRLContextProperty(pCrlContext:PCCRL_CONTEXT;dwPropId:DWORD;pvData:Pointer;pcbData:PDWORD):Boolean;stdcall;overload;
function CertGetCertificateContextProperty(pCertContext:PCCERT_CONTEXT;dwPropId:DWORD;pvData:Pointer;pcbData:PDWORD):Boolean;stdcall;
function CertNameToStrA(dwCertEncodingType:DWORD;pName:PCERT_NAME_BLOB;dwStrType:DWORD;psz:LPSTR;csz:DWORD):DWORD;stdcall;
function CertNameToStrW(dwCertEncodingType:DWORD;pName:PCERT_NAME_BLOB;dwStrType:DWORD;psz:LPWSTR;csz:DWORD):DWORD;stdcall;
function CertGetCertificateChain(hChainEngine:HCERTCHAINENGINE;pCertContext:PCCERT_CONTEXT;pTime:LPFILETIME;hAdditionalStore:HCERTSTORE;pChainPara:PCERT_CHAIN_PARA;dwFlags:DWORD;pvReserved:Pointer;out ppChainContext:PCCERT_CHAIN_CONTEXT):Boolean;stdcall;

const
  (* Certificate System Store Flag Values *)
  CERT_SYSTEM_STORE_MASK                  :LongWord=$FFFF0000;
  CERT_SYSTEM_STORE_RELOCATE_FLAG         :LongWord=$80000000;
  CERT_SYSTEM_STORE_UNPROTECTED_FLAG      :LongWord=$40000000;
  CERT_SYSTEM_STORE_DEFER_READ_FLAG       :LongWord=$20000000;
  CERT_SYSTEM_STORE_LOCATION_MASK         :LongWord=$00FF0000;
  CERT_SYSTEM_STORE_LOCATION_SHIFT        :LongWord=16;
  CERT_SYSTEM_STORE_CURRENT_USER_ID              :LongWord= 1;
  CERT_SYSTEM_STORE_LOCAL_MACHINE_ID             :LongWord= 2;
  CERT_SYSTEM_STORE_CURRENT_SERVICE_ID           :LongWord= 4;
  CERT_SYSTEM_STORE_SERVICES_ID                  :LongWord= 5;
  CERT_SYSTEM_STORE_USERS_ID                     :LongWord= 6;
  CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY_ID :LongWord= 7;
  CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY_ID:LongWord= 8;
  CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE_ID  :LongWord= 9;
  CERT_SYSTEM_STORE_LOCAL_MACHINE_WCOS_ID        :LongWord=10;
  CERT_SYSTEM_STORE_CURRENT_USER              :LongWord=$00010000;
  CERT_SYSTEM_STORE_LOCAL_MACHINE             :LongWord=$00020000;
  CERT_SYSTEM_STORE_CURRENT_SERVICE           :LongWord=$00040000;
  CERT_SYSTEM_STORE_SERVICES                  :LongWord=$00050000;
  CERT_SYSTEM_STORE_USERS                     :LongWord=$00060000;
  CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY :LongWord=$00070000;
  CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY:LongWord=$00080000;
  CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE  :LongWord=$00090000;
  CERT_SYSTEM_STORE_LOCAL_MACHINE_WCOS        :LongWord=$000a0000;
  CERT_PHYSICAL_STORE_PREDEFINED_ENUM_FLAG:LongWord=1;

  (* Certificate Store Provider Types *)
  CERT_STORE_PROV_MSG              :PAnsiChar=PAnsiChar( 1);
  CERT_STORE_PROV_MEMORY           :PAnsiChar=PAnsiChar( 2);
  CERT_STORE_PROV_FILE             :PAnsiChar=PAnsiChar( 3);
  CERT_STORE_PROV_REG              :PAnsiChar=PAnsiChar( 4);
  CERT_STORE_PROV_PKCS7            :PAnsiChar=PAnsiChar( 5);
  CERT_STORE_PROV_SERIALIZED       :PAnsiChar=PAnsiChar( 6);
  CERT_STORE_PROV_FILENAME_A       :PAnsiChar=PAnsiChar( 7);
  CERT_STORE_PROV_FILENAME_W       :PAnsiChar=PAnsiChar( 8);
  CERT_STORE_PROV_SYSTEM_A         :PAnsiChar=PAnsiChar( 9);
  CERT_STORE_PROV_SYSTEM_W         :PAnsiChar=PAnsiChar(10);
  CERT_STORE_PROV_COLLECTION       :PAnsiChar=PAnsiChar(11);
  CERT_STORE_PROV_SYSTEM_REGISTRY_A:PAnsiChar=PAnsiChar(12);
  CERT_STORE_PROV_SYSTEM_REGISTRY_W:PAnsiChar=PAnsiChar(13);
  CERT_STORE_PROV_PHYSICAL_W       :PAnsiChar=PAnsiChar(14);
  CERT_STORE_PROV_SMART_CARD_W     :PAnsiChar=PAnsiChar(15);
  CERT_STORE_PROV_LDAP_W           :PAnsiChar=PAnsiChar(16);
  CERT_STORE_PROV_PKCS12           :PAnsiChar=PAnsiChar(17);
  sz_CERT_STORE_PROV_MEMORY            :PAnsiChar=PAnsiChar('Memory');
  sz_CERT_STORE_PROV_FILENAME_W        :PAnsiChar=PAnsiChar('File');
  sz_CERT_STORE_PROV_SYSTEM_W          :PAnsiChar=PAnsiChar('System');
  sz_CERT_STORE_PROV_PKCS7             :PAnsiChar=PAnsiChar('PKCS7');
  sz_CERT_STORE_PROV_PKCS12            :PAnsiChar=PAnsiChar('PKCS12');
  sz_CERT_STORE_PROV_SERIALIZED        :PAnsiChar=PAnsiChar('Serialized');
  sz_CERT_STORE_PROV_COLLECTION        :PAnsiChar=PAnsiChar('Collection');
  sz_CERT_STORE_PROV_SYSTEM_REGISTRY_W :PAnsiChar=PAnsiChar('SystemRegistry');
  sz_CERT_STORE_PROV_PHYSICAL_W        :PAnsiChar=PAnsiChar('Physical');
  sz_CERT_STORE_PROV_SMART_CARD_W      :PAnsiChar=PAnsiChar('SmartCard');
  sz_CERT_STORE_PROV_LDAP_W            :PAnsiChar=PAnsiChar('Ldap');

const
  {$IF DEFINED(VER140) OR DEFINED(VER150)}
  CERT_STORE_PROV_FILENAME            :PAnsiChar=PAnsiChar( 8);
  CERT_STORE_PROV_SYSTEM              :PAnsiChar=PAnsiChar(10);
  CERT_STORE_PROV_SYSTEM_REGISTRY     :PAnsiChar=PAnsiChar(13);
  CERT_STORE_PROV_PHYSICAL            :PAnsiChar=PAnsiChar(14);
  CERT_STORE_PROV_SMART_CARD          :PAnsiChar=PAnsiChar(15);
  CERT_STORE_PROV_LDAP                :PAnsiChar=PAnsiChar(16);
  sz_CERT_STORE_PROV_FILENAME         :PAnsiChar=PAnsiChar('File');
  sz_CERT_STORE_PROV_SYSTEM           :PAnsiChar=PAnsiChar('System');
  sz_CERT_STORE_PROV_SYSTEM_REGISTRY  :PAnsiChar=PAnsiChar('SystemRegistry');
  sz_CERT_STORE_PROV_PHYSICAL         :PAnsiChar=PAnsiChar('Physical');
  sz_CERT_STORE_PROV_SMART_CARD       :PAnsiChar=PAnsiChar('SmartCard');
  sz_CERT_STORE_PROV_LDAP             :PAnsiChar=PAnsiChar('Ldap');
  {$ELSE}
  CERT_STORE_PROV_FILENAME            = CERT_STORE_PROV_FILENAME_W;
  CERT_STORE_PROV_SYSTEM              = CERT_STORE_PROV_SYSTEM_W;
  CERT_STORE_PROV_SYSTEM_REGISTRY     = CERT_STORE_PROV_SYSTEM_REGISTRY_W
  CERT_STORE_PROV_PHYSICAL            = CERT_STORE_PROV_PHYSICAL_W
  CERT_STORE_PROV_SMART_CARD          = CERT_STORE_PROV_SMART_CARD_W
  CERT_STORE_PROV_LDAP                = CERT_STORE_PROV_LDAP_W
  sz_CERT_STORE_PROV_FILENAME         = sz_CERT_STORE_PROV_FILENAME_W;
  sz_CERT_STORE_PROV_SYSTEM           = sz_CERT_STORE_PROV_SYSTEM_W;
  sz_CERT_STORE_PROV_SYSTEM_REGISTRY  = sz_CERT_STORE_PROV_SYSTEM_REGISTRY_W;
  sz_CERT_STORE_PROV_PHYSICAL         = sz_CERT_STORE_PROV_PHYSICAL_W;
  sz_CERT_STORE_PROV_SMART_CARD       = sz_CERT_STORE_PROV_SMART_CARD_W;
  sz_CERT_STORE_PROV_LDAP             = sz_CERT_STORE_PROV_LDAP_W;
  {$IFEND}

  (* Certificate and Message encoding types *)
  CERT_ENCODING_TYPE_MASK     :LongWord=$0000FFFF;
  CMSG_ENCODING_TYPE_MASK     :LongWord=$FFFF0000;
  CRYPT_ASN_ENCODING          :LongWord=$00000001;
  CRYPT_NDR_ENCODING          :LongWord=$00000002;
  X509_ASN_ENCODING           :LongWord=$00000001;
  X509_NDR_ENCODING           :LongWord=$00000002;
  PKCS_7_ASN_ENCODING         :LongWord=$00010000;
  PKCS_7_NDR_ENCODING         :LongWord=$00020000;

  (* Certificate Store open/property flags *)
  CERT_STORE_NO_CRYPT_RELEASE_FLAG                :LongWord=$00000001;
  CERT_STORE_SET_LOCALIZED_NAME_FLAG              :LongWord=$00000002;
  CERT_STORE_DEFER_CLOSE_UNTIL_LAST_FREE_FLAG     :LongWord=$00000004;
  CERT_STORE_DELETE_FLAG                          :LongWord=$00000010;
  CERT_STORE_UNSAFE_PHYSICAL_FLAG                 :LongWord=$00000020;
  CERT_STORE_SHARE_STORE_FLAG                     :LongWord=$00000040;
  CERT_STORE_SHARE_CONTEXT_FLAG                   :LongWord=$00000080;
  CERT_STORE_MANIFOLD_FLAG                        :LongWord=$00000100;
  CERT_STORE_ENUM_ARCHIVED_FLAG                   :LongWord=$00000200;
  CERT_STORE_UPDATE_KEYID_FLAG                    :LongWord=$00000400;
  CERT_STORE_BACKUP_RESTORE_FLAG                  :LongWord=$00000800;
  CERT_STORE_READONLY_FLAG                        :LongWord=$00008000;
  CERT_STORE_OPEN_EXISTING_FLAG                   :LongWord=$00004000;
  CERT_STORE_CREATE_NEW_FLAG                      :LongWord=$00002000;
  CERT_STORE_MAXIMUM_ALLOWED_FLAG                 :LongWord=$00001000;

  (* Certificate Store close flags *)
  CERT_CLOSE_STORE_FORCE_FLAG         :LongWord=$00000001;
  CERT_CLOSE_STORE_CHECK_FLAG         :LongWord=$00000002;

  (* Certificate, CRL and CTL property IDs *)
  CERT_KEY_PROV_HANDLE_PROP_ID                        :LongWord=  1;
  CERT_KEY_PROV_INFO_PROP_ID                          :LongWord=  2;
  CERT_SHA1_HASH_PROP_ID                              :LongWord=  3;
  CERT_MD5_HASH_PROP_ID                               :LongWord=  4;
  CERT_HASH_PROP_ID                                   :LongWord=  3;
  CERT_KEY_CONTEXT_PROP_ID                            :LongWord=  5;
  CERT_KEY_SPEC_PROP_ID                               :LongWord=  6;
  CERT_IE30_RESERVED_PROP_ID                          :LongWord=  7;
  CERT_PUBKEY_HASH_RESERVED_PROP_ID                   :LongWord=  8;
  CERT_ENHKEY_USAGE_PROP_ID                           :LongWord=  9;
  CERT_CTL_USAGE_PROP_ID                              :LongWord=  9;
  CERT_NEXT_UPDATE_LOCATION_PROP_ID                   :LongWord= 10;
  CERT_FRIENDLY_NAME_PROP_ID                          :LongWord= 11;
  CERT_PVK_FILE_PROP_ID                               :LongWord= 12;
  CERT_DESCRIPTION_PROP_ID                            :LongWord= 13;
  CERT_ACCESS_STATE_PROP_ID                           :LongWord= 14;
  CERT_SIGNATURE_HASH_PROP_ID                         :LongWord= 15;
  CERT_SMART_CARD_DATA_PROP_ID                        :LongWord= 16;
  CERT_EFS_PROP_ID                                    :LongWord= 17;
  CERT_FORTEZZA_DATA_PROP_ID                          :LongWord= 18;
  CERT_ARCHIVED_PROP_ID                               :LongWord= 19;
  CERT_KEY_IDENTIFIER_PROP_ID                         :LongWord= 20;
  CERT_AUTO_ENROLL_PROP_ID                            :LongWord= 21;
  CERT_PUBKEY_ALG_PARA_PROP_ID                        :LongWord= 22;
  CERT_CROSS_CERT_DIST_POINTS_PROP_ID                 :LongWord= 23;
  CERT_ISSUER_PUBLIC_KEY_MD5_HASH_PROP_ID             :LongWord= 24;
  CERT_SUBJECT_PUBLIC_KEY_MD5_HASH_PROP_ID            :LongWord= 25;
  CERT_ENROLLMENT_PROP_ID                             :LongWord= 26;
  CERT_DATE_STAMP_PROP_ID                             :LongWord= 27;
  CERT_ISSUER_SERIAL_NUMBER_MD5_HASH_PROP_ID          :LongWord= 28;
  CERT_SUBJECT_NAME_MD5_HASH_PROP_ID                  :LongWord= 29;
  CERT_EXTENDED_ERROR_INFO_PROP_ID                    :LongWord= 30;
  CERT_RENEWAL_PROP_ID                                :LongWord= 64;
  CERT_ARCHIVED_KEY_HASH_PROP_ID                      :LongWord= 65;
  CERT_AUTO_ENROLL_RETRY_PROP_ID                      :LongWord= 66;
  CERT_AIA_URL_RETRIEVED_PROP_ID                      :LongWord= 67;
  CERT_AUTHORITY_INFO_ACCESS_PROP_ID                  :LongWord= 68;
  CERT_BACKED_UP_PROP_ID                              :LongWord= 69;
  CERT_OCSP_RESPONSE_PROP_ID                          :LongWord= 70;
  CERT_REQUEST_ORIGINATOR_PROP_ID                     :LongWord= 71;
  CERT_SOURCE_LOCATION_PROP_ID                        :LongWord= 72;
  CERT_SOURCE_URL_PROP_ID                             :LongWord= 73;
  CERT_NEW_KEY_PROP_ID                                :LongWord= 74;
  CERT_OCSP_CACHE_PREFIX_PROP_ID                      :LongWord= 75;
  CERT_SMART_CARD_ROOT_INFO_PROP_ID                   :LongWord= 76;
  CERT_NO_AUTO_EXPIRE_CHECK_PROP_ID                   :LongWord= 77;
  CERT_NCRYPT_KEY_HANDLE_PROP_ID                      :LongWord= 78;
  CERT_HCRYPTPROV_OR_NCRYPT_KEY_HANDLE_PROP_ID        :LongWord= 79;
  CERT_SUBJECT_INFO_ACCESS_PROP_ID                    :LongWord= 80;
  CERT_CA_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID          :LongWord= 81;
  CERT_CA_DISABLE_CRL_PROP_ID                         :LongWord= 82;
  CERT_ROOT_PROGRAM_CERT_POLICIES_PROP_ID             :LongWord= 83;
  CERT_ROOT_PROGRAM_NAME_CONSTRAINTS_PROP_ID          :LongWord= 84;
  CERT_SUBJECT_OCSP_AUTHORITY_INFO_ACCESS_PROP_ID     :LongWord= 85;
  CERT_SUBJECT_DISABLE_CRL_PROP_ID                    :LongWord= 86;
  CERT_CEP_PROP_ID                                    :LongWord= 87;
  CERT_SIGN_HASH_CNG_ALG_PROP_ID                      :LongWord= 89;
  CERT_SCARD_PIN_ID_PROP_ID                           :LongWord= 90;
  CERT_SCARD_PIN_INFO_PROP_ID                         :LongWord= 91;
  CERT_SUBJECT_PUB_KEY_BIT_LENGTH_PROP_ID             :LongWord= 92;
  CERT_PUB_KEY_CNG_ALG_BIT_LENGTH_PROP_ID             :LongWord= 93;
  CERT_ISSUER_PUB_KEY_BIT_LENGTH_PROP_ID              :LongWord= 94;
  CERT_ISSUER_CHAIN_SIGN_HASH_CNG_ALG_PROP_ID         :LongWord= 95;
  CERT_ISSUER_CHAIN_PUB_KEY_CNG_ALG_BIT_LENGTH_PROP_ID:LongWord= 96;
  CERT_NO_EXPIRE_NOTIFICATION_PROP_ID                 :LongWord= 97;
  CERT_AUTH_ROOT_SHA256_HASH_PROP_ID                  :LongWord= 98;
  CERT_NCRYPT_KEY_HANDLE_TRANSFER_PROP_ID             :LongWord= 99;
  CERT_HCRYPTPROV_TRANSFER_PROP_ID                    :LongWord=100;
  CERT_SMART_CARD_READER_PROP_ID                      :LongWord=101;
  CERT_SEND_AS_TRUSTED_ISSUER_PROP_ID                 :LongWord=102;
  CERT_KEY_REPAIR_ATTEMPTED_PROP_ID                   :LongWord=103;
  CERT_DISALLOWED_FILETIME_PROP_ID                    :LongWord=104;
  CERT_ROOT_PROGRAM_CHAIN_POLICIES_PROP_ID            :LongWord=105;
  CERT_SMART_CARD_READER_NON_REMOVABLE_PROP_ID        :LongWord=106;
  CERT_SHA256_HASH_PROP_ID                            :LongWord=107;
  CERT_SCEP_SERVER_CERTS_PROP_ID                      :LongWord=108;
  CERT_SCEP_RA_SIGNATURE_CERT_PROP_ID                 :LongWord=109;
  CERT_SCEP_RA_ENCRYPTION_CERT_PROP_ID                :LongWord=110;
  CERT_SCEP_CA_CERT_PROP_ID	                          :LongWord=111;
  CERT_SCEP_SIGNER_CERT_PROP_ID                       :LongWord=112;
  CERT_SCEP_NONCE_PROP_ID                             :LongWord=113;
  CERT_SCEP_ENCRYPT_HASH_CNG_ALG_PROP_ID              :LongWord=114;
  CERT_SCEP_FLAGS_PROP_ID                             :LongWord=115;
  CERT_SCEP_GUID_PROP_ID                              :LongWord=116;
  CERT_SERIALIZABLE_KEY_CONTEXT_PROP_ID               :LongWord=117;
  CERT_ISOLATED_KEY_PROP_ID                           :LongWord=118;
  CERT_SERIAL_CHAIN_PROP_ID                           :LongWord=119;
  CERT_KEY_CLASSIFICATION_PROP_ID                     :LongWord=120;
  CERT_OCSP_MUST_STAPLE_PROP_ID                       :LongWord=121;
  CERT_DISALLOWED_ENHKEY_USAGE_PROP_ID                :LongWord=122;
  CERT_NONCOMPLIANT_ROOT_URL_PROP_ID                  :LongWord=123;
  CERT_PIN_SHA256_HASH_PROP_ID                        :LongWord=124;
  CERT_CLR_DELETE_KEY_PROP_ID                         :LongWord=125;
  CERT_NOT_BEFORE_FILETIME_PROP_ID                    :LongWord=126;
  CERT_NOT_BEFORE_ENHKEY_USAGE_PROP_ID                :LongWord=127;
  CERT_FIRST_RESERVED_PROP_ID                         :LongWord=128;

  (* Certificate name string types *)
  CERT_SIMPLE_NAME_STR        :LongWord=1;
  CERT_OID_NAME_STR           :LongWord=2;
  CERT_X500_NAME_STR          :LongWord=3;
  CERT_XML_NAME_STR           :LongWord=4;

  (*  Certificate name string type flags OR'ed with the above types *)
  CERT_NAME_STR_SEMICOLON_FLAG    :LongWord=$40000000;
  CERT_NAME_STR_NO_PLUS_FLAG      :LongWord=$20000000;
  CERT_NAME_STR_NO_QUOTING_FLAG   :LongWord=$10000000;
  CERT_NAME_STR_CRLF_FLAG         :LongWord=$08000000;
  CERT_NAME_STR_COMMA_FLAG        :LongWord=$04000000;
  CERT_NAME_STR_REVERSE_FLAG      :LongWord=$02000000;
  CERT_NAME_STR_FORWARD_FLAG      :LongWord=$01000000;

  CERT_NAME_STR_DISABLE_IE4_UTF8_FLAG     :LongWord=$00010000;
  CERT_NAME_STR_ENABLE_T61_UNICODE_FLAG   :LongWord=$00020000;
  CERT_NAME_STR_ENABLE_UTF8_UNICODE_FLAG  :LongWord=$00040000;
  CERT_NAME_STR_FORCE_UTF8_DIR_STR_FLAG   :LongWord=$00080000;
  CERT_NAME_STR_DISABLE_UTF8_DIR_STR_FLAG :LongWord=$00100000;
  CERT_NAME_STR_ENABLE_PUNYCODE_FLAG      :LongWord=$00200000;

  CRYPT_CRLREASON_UNSPECIFIED          =  0;
  CRYPT_CRLREASON_KEYCOMPROMISE        =  1;
  CRYPT_CRLREASON_CACOMPROMISE         =  2;
  CRYPT_CRLREASON_AFFILIATIONCHANGED   =  3;
  CRYPT_CRLREASON_SUPERSEDED           =  4;
  CRYPT_CRLREASON_CESSATIONOFOPERATION =  5;
  CRYPT_CRLREASON_CERTIFICATEHOLD      =  6;
  CRYPT_CRLREASON_REMOVEFROMCRL        =  8;
  CRYPT_CRLREASON_PRIVILEGEWITHDRAWN   =  9;
  CRYPT_CRLREASON_AACOMPROMISE         = 10;

  szOID_CRL_NUMBER:AnsiString='2.5.29.20';
  szOID_CRL_REASON_CODE:AnsiString='2.5.29.21';

  ASN1_CLASS_UNIVERSAL        = $00;
  ASN1_CLASS_APPLICATION      = $40;
  ASN1_CLASS_CONTEXT_SPECIFIC = $80;
  ANS1_CLASS_PRIVATE          = $c0;
  ASN1_TYPE_INTEGER =  2;
  ASN1_TYPE_ENUM    = 10;

type
  DByteArray = array of Byte;

function CertNameToStr(pName:PCERT_NAME_BLOB;dwStrType:DWORD):WideString;
function CertDataTimeToStr(Value:TFileTime):String;overload;
function CertDataTimeToStr(Value:TDateTime):String;overload;
function CertQueryCertificateCount(Store:HCERTSTORE):Integer;
function CertQueryCRLCount(Store:HCERTSTORE):Integer;
function FormatMessage(SCode:HRESULT):WideString;
function RegOpenKeyW(hKey:HKEY;lpSubKey:LPCWSTR):HKEY;overload;
function RegOpenKeyW(hKey:HKEY;lpSubKey:WideString):HKEY;overload;
function RegQuerySubkeys(hKey:HKEY):TStrings;
function RtlZeroMemory(Destination:Pointer;Length:DWORD):Pointer;
function CCryptGetThumbprint(pCrlContext:PCCRL_CONTEXT):WideString;overload;
function CCryptGetThumbprint(pCertContext:PCCERT_CONTEXT):WideString;overload;
function CCryptGetSerialNumber(pCertContext:PCCERT_CONTEXT):String;overload;
function CCryptGetSerialNumber(pCertInfo:PCERT_INFO):String;overload;
function CCryptGetSerialNumber(pCrlInfo:PCRL_INFO):String;overload;
function CCryptGetSerialNumber(pCrlEntry:PCRL_ENTRY):String;overload;
function CCryptGetContextProperty(pCrlContext:PCCRL_CONTEXT;dwPropId:DWORD;var Data:DByteArray):Boolean;overload;
function CCryptGetContextProperty(pCertContext:PCCERT_CONTEXT;dwPropId:DWORD;var Data:DByteArray):Boolean;overload;
function CCryptErrorStatusToStr(ErrorStatus:DWORD):String;
function CCryptInfoStatusToStr(InfoStatus:DWORD):String;

type
  TCrlEntry=class
  private
    Source:PCRL_ENTRY;
  public
    constructor Create(Source:PCRL_ENTRY);
  public
    function SerialNumber:String;overload;
    function RevocationDate:TDateTime;
    function ReasonCode:Integer;
  private
    class function SerialNumber(Source:PCRL_ENTRY):String;overload;
  public
    procedure WriteTo(Writer:JsonWriter);
  end;

implementation

uses
  SysUtils;

const
  crypt32  = 'crypt32.dll';
  advapi32 = 'advapi32.dll';


function CertEnumSystemStore; stdcall;external crypt32 name 'CertEnumSystemStore';
function CertEnumPhysicalStore; stdcall;external crypt32 name 'CertEnumPhysicalStore';
function CertEnumSystemStoreLocation; stdcall;external crypt32 name 'CertEnumSystemStoreLocation';
function CertOpenStore; stdcall;external crypt32 name 'CertOpenStore';
function CertCloseStore; stdcall;external crypt32 name 'CertCloseStore';
function CertEnumCRLsInStore; stdcall;external crypt32 name 'CertEnumCRLsInStore';
function CertEnumCertificatesInStore; stdcall;external crypt32 name 'CertEnumCertificatesInStore';
function CertGetCRLContextProperty(pCrlContext:PCCRL_CONTEXT;dwPropId:DWORD;pvData:Pointer;pcbData:PDWORD):Boolean;stdcall;overload;external crypt32 name 'CertGetCRLContextProperty';
function CertGetCertificateContextProperty;stdcall;overload;external crypt32 name 'CertGetCertificateContextProperty';
function CertNameToStrA; stdcall;external crypt32 name 'CertNameToStrA';
function CertNameToStrW; stdcall;external crypt32 name 'CertNameToStrW';
function CertGetCertificateChain; stdcall;external crypt32 name 'CertGetCertificateChain';
function RegOpenKeyW(hKey:HKEY;lpSubKey:PWideChar;phkResult:PHKEY):Longint; stdcall;external advapi32 name 'RegOpenKeyW';overload;
function RegQueryInfoKeyW(hKey:HKEY;lpClass:LPWSTR;
  lpcbClass:PDWORD;lpReserved:Pointer;
  lpcSubKeys,lpcbMaxSubKeyLen,lpcbMaxClassLen,lpcValues,
  lpcbMaxValueNameLen,lpcbMaxValueLen,lpcbSecurityDescriptor:PDWORD;
  lpftLastWriteTime:PFileTime):Longint;stdcall;external advapi32 name 'RegQueryInfoKeyW';
function RegEnumKeyExW(hKey:HKEY;dwIndex:DWORD;lpName:LPWSTR;
  lpcbName:PDWORD;lpReserved:Pointer;lpClass:LPWSTR;
  lpcbClass:PDWORD;lpftLastWriteTime:PFileTime):Longint; stdcall;external advapi32 name 'RegEnumKeyExW';

{$IF DEFINED(VER140) OR DEFINED(VER150)}
function MAKELANGID(p,s:WORD):WORD;
begin
  Result := WORD(s shl 10) or p;
end;
{$IFEND}

function FileTimeToDateTime(FileTime:TFileTime):TDateTime;
var
  ModifiedTime: TFileTime;
  SystemTime: TSystemTime;
begin
  Result := 0;
  if (FileTime.dwLowDateTime = 0) and (FileTime.dwHighDateTime = 0) then
    Exit;
  try
    FileTimeToLocalFileTime(FileTime, ModifiedTime);
    FileTimeToSystemTime(ModifiedTime, SystemTime);
    Result := SystemTimeToDateTime(SystemTime);
  except
    Result := 0;
 end;
end;

function DateTimeToFileTime(FileTime:TDateTime):TFileTime;
var
  LocalFileTime, Ft: TFileTime;
  SystemTime: TSystemTime;
begin
  Result.dwLowDateTime  := 0;
  Result.dwHighDateTime := 0;
  DateTimeToSystemTime(FileTime, SystemTime);
  SystemTimeToFileTime(SystemTime, LocalFileTime);
  LocalFileTimeToFileTime(LocalFileTime, Ft);
  Result := Ft;
end;

function StringJoin(Delimiter:String;Values:TStrings):String;
begin
  Result := '';
end;

function CCryptGetContextProperty(pCrlContext:PCCRL_CONTEXT;dwPropId:DWORD;var Data:DByteArray):Boolean;overload;
var
  Size:DWORD;
  OutputData:DByteArray;
  ErrorMessage:WideString;
begin
  ErrorMessage := '';
  Result := False;
  SetLength(OutputData,0);
  try
    Size := 0;
    if CertGetCRLContextProperty(pCrlContext,dwPropId,nil,@Size) then
    begin
      SetLength(OutputData,Size);
      if not CertGetCRLContextProperty(pCrlContext,dwPropId,@OutputData[0],@Size) then
      begin
        ErrorMessage := FormatMessage(GetLastError());
        SetLength(OutputData,0);
        Exit;
      end;
      Result := True;
    end
    else
      ErrorMessage := FormatMessage(GetLastError());
  finally
    Data := OutputData;
  end;
end;

function CCryptGetContextProperty(pCertContext:PCCERT_CONTEXT;dwPropId:DWORD;var Data:DByteArray):Boolean;overload;
var
  Size:DWORD;
  OutputData:DByteArray;
  ErrorMessage:WideString;
begin
  ErrorMessage := '';
  Result := False;
  SetLength(OutputData,0);
  try
    Size := 0;
    if CertGetCertificateContextProperty(pCertContext,dwPropId,nil,@Size) then
    begin
      SetLength(OutputData,Size);
      if not CertGetCertificateContextProperty(pCertContext,dwPropId,@OutputData[0],@Size) then
      begin
        ErrorMessage := FormatMessage(GetLastError());
        SetLength(OutputData,0);
        Exit;
      end;
      Result := True;
    end
    else
      ErrorMessage := FormatMessage(GetLastError());
  finally
    Data := OutputData;
  end;
end;

function FormatMessage(SCode:HRESULT):WideString;
var
  Message:LPWSTR;
begin
  if (FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER or FORMAT_MESSAGE_FROM_SYSTEM,
    nil, SCode, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),LPWSTR(@Message),0,nil)) > 0 then
    begin
      Result := WideString(Message);
      LocalFree(HLOCAL(Pointer(Message)));
    end
  else
    Result := Format('SCode:{%.8x}',[SCode]);
end;

function CertNameToStr(pName:PCERT_NAME_BLOB;dwStrType:DWORD):WideString;
var
  Size:DWORD;
  Buffer:DByteArray;
begin
  SetLength(Buffer,0);
  Result := '';
  Size := CertNameToStrW(X509_ASN_ENCODING,pName,dwStrType,nil,0);
  if Size > 0 then
  begin
    SetLength(Buffer,(Size + 1)*2);
    CertNameToStrW(X509_ASN_ENCODING,pName,dwStrType,PWideChar(@Buffer[0]),Size);
    Result := WideString(PWideChar(@Buffer[0]));
  end;
end;

function CertDataTimeToStr(Value:TDateTime):String;
begin
  Result := Format('%sT%s',[
    FormatDateTime('yyyy-MM-dd',Value),
    FormatDateTime('HH:mm:ss',Value)]);
end;

function CertDataTimeToStr(Value:TFileTime):String;
begin
  Result := CertDataTimeToStr(FileTimeToDateTime(Value));
end;

function RegOpenKeyW(hKey:HKEY;lpSubKey:LPCWSTR):HKEY;overload;
begin
  Result := 0;
  if RegOpenKeyW(hKey,lpSubKey,@Result) <> ERROR_SUCCESS then
  begin
    Result := 0;
  end;
end;

function RegOpenKeyW(hKey:HKEY;lpSubKey:WideString):HKEY;overload;
begin
  Result := RegOpenKeyW(hKey,PWideChar(lpSubKey));
end;

function RegQuerySubkeys(hKey:HKEY):TStrings;
const
  MAX_KEY_LENGTH = 255;
var
  achClass:array [0..MAX_PATH] of WideChar;
  achKey:array [0..MAX_KEY_LENGTH] of WideChar;
  I,cchClassName,cSubKeys,cbMaxSubKey,cchMaxClass,cValues,cchMaxValue,cbMaxValueData,cbSecurityDescriptor,cbName:DWORD;
  ftLastWriteTime:FILETIME;
begin
  Result := TStringList.Create;
  achClass[0] := #0;
  cSubKeys := 0;
  if RegQueryInfoKeyW(hKey,achClass,@cchClassName,nil,@cSubKeys,@cbMaxSubKey,
    @cchMaxClass,@cValues,@cchMaxValue,@cbMaxValueData,@cbSecurityDescriptor,
    @ftLastWriteTime) = ERROR_SUCCESS then
  begin
    if cSubKeys > 0 then
    begin
      for I := 0 to cSubKeys-1 do
      begin
        cbName := MAX_KEY_LENGTH;
        if RegEnumKeyExW(hKey,I,achKey,@cbName,nil,nil,nil,@ftLastWriteTime) = ERROR_SUCCESS then
        begin
          Result.Add(achKey);
        end;
      end;
    end;
  end;
end;

function CertQueryCertificateCount(Store:HCERTSTORE):Integer;
var
  Context:PCERT_CONTEXT;
begin
  Result := 0;
  if Store <> nil then
  begin
    Context := nil;
    repeat
      Context := CertEnumCertificatesInStore(Store,Context);
      if Context <> nil then
      begin
        Inc(Result);
      end;
    until Context = nil;
  end;
end;

function CertQueryCRLCount(Store:HCERTSTORE):Integer;
var
  Context:PCCRL_CONTEXT;
begin
  Result := 0;
  if Store <> nil then
  begin
    Context := nil;
    repeat
      Context := CertEnumCRLsInStore(Store,Context);
      if Context <> nil then
      begin
        Inc(Result);
      end;
    until Context = nil;
  end;
end;

function RtlZeroMemory(Destination:Pointer;Length:DWORD):Pointer;
begin
  Result := Destination;
  FillMemory(Destination,Length,0);
end;

function CCryptGetThumbprint(pCertContext:PCCERT_CONTEXT):WideString;overload;
  function Hex(Value:Byte):WideChar;
  begin
    Result := WideChar(Byte('a') + Value - 10);
    if (Value >= 0) and (Value <= 9) then
      Result := WideChar(Byte('0') + Value);
  end;
var
  PropertyBuffer:DByteArray;
  I,Count:Integer;
begin
  Result := '{none}';
  if pCertContext <> nil then
  begin
    if CCryptGetContextProperty(pCertContext,CERT_HASH_PROP_ID,PropertyBuffer) then
    begin
      Result := '';
      Count := Length(PropertyBuffer);
      for I := 0 to Count - 1 do
      begin
        Result := Result +
          Hex((PropertyBuffer[I] shr 4) and $0f) +
          Hex((PropertyBuffer[I]      ) and $0f);
      end;
    end;
  end;
end;

function CCryptGetThumbprint(pCrlContext:PCCRL_CONTEXT):WideString;overload;
  function Hex(Value:Byte):WideChar;
  begin
    Result := WideChar(Byte('a') + Value - 10);
    if (Value >= 0) and (Value <= 9) then
      Result := WideChar(Byte('0') + Value);
  end;
var
  PropertyBuffer:DByteArray;
  I,Count:Integer;
begin
  Result := '{none}';
  if pCrlContext <> nil then
  begin
    if CCryptGetContextProperty(pCrlContext,CERT_HASH_PROP_ID,PropertyBuffer) then
    begin
      Result := '';
      Count := Length(PropertyBuffer);
      for I := 0 to Count - 1 do
      begin
        Result := Result +
          Hex((PropertyBuffer[I] shr 4) and $0f) +
          Hex((PropertyBuffer[I]      ) and $0f);
      end;
    end;
  end;
end;

function CCryptErrorStatusToStr(ErrorStatus:DWORD):String;
  procedure Capture(FlagValue:DWORD;FlagName:String);
  begin
    if (ErrorStatus and FlagValue) > 0 then
    begin
      ErrorStatus := ErrorStatus and (not FlagValue);
      if Result <> '' then Result := Result + ';';
      Result := Result + FlagName;
    end;
  end;
begin
  Result := 'CERT_TRUST_NO_ERROR';
  if ErrorStatus > 0 then
  begin
    Result := '';
    Capture(CERT_TRUST_IS_NOT_TIME_VALID,'CERT_TRUST_IS_NOT_TIME_VALID');
    Capture(CERT_TRUST_IS_NOT_TIME_NESTED,'CERT_TRUST_IS_NOT_TIME_NESTED');
    Capture(CERT_TRUST_IS_REVOKED,'CERT_TRUST_IS_REVOKED');
    Capture(CERT_TRUST_IS_NOT_SIGNATURE_VALID,'CERT_TRUST_IS_NOT_SIGNATURE_VALID');
    Capture(CERT_TRUST_IS_NOT_VALID_FOR_USAGE,'CERT_TRUST_IS_NOT_VALID_FOR_USAGE');
    Capture(CERT_TRUST_IS_UNTRUSTED_ROOT,'CERT_TRUST_IS_UNTRUSTED_ROOT');
    Capture(CERT_TRUST_REVOCATION_STATUS_UNKNOWN,'CERT_TRUST_REVOCATION_STATUS_UNKNOWN');
    Capture(CERT_TRUST_IS_CYCLIC,'CERT_TRUST_IS_CYCLIC');
    Capture(CERT_TRUST_INVALID_EXTENSION,'CERT_TRUST_INVALID_EXTENSION');
    Capture(CERT_TRUST_INVALID_POLICY_CONSTRAINTS,'CERT_TRUST_INVALID_POLICY_CONSTRAINTS');
    Capture(CERT_TRUST_INVALID_BASIC_CONSTRAINTS,'CERT_TRUST_INVALID_BASIC_CONSTRAINTS');
    Capture(CERT_TRUST_INVALID_NAME_CONSTRAINTS,'CERT_TRUST_INVALID_NAME_CONSTRAINTS');
    Capture(CERT_TRUST_HAS_NOT_SUPPORTED_NAME_CONSTRAINT,'CERT_TRUST_HAS_NOT_SUPPORTED_NAME_CONSTRAINT');
    Capture(CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT,'CERT_TRUST_HAS_NOT_DEFINED_NAME_CONSTRAINT');
    Capture(CERT_TRUST_HAS_NOT_PERMITTED_NAME_CONSTRAINT,'CERT_TRUST_HAS_NOT_PERMITTED_NAME_CONSTRAINT');
    Capture(CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT,'CERT_TRUST_HAS_EXCLUDED_NAME_CONSTRAINT');
    Capture(CERT_TRUST_IS_OFFLINE_REVOCATION,'CERT_TRUST_IS_OFFLINE_REVOCATION');
    Capture(CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY,'CERT_TRUST_NO_ISSUANCE_CHAIN_POLICY');
    Capture(CERT_TRUST_IS_EXPLICIT_DISTRUST,'CERT_TRUST_IS_EXPLICIT_DISTRUST');
    Capture(CERT_TRUST_HAS_NOT_SUPPORTED_CRITICAL_EXT,'CERT_TRUST_HAS_NOT_SUPPORTED_CRITICAL_EXT');
    Capture(CERT_TRUST_HAS_WEAK_SIGNATURE,'CERT_TRUST_HAS_WEAK_SIGNATURE');
    Capture(CERT_TRUST_HAS_WEAK_HYGIENE,'CERT_TRUST_HAS_WEAK_HYGIENE');
    Capture(CERT_TRUST_IS_PARTIAL_CHAIN,'CERT_TRUST_IS_PARTIAL_CHAIN');
    Capture(CERT_TRUST_CTL_IS_NOT_TIME_VALID,'CERT_TRUST_CTL_IS_NOT_TIME_VALID');
    Capture(CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID,'CERT_TRUST_CTL_IS_NOT_SIGNATURE_VALID');
    Capture(CERT_TRUST_CTL_IS_NOT_VALID_FOR_USAGE,'CERT_TRUST_CTL_IS_NOT_VALID_FOR_USAGE');
    if ErrorStatus > 0 then
    begin
      if Result <> '' then Result := Result + ';';
      Result := Result + Format('%.8x', [ErrorStatus]);
    end;
  end;
end;

function CCryptInfoStatusToStr(InfoStatus:DWORD):String;
  procedure Capture(FlagValue:DWORD;FlagName:String);
  begin
    if (InfoStatus and FlagValue) > 0 then
    begin
      InfoStatus := InfoStatus and (not FlagValue);
      if Result <> '' then Result := Result + ';';
      Result := Result + FlagName;
    end;
  end;
begin
  Result := 'CERT_TRUST_NO_ERROR';
  if InfoStatus > 0 then
  begin
    Result := '';
    Capture(CERT_TRUST_HAS_EXACT_MATCH_ISSUER,'CERT_TRUST_HAS_EXACT_MATCH_ISSUER');
    Capture(CERT_TRUST_HAS_KEY_MATCH_ISSUER,'CERT_TRUST_HAS_KEY_MATCH_ISSUER');
    Capture(CERT_TRUST_HAS_NAME_MATCH_ISSUER,'CERT_TRUST_HAS_NAME_MATCH_ISSUER');
    Capture(CERT_TRUST_IS_SELF_SIGNED,'CERT_TRUST_IS_SELF_SIGNED');
    Capture(CERT_TRUST_AUTO_UPDATE_CA_REVOCATION,'CERT_TRUST_AUTO_UPDATE_CA_REVOCATION');
    Capture(CERT_TRUST_AUTO_UPDATE_END_REVOCATION,'CERT_TRUST_AUTO_UPDATE_END_REVOCATION');
    Capture(CERT_TRUST_NO_OCSP_FAILOVER_TO_CRL,'CERT_TRUST_NO_OCSP_FAILOVER_TO_CRL');
    Capture(CERT_TRUST_IS_KEY_ROLLOVER,'CERT_TRUST_IS_KEY_ROLLOVER');
    Capture(CERT_TRUST_SSL_HANDSHAKE_OCSP,'CERT_TRUST_SSL_HANDSHAKE_OCSP');
    Capture(CERT_TRUST_SSL_TIME_VALID_OCSP,'CERT_TRUST_SSL_TIME_VALID_OCSP');
    Capture(CERT_TRUST_SSL_RECONNECT_OCSP,'CERT_TRUST_SSL_RECONNECT_OCSP');
    Capture(CERT_TRUST_HAS_PREFERRED_ISSUER,'CERT_TRUST_HAS_PREFERRED_ISSUER');
    Capture(CERT_TRUST_HAS_ISSUANCE_CHAIN_POLICY,'CERT_TRUST_HAS_ISSUANCE_CHAIN_POLICY');
    Capture(CERT_TRUST_HAS_VALID_NAME_CONSTRAINTS,'CERT_TRUST_HAS_VALID_NAME_CONSTRAINTS');
    Capture(CERT_TRUST_IS_PEER_TRUSTED,'CERT_TRUST_IS_PEER_TRUSTED');
    Capture(CERT_TRUST_HAS_CRL_VALIDITY_EXTENDED,'CERT_TRUST_HAS_CRL_VALIDITY_EXTENDED');
    Capture(CERT_TRUST_IS_FROM_EXCLUSIVE_TRUST_STORE,'CERT_TRUST_IS_FROM_EXCLUSIVE_TRUST_STORE');
    Capture(CERT_TRUST_IS_CA_TRUSTED,'CERT_TRUST_IS_CA_TRUSTED');
    Capture(CERT_TRUST_HAS_AUTO_UPDATE_WEAK_SIGNATURE,'CERT_TRUST_HAS_AUTO_UPDATE_WEAK_SIGNATURE');
    Capture(CERT_TRUST_HAS_ALLOW_WEAK_SIGNATURE,'CERT_TRUST_HAS_ALLOW_WEAK_SIGNATURE');
    Capture(CERT_TRUST_IS_COMPLEX_CHAIN,'CERT_TRUST_IS_COMPLEX_CHAIN');
    Capture(CERT_TRUST_SSL_TIME_VALID,'CERT_TRUST_SSL_TIME_VALID');
    Capture(CERT_TRUST_NO_TIME_CHECK,'CERT_TRUST_NO_TIME_CHECK');
    if InfoStatus > 0 then
    begin
      if Result <> '' then Result := Result + ';';
      Result := Result + Format('%.8x', [InfoStatus]);
    end;
  end;
end;

function CCryptGetSerialNumber(pCertContext:PCCERT_CONTEXT):String;overload;
begin
  Result := '{none}';
  if pCertContext <> nil then
  begin
    Result := CCryptGetSerialNumber(pCertContext.pCertInfo);
  end;
end;

function CCryptGetSerialNumber(pCertInfo:PCERT_INFO):String;overload;
  function Hex(Value:Byte):Char;
  begin
    Result := Char(Byte('a') + Value - 10);
    if (Value >= 0) and (Value <= 9) then
      Result := Char(Byte('0') + Value);
  end;
var
  I:Integer;
begin
  Result := '{none}';
  if pCertInfo <> nil then
  begin
    Result := '';
    for I := 0 to pCertInfo.SerialNumber.cbData - 1 do
    begin
      Result :=
        Hex((pCertInfo.SerialNumber.pbData[I] shr 4) and $0f) +
        Hex((pCertInfo.SerialNumber.pbData[I]      ) and $0f) + Result;
    end;
  end;
end;

function CCryptGetSerialNumber(pCrlEntry:PCRL_ENTRY):String;overload;
  function Hex(Value:Byte):Char;
  begin
    Result := Char(Byte('a') + Value - 10);
    if (Value >= 0) and (Value <= 9) then
      Result := Char(Byte('0') + Value);
  end;
var
  I:Integer;
begin
  Result := '{none}';
  if pCrlEntry <> nil then
  begin
    Result := '';
    for I := 0 to pCrlEntry.SerialNumber.cbData - 1 do
    begin
      Result :=
        Hex((pCrlEntry.SerialNumber.pbData[I] shr 4) and $0f) +
        Hex((pCrlEntry.SerialNumber.pbData[I]      ) and $0f) + Result;
    end;
  end;
end;

function CCryptGetSerialNumber(pCrlInfo:PCRL_INFO):String;overload;
  function Hex(Value:Byte):Char;
  begin
    Result := Char(Byte('a') + Value - 10);
    if (Value >= 0) and (Value <= 9) then
      Result := Char(Byte('0') + Value);
  end;
var
  Extension:PCERT_EXTENSION;
  I,J,Count:DWORD;
begin
  Result := '{none}';
  if pCrlInfo <> nil then
  begin
    for I := 0 to pCrlInfo.cExtension - 1 do
    begin
      Extension := @pCrlInfo.rgExtension[I];
      if Extension.pszObjId = szOID_CRL_NUMBER then
      begin
        if Extension.Value.cbData > 0 then
        begin
          if Extension.Value.pbData[0] = (ASN1_CLASS_UNIVERSAL or ASN1_TYPE_INTEGER) then
          begin
            Count := Extension.Value.pbData[1];
            Result := '';
            for J := 0 to Count - 1 do
            begin
              Result := Result +
                Hex((Extension.Value.pbData[J + 2] shr 4) and $0f) +
                Hex((Extension.Value.pbData[J + 2]      ) and $0f)
            end;
          end;
          Break;
        end;
      end;
    end;
  end;
end;

{ TCrlEntry }

constructor TCrlEntry.Create(Source:PCRL_ENTRY);
begin
  Self.Source := Source;
end;

function TCrlEntry.ReasonCode:Integer;
var
  Extension:PCERT_EXTENSION;
  I:Integer;
begin
  Result := 0;
  if Source <> nil then
  begin
    for I := 0 to Source.cExtension - 1 do
    begin
      Extension := @Source.rgExtension[I];
      if Extension.pszObjId = szOID_CRL_REASON_CODE then
      begin
        if Extension.Value.cbData > 0 then
        begin
          if Extension.Value.pbData[0] = (ASN1_CLASS_UNIVERSAL or ASN1_TYPE_ENUM) then
          begin
            Result := Extension.Value.pbData[2];
            Break;
          end;
        end;
      end;
    end;
  end;
end;

function TCrlEntry.RevocationDate:TDateTime;
  function FileTimeToDateTime(FileTime:TFileTime):TDateTime;
  var
    ModifiedTime: TFileTime;
    SystemTime: TSystemTime;
  begin
    Result := 0;
    if (FileTime.dwLowDateTime = 0) and (FileTime.dwHighDateTime = 0) then
      Exit;
    try
      FileTimeToLocalFileTime(FileTime, ModifiedTime);
      FileTimeToSystemTime(ModifiedTime, SystemTime);
      Result := SystemTimeToDateTime(SystemTime);
    except
      Result := 0;
    end;
   end;
begin
  Result := FileTimeToDateTime(Source.RevocationDate);
end;

class function TCrlEntry.SerialNumber(Source:PCRL_ENTRY):String;
  function Hex(Value:Byte):Char;
  begin
    Result := Char(Byte('a') + Value - 10);
    if (Value >= 0) and (Value <= 9) then
      Result := Char(Byte('0') + Value);
  end;
var
  I:Integer;
begin
  Result := '{none}';
  if Source <> nil then
  begin
    Result := '';
    for I := 0 to Source.SerialNumber.cbData - 1 do
    begin
      Result :=
        Hex((Source.SerialNumber.pbData[I] shr 4) and $0f) +
        Hex((Source.SerialNumber.pbData[I]      ) and $0f) + Result;
    end;
  end;
end;

function TCrlEntry.SerialNumber:String;
begin
  Result := SerialNumber(Source);
end;

procedure TCrlEntry.WriteTo(Writer: JsonWriter);
var
  ReasonCode:Integer;
begin
  Writer.WriteStartObject;
    Writer.WriteProperty('SerialNumber',SerialNumber);
    Writer.WriteProperty('RevocationDate',RevocationDate);
    ReasonCode := Self.ReasonCode;
    case ReasonCode of
      CRYPT_CRLREASON_UNSPECIFIED          : Writer.WriteProperty('ReasonCode','Unspecified');
      CRYPT_CRLREASON_KEYCOMPROMISE        : Writer.WriteProperty('ReasonCode','KeyCompromise');
      CRYPT_CRLREASON_CACOMPROMISE         : Writer.WriteProperty('ReasonCode','CACompromise');
      CRYPT_CRLREASON_AFFILIATIONCHANGED   : Writer.WriteProperty('ReasonCode','AffiliationChanged');
      CRYPT_CRLREASON_SUPERSEDED           : Writer.WriteProperty('ReasonCode','Superseded');
      CRYPT_CRLREASON_CESSATIONOFOPERATION : Writer.WriteProperty('ReasonCode','CessationOfOperation');
      CRYPT_CRLREASON_CERTIFICATEHOLD      : Writer.WriteProperty('ReasonCode','CertificateHold');
      CRYPT_CRLREASON_REMOVEFROMCRL        : Writer.WriteProperty('ReasonCode','RemoveFromCrl');
      CRYPT_CRLREASON_PRIVILEGEWITHDRAWN   : Writer.WriteProperty('ReasonCode','PrivilegeWithDrawn');
      CRYPT_CRLREASON_AACOMPROMISE         : Writer.WriteProperty('ReasonCode','AACompromise');
    else
      Writer.WriteProperty('ReasonCode',ReasonCode);
    end;
  Writer.WriteEnd;
end;

end.
