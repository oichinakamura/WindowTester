namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public class StackPanel : SysCtrl.StackPanel
    {
        public SysCtrl.Dock Dock { get => DockPanel.GetDock(this); set => DockPanel.SetDock(this, value); }
    }
}
