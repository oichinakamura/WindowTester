using HIMTools.Interface;
using HIMTools.MapTools;
using HIMTools.MapTools.RasterContentLib;
using HIMTools.ShapeDoc;
using HIMTools.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace HIMTools.Input
{
    public abstract class InputControllerBase : IInputController
    {
        public virtual Visibility Visibility { get; set; } = Visibility.Visible;
        protected ShapeItem Item;

        protected InputControllerBase()
        {
            Values = new EditDictionary(this);
        }

        public Cursor Cursor => Cursors.Cross;

        public EditDictionary Values { get; private set; }
        public IMapInputTarget Owner { get; set; }
        public bool IsEnabled { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void SendPropertyChanged(params string[] propertyNames) { foreach (string propertyName in propertyNames) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); } }

        public abstract void PenDown(Point point);
        public virtual void PenMove(Point point, MouseButtonState LeftButton) { }
        public virtual void PenUp(Point point) { }
        public abstract void Draw(IVisualLayerCollection visualLayerCollection);

        public abstract void Draw(VectorDrawingVisual vectorDrawingVisual);

        public abstract void Undo();

    }

    public interface IInputController : INotifyPropertySet
    {
        IMapInputTarget Owner { get; }
        EditDictionary Values { get; }

        void Draw(IVisualLayerCollection visualLayerCollection);
        void Draw(VectorDrawingVisual vectorDrawingVisual);

        void PenDown(Point point);
        void PenMove(Point point, MouseButtonState LeftButton);
        void PenUp(Point point);
        void Undo();

        bool IsEnabled { get; set; }
        Visibility Visibility { get; }
    }

    public interface IMapInputTarget
    {
        string GetAttribute(string name);
        string GetElementValue(string name);
        IMetaElement SetElementValue(string name, string value);
    }

    public class EditDictionary : Dictionary<string, object>
    {
        public EditDictionary(INotifyPropertySet owner)
        {
            Owner = owner;
        }
        private INotifyPropertySet Owner;
        public new object this[string Key]
        {
            get => base[Key];
            set
            {
                base[Key] = value;
                Owner.SendPropertyChanged($"Values[{Key}]");
            }
        }
    }
}
