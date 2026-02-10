namespace HIMTools.Controls
{

    public partial class Canvas
    {
        public int GridColumn { get => Grid.GetColumn(this); set => Grid.SetColumn(this, value); }
        public int GridRow { get => Grid.GetRow(this); set => Grid.SetRow(this, value); }

    }
}
