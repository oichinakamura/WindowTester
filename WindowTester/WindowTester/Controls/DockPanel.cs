using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    using SysWin = System.Windows;

    public class DockPanel : SysCtrl.DockPanel
    {

        public int GridColumn { get => Grid.GetColumn(this); set => Grid.SetColumn(this, value); }
        public int GridRow { get => Grid.GetRow(this); set => Grid.SetRow(this, value); }

        public void Docking(SysWin.UIElement element, SysCtrl.Dock? dock = null)
        {
            if (dock.HasValue)
                DockPanel.SetDock(element, dock.Value);

            Children.Add(element);
        }
    }
}
