using System;
using System.Windows;

namespace BinaryStudio.PlatformUI.Controls
    {
    public class ProgressRingTemplateSettings : DependencyObject, IProgressRingTemplateSettings
        {
        public Double EllipseDiameter { get;set; }
        public Double MaxSideLength { get;set; }
        public Thickness EllipseOffset { get;set; }

        public ProgressRingTemplateSettings()
            {
            EllipseDiameter = 5;
            MaxSideLength = 500;
            }
        }
    }