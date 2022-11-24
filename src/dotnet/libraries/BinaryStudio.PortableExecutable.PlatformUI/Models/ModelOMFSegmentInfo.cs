using System;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSegmentInfo))]
    internal class ModelOMFSegmentInfo : NotifyPropertyChangedDispatcherObject<OMFSegmentInfo>
        {
        public Int32 Order{ get; }
        public ModelOMFSegmentInfo(Int32 order, OMFSegmentInfo source)
            : base(source)
            {
            Order = order;
            }
        }
    }