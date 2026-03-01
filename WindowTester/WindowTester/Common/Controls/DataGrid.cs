namespace HIMTools.Controls
{
    using System.Windows;
    using SysCtrl = System.Windows.Controls;
    public partial class DataGrid : SysCtrl.DataGrid
    {
        protected override void OnMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedItem is IDataGridItem dataGridItem)
            {
                dataGridItem.OnMouseDoubleClick();
            }
            base.OnMouseDoubleClick(e);
        }
    }

    public interface IDataGridItem
    {
        void OnMouseDoubleClick();
    }

    public class DataGridTextColumn : SysCtrl.DataGridTextColumn
    {
        private static void OnColumnHorizontalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGridTextColumn)d).UpdateColumnStyle();
        }

        public static readonly DependencyProperty HorizontalContentAlignmentProperty =
           DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(DataGridTextColumn), new PropertyMetadata(HorizontalAlignment.Left, OnColumnHorizontalAlignmentChanged));


        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        private void UpdateColumnStyle()
        {
            Style elementStyle = new Style(typeof(SysCtrl.TextBlock));
            elementStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalContentAlignment));

            if (ElementStyle != null)
                elementStyle.BasedOn = ElementStyle;

            this.ElementStyle = elementStyle;
        }
    }
}
