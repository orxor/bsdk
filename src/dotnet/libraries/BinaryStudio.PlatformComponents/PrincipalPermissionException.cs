using System;
using System.Security;

namespace BinaryStudio.PlatformComponents
    {
    public class PrincipalPermissionException : SecurityException
        {
        #region ctor{String,Exception}
        public PrincipalPermissionException(String message, Exception e)
            :base(message, e)
            {
            }
        #endregion
        }
    }