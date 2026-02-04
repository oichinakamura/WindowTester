namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    using SysWin = System.Windows;
    public class Grid : SysCtrl.Grid
    {

        public object[] RowSplit
        {
            set
            {
                RowDefinitions.Clear();
                foreach (var rowSize in value)
                    switch (rowSize)
                    {
                        case int pixel:
                            RowDefinitions.Add(new SysCtrl.RowDefinition { Height = new SysWin.GridLength(pixel, SysWin.GridUnitType.Pixel) });
                            break;
                        default:
                            RowDefinitions.Add(new SysCtrl.RowDefinition { Height = new SysWin.GridLength(double.Parse($"{rowSize}"), SysWin.GridUnitType.Star) });
                            break;
                    }
            }
        }

        public int GridColumn { get => Grid.GetColumn(this); set => Grid.SetColumn(this, value); }
        public int GridRow { get => Grid.GetRow(this); set => Grid.SetRow(this, value); }
        public SysCtrl.Dock Dock { get => SysCtrl.DockPanel.GetDock(this); set => SysCtrl.DockPanel.SetDock(this, value); }
    }

    public class GridSplitter : SysCtrl.GridSplitter
    {
        public GridSplitter() { HorizontalAlignment = SysWin.HorizontalAlignment.Stretch; }
        public int GridColumn { get => Grid.GetColumn(this); set => Grid.SetColumn(this, value); }
        public int GridRow { get => Grid.GetRow(this); set => Grid.SetRow(this, value); }
    }
}
