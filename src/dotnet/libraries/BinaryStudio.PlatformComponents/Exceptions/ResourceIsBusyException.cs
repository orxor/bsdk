using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.Security.Cryptography
    {
    public class ResourceIsBusyException : HResultException
        {
        #region ctor{HRESULT}
        public ResourceIsBusyException(HRESULT SCode)
            :base(SCode)
            {
            }
        #endregion
        }
    }