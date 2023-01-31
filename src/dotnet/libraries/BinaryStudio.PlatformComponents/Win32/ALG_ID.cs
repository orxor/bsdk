﻿namespace BinaryStudio.PlatformComponents.Win32
    {
    public enum ALG_ID
        {
        CALG_GR3410                         = 0x00002e1e,
        CALG_GR3411                         = 0x0000801e,
        CALG_GR3411_2012_256                = 0x00008021,
        CALG_GR3411_2012_512                = 0x00008022,
        CALG_GR3411_HMAC                    = 0x00008027,
        CALG_GR3411_HMAC34                  = 0x00008028,
        CALG_UECMASTER_DIVERS               = 0x0000802f,
        CALG_GR3411_HMAC_FIXEDKEY           = 0x00008037,
        CALG_GR3411_2012_256_HMAC           = 0x00008034,
        CALG_GR3411_2012_512_HMAC           = 0x00008035,
        CALG_GR3411_2012_256_HMAC_FIXEDKEY  = 0x00008038,
        CALG_GR3411_2012_512_HMAC_FIXEDKEY  = 0x00008039,
        CALG_GR3411_PRFKEYMAT               = 0x0000804a,
        CALG_GR3411_2012_256_PRFKEYMAT      = 0x0000804b,
        CALG_GR3411_2012_512_PRFKEYMAT      = 0x0000804c,
        CALG_G28147_MAC                     = 0x0000801f,
        CALG_G28147_IMIT                    = 0x0000801f,
        CALG_GR3413_2015_M_IMIT             = 0x0000803c,
        CALG_GR3413_2015_K_IMIT             = 0x0000803d,
        CALG_G28147_CHV                     = 0x0000601f,
        CALG_GR3410EL                       = 0x00002e23,
        CALG_GR3410_12_256                  = 0x00002e49,
        CALG_GR3410_12_512                  = 0x00002e3d,
        CALG_G28147                         = 0x0000661e,
        CALG_SYMMETRIC_512                  = 0x00006622,
        CALG_GR3412_2015_M                  = 0x00006630,
        CALG_GR3412_2015_K                  = 0x00006631,
        CALG_DH_EL_SF                       = 0x0000aa24,
        CALG_DH_EL_EPHEM                    = 0x0000aa25,
        CALG_DH_GR3410_12_256_SF            = 0x0000aa46,
        CALG_DH_GR3410_12_256_EPHEM         = 0x0000aa47,
        CALG_DH_GR3410_12_512_SF            = 0x0000aa42,
        CALG_DH_GR3410_12_512_EPHEM         = 0x0000aa43,
        CALG_UECSYMMETRIC                   = 0x0000c62e,
        CALG_UECSYMMETRIC_EPHEM             = 0x0000c62f,
        CALG_GR3410_94_ESDH                 = 0x0000aa27,
        CALG_GR3410_01_ESDH                 = 0x0000aa28,
        CALG_GR3410_12_256_ESDH             = 0x0000aa48,
        CALG_GR3410_12_512_ESDH             = 0x0000aa3f,
        CALG_PRO_AGREEDKEY_DH               = 0x0000a621,
        CALG_PRO12_EXPORT                   = 0x00006621,
        CALG_PRO_EXPORT                     = 0x0000661f,
        CALG_SIMPLE_EXPORT                  = 0x00006620,
        CALG_KEXP_2015_M                    = 0x00006624,
        CALG_KEXP_2015_K                    = 0x00006625,
        CALG_TLS1PRF_2012_256               = 0x00008031,
        CALG_TLS1_MASTER_HASH               = 0x00008020,
        CALG_TLS1_MASTER_HASH_2012_256      = 0x00008036,
        CALG_TLS1_MAC_KEY                   = 0x00006c03,
        CALG_TLS1_ENC_KEY                   = 0x00006c07,
        CALG_PBKDF2_2012_512                = 0x0000803a,
        CALG_PBKDF2_2012_256                = 0x0000803b,
        CALG_PBKDF2_94_256                  = 0x00008040,
        CALG_SHAREDKEY_HASH                 = 0x00009032,
        CALG_FITTINGKEY_HASH                = 0x00009033,
        CALG_PRO_DIVERS                     = 0x00006626,
        CALG_RIC_DIVERS                     = 0x00006628,
        CALG_OSCAR_DIVERS                   = 0x00006628,
        CALG_PRO12_DIVERS                   = 0x0000662d,
        CALG_KDF_TREE_GOSTR3411_2012_256    = 0x00006623,
        CALG_EKE_CIPHER                     = 0x0000a629,
        CALG_EKEVERIFY_HASH                 = 0x0000802b,        
        CALG_MD2                            = 0x00008001,
        CALG_MD4                            = 0x00008002,
        CALG_MD5                            = 0x00008003,
        CALG_SHA                            = 0x00008004,
        CALG_SHA1                           = 0x00008004,
        CALG_MAC                            = 0x00008005,
        CALG_RSA_SIGN                       = 0x00002400,
        CALG_DSS_SIGN                       = 0x00002200,
        CALG_NO_SIGN                        = 0x00002000,
        CALG_RSA_KEYX                       = 0x0000a400,
        CALG_DES                            = 0x00006601,
        CALG_3DES_112                       = 0x00006609,
        CALG_3DES                           = 0x00006603,
        CALG_DESX                           = 0x00006604,
        CALG_RC2                            = 0x00006602,
        CALG_RC4                            = 0x00006801,
        CALG_SEAL                           = 0x00006802,
        CALG_DH_SF                          = 0x0000aa01,
        CALG_DH_EPHEM                       = 0x0000aa02,
        CALG_AGREEDKEY_ANY                  = 0x0000aa03,
        CALG_KEA_KEYX                       = 0x0000aa04,
        CALG_HUGHES_MD5                     = 0x0000a003,
        CALG_SKIPJACK                       = 0x0000660a,
        CALG_TEK                            = 0x0000660b,
        CALG_CYLINK_MEK                     = 0x0000660c,
        CALG_SSL3_SHAMD5                    = 0x00008008,
        CALG_SSL3_MASTER                    = 0x00004c01,
        CALG_SCHANNEL_MASTER_HASH           = 0x00004c02,
        CALG_SCHANNEL_MAC_KEY               = 0x00004c03,
        CALG_SCHANNEL_ENC_KEY               = 0x00004c07,
        CALG_PCT1_MASTER                    = 0x00004c04,
        CALG_SSL2_MASTER                    = 0x00004c05,
        CALG_TLS1_MASTER                    = 0x00004c06,
        CALG_RC5                            = 0x0000660d,
        CALG_HMAC                           = 0x00008009,
        CALG_TLS1PRF                        = 0x0000800a,
        CALG_HASH_REPLACE_OWF               = 0x0000800b,
        CALG_AES_128                        = 0x0000660e,
        CALG_AES_192                        = 0x0000660f,
        CALG_AES_256                        = 0x00006610,
        CALG_AES                            = 0x00006611,
        CALG_SHA_256                        = 0x0000800c,
        CALG_SHA_384                        = 0x0000800d,
        CALG_SHA_512                        = 0x0000800e,
        CALG_ECDH                           = 0x0000aa05,
        CALG_ECDH_EPHEM                     = 0x0000ae06,
        CALG_ECMQV                          = 0x0000a001,
        CALG_ECDSA                          = 0x00002203,
        CALG_NULLCIPHER                     = 0x00006000,
        ALG_CLASS_HASH                      = 0x00008000
        }
    }