﻿using System;
using BinaryStudio.PlatformUI.Models;

namespace BinaryStudio.PortableExecutable.PlatformUI.Models
    {
    [Model(typeof(OMFSegmentInfo))]
    internal class ModelOMFSegmentInfo : NotifyPropertyChangedDispatcherObject<OMFSegmentInfo>
        {
        public Int32 Order { get; }
        public String FormattedValue { get; }
        public ModelOMFSegmentInfo(Int32 order, OMFSegmentInfo source)
            : base(source)
            {
            Order = order;
            FormattedValue = $"{source.Index:x4}:{source.Offset:x8}-{source.Offset + source.Size:x8}";
            }
        }
    }