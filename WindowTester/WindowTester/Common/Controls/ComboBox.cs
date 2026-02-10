namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public class ComboBox : SysCtrl.ComboBox
    {
        public new object SelectedItem
        {
            get => base.SelectedItem;
            set
            {
                switch (value)
                {
                    case System.Windows.Data.Binding binding: SetBinding(ComboBox.SelectedItemProperty, binding); break;
                    default: SelectedItem = value; break;
                }
            }
        }
    }
}
