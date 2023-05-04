using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;
using JetBrains.Annotations;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    [UsedImplicitly]
    [CloneFactory(typeof(Figure))]
    internal class TransferFigureFactory : TransferAnchoredBlockFactory<Figure>
        {
        /// <summary>Copies properties from one instance to another.</summary>
        /// <param name="Source">Source of properties.</param>
        /// <param name="Target">Target where properties are copied to.</param>
        protected override void Transfer(Figure Source, Figure Target) {
            if (Source == null) { return; }
            base.Transfer(Source, Target);
            using (new DebugScope()) {
                Transfer(Source,Target,Figure.CanDelayPlacementProperty);
                Transfer(Source,Target,Figure.HeightProperty);
                Transfer(Source,Target,Figure.HorizontalAnchorProperty);
                Transfer(Source,Target,Figure.HorizontalOffsetProperty);
                Transfer(Source,Target,Figure.VerticalAnchorProperty);
                Transfer(Source,Target,Figure.VerticalOffsetProperty);
                Transfer(Source,Target,Figure.WidthProperty);
                Transfer(Source,Target,Figure.WrapDirectionProperty);
                Transfer(Source,Target,FrameworkContentElement.DataContextProperty);
                }
            }
        }
    }