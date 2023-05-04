using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using BinaryStudio.DiagnosticServices;

namespace BinaryStudio.PlatformUI.Extensions.Transfer
    {
    internal class FrameworkContentElementOperations
        {
        #region M:ForEachDescendants(FrameworkContentElement,Action<FrameworkContentElement>)
        public static void ForEachDescendants(FrameworkContentElement source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                ForEachDescendants(source as FlowDocument,predicate);
                ForEachDescendants(source as TableColumn,predicate);
                ForEachDescendants(source as TextElement,predicate);
                }
            }
        #endregion
        #region M:ForSelfAndEachDescendants(FrameworkContentElement,Action<FrameworkContentElement>)
        public static void ForSelfAndEachDescendants(FrameworkContentElement source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                predicate(source);
                ForEachDescendants(source as FlowDocument,predicate);
                ForEachDescendants(source as TableColumn,predicate);
                ForEachDescendants(source as TextElement,predicate);
                }
            }
        #endregion
        #region M:ForEachDescendants(FlowDocument,Action<FrameworkContentElement>)
        private static void ForEachDescendants(FlowDocument source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    var i = source.Blocks.FirstBlock;
                    while (i != null) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        i = i.NextBlock;
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(TextElement,Action<FrameworkContentElement>)
        private static void ForEachDescendants(TextElement source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                ForEachDescendants(source as Block,predicate);
                ForEachDescendants(source as Inline,predicate);
                ForEachDescendants(source as ListItem,predicate);
                ForEachDescendants(source as TableCell,predicate);
                ForEachDescendants(source as TableRow,predicate);
                ForEachDescendants(source as TableRowGroup,predicate);
                }
            }
        #endregion
        #region M:ForEachDescendants(Block,Action<FrameworkContentElement>)
        private static void ForEachDescendants(Block source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                ForEachDescendants(source as BlockUIContainer,predicate);
                ForEachDescendants(source as List,predicate);
                ForEachDescendants(source as Paragraph,predicate);
                ForEachDescendants(source as Section,predicate);
                ForEachDescendants(source as Table,predicate);
                }
            }
        #endregion
        #region M:ForEachDescendants(Inline,Action<FrameworkContentElement>)
        private static void ForEachDescendants(Inline source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                ForEachDescendants(source as AnchoredBlock,predicate);
                ForEachDescendants(source as InlineUIContainer,predicate);
                ForEachDescendants(source as LineBreak,predicate);
                ForEachDescendants(source as Run,predicate);
                ForEachDescendants(source as Span,predicate);
                }
            }
        #endregion
        #region M:ForEachDescendants(List,Action<FrameworkContentElement>)
        private static void ForEachDescendants(List source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    var i = source.ListItems.FirstListItem;
                    while (i != null) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        i = i.NextListItem;
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(Paragraph,Action<FrameworkContentElement>)
        private static void ForEachDescendants(Paragraph source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    var i = source.Inlines.FirstInline;
                    while (i != null) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        i = i.NextInline;
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(Section,Action<FrameworkContentElement>)
        private static void ForEachDescendants(Section source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    var i = source.Blocks.FirstBlock;
                    while (i != null) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        i = i.NextBlock;
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(Table,Action<FrameworkContentElement>)
        private static void ForEachDescendants(Table source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    foreach (var i in source.RowGroups.ToArray()) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(TableRowGroup,Action<FrameworkContentElement>)
        private static void ForEachDescendants(TableRowGroup source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    foreach (var i in source.Rows.ToArray()) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(TableRow,Action<FrameworkContentElement>)
        private static void ForEachDescendants(TableRow source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    foreach (var i in source.Cells.ToArray()) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(TableCell,Action<FrameworkContentElement>)
        private static void ForEachDescendants(TableCell source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    var i = source.Blocks.FirstBlock;
                    while (i != null) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        i = i.NextBlock;
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(Span,Action<FrameworkContentElement>)
        private static void ForEachDescendants(Span source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    var i = source.Inlines.FirstInline;
                    while (i != null) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        i = i.NextInline;
                        }
                    }
                }
            }
        #endregion
        #region M:ForEachDescendants(ListItem,Action<FrameworkContentElement>)
        private static void ForEachDescendants(ListItem source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            if (source != null) {
                using (new DebugScope()) {
                    var i = source.Blocks.FirstBlock;
                    while (i != null) {
                        predicate(i);
                        ForEachDescendants(i,predicate);
                        i = i.NextBlock;
                        }
                    }
                }
            }
        #endregion

        #region M:ForEachDescendants(TableColumn,Action<FrameworkContentElement>)
        private static void ForEachDescendants(TableColumn source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            }
        #endregion
        #region M:ForEachDescendants(BlockUIContainer,Action<FrameworkContentElement>)
        private static void ForEachDescendants(BlockUIContainer source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            }
        #endregion
        #region M:ForEachDescendants(AnchoredBlock,Action<FrameworkContentElement>)
        private static void ForEachDescendants(AnchoredBlock source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            }
        #endregion
        #region M:ForEachDescendants(InlineUIContainer,Action<FrameworkContentElement>)
        private static void ForEachDescendants(InlineUIContainer source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            }
        #endregion
        #region M:ForEachDescendants(LineBreak,Action<FrameworkContentElement>)
        private static void ForEachDescendants(LineBreak source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            }
        #endregion
        #region M:ForEachDescendants(Run,Action<FrameworkContentElement>)
        private static void ForEachDescendants(Run source, Action<FrameworkContentElement> predicate) {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate)); }
            }
        #endregion
        }
    }