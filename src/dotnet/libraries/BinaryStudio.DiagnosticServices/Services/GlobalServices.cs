using System;
using System.Collections.Generic;

namespace BinaryStudio.Services
    {
    public delegate void ServiceResolveEventHandler(Object sender, ServiceResolveEventArgs args);
    public static class GlobalServices
        {
        public static event ServiceResolveEventHandler ServiceResolve;
        #region M:GetService(Object):Object
        public static Object GetService(Object service) {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            lock (Services) {
                if (Services.TryGetValue(service, out var r)) {
                    if (r.IsAlive) {
                        return r.Target;
                        }
                    }
                var e = new ServiceResolveEventArgs(service);
                var s = ServiceResolve;
                if (s != null) {
                    foreach (ServiceResolveEventHandler i in s.GetInvocationList()) {
                        i.Invoke(service,e);
                        if (e.Service != null) {
                            Services[e.RequestingService] = new WeakReference(e.Service);
                            return e.Service;
                            }
                        }
                    }
                return null;
                }
            }
        #endregion
        #region M:GetService(Type):Object
        public static Object GetService(Type service) {
            if (service == null) { throw new ArgumentNullException(nameof(service)); }
            lock (Services)
                {
                return GetService((Object)service);
                }
            }
        #endregion

        private static IDictionary<Object,WeakReference> Services = new Dictionary<Object,WeakReference>();
        }
    }
