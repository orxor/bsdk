﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BinaryStudio.DiagnosticServices;
using BinaryStudio.DiagnosticServices.Logging;
using BinaryStudio.DirectoryServices;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;
using BinaryStudio.Security.Cryptography.CryptographicMessageSyntax;
using BinaryStudio.PlatformComponents.Win32;
using BinaryStudio.Security.Cryptography;
using BinaryStudio.Security.Cryptography.Certificates;
using Options;

namespace Operations
    {
    internal class VerifyMessageOperation : MessageOperation
        {
        private CertificateChainPolicy CertificateChainPolicy { get; }
        public String Policy { get; }
        private FileOperation UnderlyingObject { get; }

        #region ctor{IList<OperationOption>}
        public VerifyMessageOperation(IList<OperationOption> args)
            : base(args)
            {
            UnderlyingObject = new FileOperation(args);
            Policy = args.OfType<PolicyOption>().FirstOrDefault()?.Value;
            if (Policy != null) {
                switch (Policy) {
                    case "base"    : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_BASE;                break;
                    case "auth"    : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_AUTHENTICODE;        break;
                    case "authts"  : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_AUTHENTICODE_TS;     break;
                    case "ssl"     : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_SSL;                 break;
                    case "basic"   : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_BASIC_CONSTRAINTS;   break;
                    case "ntauth"  : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_NT_AUTH;             break;
                    case "msroot"  : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_MICROSOFT_ROOT;      break;
                    case "ev"      : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_EV;                  break;
                    case "ssl_f12" : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_SSL_F12;             break;
                    case "ssl_hpkp": CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_SSL_HPKP_HEADER;     break;
                    case "third"   : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_THIRD_PARTY_ROOT;    break;
                    case "ssl_key" : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_SSL_KEY_PIN;         break;
                    case "icao"    : CertificateChainPolicy = CertificateChainPolicy.CERT_CHAIN_POLICY_ICAO;                break;
                    default: throw new NotSupportedException();
                    }
                }
            }
        #endregion
        #region M:OnExecuteAction(Object,ExecuteActionEventArgs)
        private void OnExecuteAction(Object sender,ExecuteActionEventArgs e) {
            switch (Path.GetExtension(e.FileService.FileName).ToLower()) {
                case ".cer":
                case ".crl":
                case ".der":
                case ".ber":
                case ".p7b":
                    {
                    using (var stream = e.FileService.OpenRead()) {
                        using (var o = Asn1Object.Load(stream).FirstOrDefault()) {
                            using (var cer = new Asn1Certificate(o))               if (!cer.IsFailed) { e.OperationStatus = Execute(e.FileService,cer); break; }
                            using (var crl = new Asn1CertificateRevocationList(o)) if (!crl.IsFailed) { e.OperationStatus = Execute(e.FileService,crl); break; }
                            using (var cms = new CmsMessage(o))                    if (!cms.IsFailed) { e.OperationStatus = Execute(e.FileService,cms); break; }
                            e.OperationStatus = FileOperationStatus.Skip;
                            }
                        }
                    }
                    break;
                }
            }
        #endregion
        #region M:Execute
        public override void Execute() {
            UnderlyingObject.ExecuteAction += OnExecuteAction;
            UnderlyingObject.Execute();
            UnderlyingObject.ExecuteAction -= OnExecuteAction;
            }
        #endregion
        #region M:Execute(IFileService,Asn1Certificate):FileOperationStatus
        private FileOperationStatus Execute(IFileService FileService, Asn1Certificate Source) {
            if (CertificateChainPolicy != 0) {
                using (var certificate = new X509Certificate(Source)) {
                    certificate.Verify(CertificateChainPolicy);
                    return FileOperationStatus.Success;
                    }
                }
            /*
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder)) {
                JsonSerialize(Source,writer);
                }
            Logger.Log(LogLevel.Debug,builder.ToString());
            */
            return FileOperationStatus.Success;
            }
        #endregion
        #region M:Execute(IFileService,Asn1CertificateRevocationList):FileOperationStatus
        private FileOperationStatus Execute(IFileService FileService, Asn1CertificateRevocationList Source) {
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder)) {
                JsonSerialize(Source,writer);
                }
            Logger.Log(LogLevel.Debug,builder.ToString());
            return FileOperationStatus.Success;
            }
        #endregion
        #region M:Execute(IFileService,CmsMessage):FileOperationStatus
        private FileOperationStatus Execute(IFileService FileService, CmsMessage Source) {
            try
                {
                using (var InputStream = FileService.OpenRead()) {
                    var ci = (CmsSignedDataContentInfo)Source.GetService(typeof(CmsSignedDataContentInfo));
                    if (ci != null) {
                        var algid = ci.Signers.FirstOrDefault()?.SignatureAlgorithm?.ToString();
                        CryptographicContext.DefaultContext.VerifyAttachedMessageSignature(InputStream);
                        //using (var context = (new CryptographicFactory()).AcquireContext(new Oid(algid), CryptographicContextFlags.CRYPT_VERIFYCONTEXT)) {
                        //    context.VerifyAttachedMessageSignature(InputStream);
                        //    }
                        }
                    }
                return FileOperationStatus.Success;
                }
            catch (Exception e) {
                var ci = (CmsSignedDataContentInfo)Source.GetService(typeof(CmsSignedDataContentInfo));
                if (ci != null) {
                    e.Add("Certificates",ci.Certificates);
                    e.Add("DigestAlgorithms",ci.DigestAlgorithms);
                    }
                throw;
                }
            }
        #endregion
        }
    }
