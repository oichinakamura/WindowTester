using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public class Border : SysCtrl.Border
    {
        public int GridColumn { get => SysCtrl.Grid.GetColumn(this); set => SysCtrl.Grid.SetColumn(this, value); }
        public int GridRow { get => SysCtrl.Grid.GetRow(this); set => SysCtrl.Grid.SetRow(this, value); }
        public SysCtrl.Dock Dock { get => DockPanel.GetDock(this); set => DockPanel.SetDock(this, value); }
    }
}
