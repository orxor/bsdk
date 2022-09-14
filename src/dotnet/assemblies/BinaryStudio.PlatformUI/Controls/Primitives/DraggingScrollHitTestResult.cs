using System;
using System.Windows;
using System.Windows.Media;

namespace BinaryStudio.PlatformUI.Controls.Primitives
    {
    public class DraggingScrollHitTestResult : PointHitTestResult
        {
        public DraggingScrollHitTestResultKind Kind { get; }
        public Vector Offset { get; }
        public DraggingScrollHitTestResult(Visual visualHit, Point pointHit, DraggingScrollHitTestResultKind kind, Vector offset)
            : base(visualHit, pointHit)
            {
            Kind = kind;
            Offset = offset;
            }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
            {
            return $"{Offset}:{Kind}";
            }
        }
    }