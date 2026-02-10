using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public class TextBox : SysCtrl.TextBox
    {
        public BindingCollection Bindings { get; }
        public TextBox()
        {
            Bindings = new BindingCollection(this);
        }
    }

    public class BindingCollection : Dictionary<DependencyProperty, BindingBase>
    {
        public BindingCollection(FrameworkElement ownerElement)
        {
            OwnerElement = ownerElement;
        }

        protected FrameworkElement OwnerElement { get; private set; }
        public new void Add(DependencyProperty property, BindingBase binding)
        {
            OwnerElement.SetBinding(property, binding);
            base.Add(property, binding);
        }
        public void Add(Tuple<DependencyProperty, BindingBase> value)
        {
            OwnerElement.SetBinding(value.Item1, value.Item2);
            base.Add(value.Item1, value.Item2);
        }
    }
    public static class BindingTuple
    {
        public static Tuple<DependencyProperty, BindingBase> Create(DependencyProperty Property, string Path = null, object Source = null, BindingMode? Mode = null)
        {
            var binding = string.IsNullOrEmpty(Path) ? new Binding() : new Binding(Path);
            if (Source != null)
                binding.Source = Source;
            if (Mode.HasValue)
                binding.Mode = Mode.Value;

            return new Tuple<DependencyProperty, BindingBase>(Property, binding);
        }
    }
}
