namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public class TextBlock : SysCtrl.TextBlock
    {
        public SysCtrl.Dock Dock { get => DockPanel.GetDock(this); set => DockPanel.SetDock(this, value); }
    }
}
