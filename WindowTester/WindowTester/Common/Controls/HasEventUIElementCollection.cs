using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace HIMTools.Controls
{
    public class HasEventUIElementCollection : UIElementCollection, INotifyCollectionChanged
    {
        public HasEventUIElementCollection(UIElement visualParent, FrameworkElement logicalParent) : base(visualParent, logicalParent) { }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public override int Add(UIElement element)
        {
            int result = base.Add(element);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, element));
            return result;
        }
    }
}
