using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public partial class ScrollBar : System.Windows.Controls.Primitives.ScrollBar
    {
        public SysCtrl.Dock Dock { get => SysCtrl.DockPanel.GetDock(this); set => SysCtrl.DockPanel.SetDock(this, value); }
    }
}
