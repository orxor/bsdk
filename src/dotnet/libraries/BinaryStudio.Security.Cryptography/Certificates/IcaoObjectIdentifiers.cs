using System;

namespace BinaryStudio.Security.Cryptography.Certificates
    {
    public class IcaoObjectIdentifiers
        {
        public const string Icao = "2.23.136";
        public const string IcaoMrtd = Icao + ".1";
        public const string IcaoMrtdSecurity = IcaoMrtd + ".1";
        public const string IcaoMrtdSecurityLdsSecurityObject = IcaoMrtdSecurity + ".1";
        public const string IcaoMrtdSecurityExtensions = IcaoMrtdSecurity + ".6";
        public const string IcaoMrtdSecurityExtensionsDocumentTypeList = IcaoMrtdSecurityExtensions + ".2";
        public const string IcaoMrtdSecurityExtensionsNameChange = IcaoMrtdSecurityExtensions + ".1";

        static IcaoObjectIdentifiers()
            {
            }
        }
    }
