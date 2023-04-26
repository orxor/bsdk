using BinaryStudio.PlatformUI.Models;
using BinaryStudio.Security.Cryptography.AbstractSyntaxNotation;

namespace BinaryStudio.Security.Cryptography.PlatformUI.Models
    {
    [Model(typeof(Asn1Object))]
    [Model(typeof(Asn1Sequence))]
    public class EAsn1 : NotifyPropertyChangedDispatcherObject<Asn1Object>
        {
        public EAsn1(Asn1Object source)
            : base(source)
            {
            }
        }
    }
