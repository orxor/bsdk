#if NET35
namespace System.ComponentModel.DataAnnotations
    {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DisplayAttribute : Attribute
        {
        public String Name { get;set; }
        public String ShortName { get;set; }
        public String Description { get;set; }
        }
    }
#endif