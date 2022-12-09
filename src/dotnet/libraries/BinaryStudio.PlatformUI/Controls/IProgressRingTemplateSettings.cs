using System;
using System.Windows;

namespace BinaryStudio.PlatformUI.Controls
    {
    public interface IProgressRingTemplateSettings
        {
        Double EllipseDiameter { get; }
        Double MaxSideLength { get; }
        Thickness EllipseOffset { get; }
        }
    }