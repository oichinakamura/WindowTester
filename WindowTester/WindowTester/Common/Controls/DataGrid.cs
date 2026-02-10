using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public class DataGrid : SysCtrl.DataGrid
    {
        protected override void OnMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedItem is IDataGridItem dataGridItem)
            {
                dataGridItem.OnMouseDoubleClick();
            }
            base.OnMouseDoubleClick(e);
        }
        public SysCtrl.Dock Dock { get => DockPanel.GetDock(this); set => DockPanel.SetDock(this, value); }
    }

    public interface IDataGridItem
    {
        void OnMouseDoubleClick();
    }
}
