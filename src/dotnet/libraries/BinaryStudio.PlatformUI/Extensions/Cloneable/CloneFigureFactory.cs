using System.Windows;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Cloneable
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Figure))]
    internal class CloneFigureFactory : CloneAnchoredBlockFactory<Figure>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void CopyTo(Figure Source, Figure Target) {
            if (Source == null) { return; }
            base.CopyTo(Source, Target);
            CopyTo(Source,Target,Figure.CanDelayPlacementProperty);
            CopyTo(Source,Target,Figure.HeightProperty);
            CopyTo(Source,Target,Figure.HorizontalAnchorProperty);
            CopyTo(Source,Target,Figure.HorizontalOffsetProperty);
            CopyTo(Source,Target,Figure.VerticalAnchorProperty);
            CopyTo(Source,Target,Figure.VerticalOffsetProperty);
            CopyTo(Source,Target,Figure.WidthProperty);
            CopyTo(Source,Target,Figure.WrapDirectionProperty);
            CopyTo(Source,Target,FrameworkContentElement.DataContextProperty);
            }
        }
    }