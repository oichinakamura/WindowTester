namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public partial class Button : SysCtrl.Button
    {
        public Button()
        {
            Bindings = new BindingCollection(this);
        }

       
        public SysCtrl.Dock Dock { get => DockPanel.GetDock(this); set => DockPanel.SetDock(this, value); }
    }
}
