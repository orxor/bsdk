using System;
using BinaryStudio.PlatformComponents.Win32;

namespace BinaryStudio.PlatformComponents.Exceptions
    {
    public static class Exceptions
        {
        #region M:GetSCODE({this}Exception):HRESULT
        public static HRESULT GetSCODE(this Exception e)
            {
            return HResultException.GetSCODE(e);
            }
        #endregion
        }
    }
