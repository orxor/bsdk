using System;

namespace BinaryStudio.Security.Cryptography
    {
    public class IcaoObjectIdentifiers
        {
        public const String Icao = "2.23.136";
        public const String IcaoMrtd = Icao + ".1";
        public const String IcaoMrtdSecurity = IcaoMrtd + ".1";
        public const String IcaoMrtdSecurityLdsSecurityObject = IcaoMrtdSecurity + ".1";
        public const String IcaoMrtdSecurityExtensions = IcaoMrtdSecurity + ".6";
        public const String IcaoMrtdSecurityExtensionsDocumentTypeList = IcaoMrtdSecurityExtensions + ".2";
        public const String IcaoMrtdSecurityExtensionsNameChange = IcaoMrtdSecurityExtensions + ".1";

        static IcaoObjectIdentifiers() {
            }
        }
    }
