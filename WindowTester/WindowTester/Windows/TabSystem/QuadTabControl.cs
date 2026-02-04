namespace HIMTools.Windows.TabSystem
{
    using HIMTools.Controls;
    using System.Data;
    using System.Windows;
    using System.Windows.Media;
    using SysCtrl = System.Windows.Controls;
    using SysWin = System.Windows;

    public class QuadTabControl : Grid, IQuadTabControl
    {
        public QuadTabControl()
        {
            L1 = new TabControl() { };
            R1 = new TabControl() { };
            L2 = new TabControl() { GridRow = 2 };
            R2 = new TabControl() { GridRow = 2 };

            var LGrid = new Grid() { RowSplit = new object[] { 1d, 8, 0d }, GridColumn = 0, Children = { L1, GetSplitterH(1), L2 } };
            var RGrid = new Grid() { RowSplit = new object[] { 1d, 8, 0d }, GridColumn = 2, Children = { R1, GetSplitterH(1), R2 } };

            ColumnDefinitions.Add(new ColumnDefinition() { InnerContent = LGrid, Width = new SysWin.GridLength(3, SysWin.GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition() { InnerContent = GetSplitterV(1), Width = new SysWin.GridLength(8, SysWin.GridUnitType.Pixel) });
            ColumnDefinitions.Add(new ColumnDefinition() { InnerContent = RGrid, Width = new SysWin.GridLength(1, SysWin.GridUnitType.Star) });

            foreach (SysCtrl.ColumnDefinition column in ColumnDefinitions)
                if (column is ColumnDefinition columnDefinition)
                    if (columnDefinition.InnerContent != null)
                        Children.Add(columnDefinition.InnerContent);

            Children.Add(new GridSplitter() { GridColumn = 1 });
        }

        public TabControl L1 { get; private set; }
        public TabControl R1 { get; private set; }
        public TabControl L2 { get; private set; }
        public TabControl R2 { get; private set; }

        private GridSplitter GetSplitterH(int row) => new GridSplitter() { GridRow = row };
        private GridSplitter GetSplitterV(int column) => new GridSplitter() { GridColumn = column };
    }

    public class ColumnDefinition : SysCtrl.ColumnDefinition
    {
        public ColumnDefinition()
        {
        }
        protected UIElement innerContent;
        public UIElement InnerContent { get; set; }
    }
    public class RowDefinition : SysCtrl.RowDefinition
    {
        public RowDefinition()
        {
        }
        protected UIElement innerContent;
        public UIElement InnerContent { get; set; }
    }

}
