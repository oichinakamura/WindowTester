using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public class Button : SysCtrl.Button
    {
        public Button()
        {
            Bindings = new BindingCollection(this);
        }

        public BindingCollection Bindings { get; }
    }
}
