using HIMTools.Interface;
using System.Collections.Generic;
using System.ComponentModel;

namespace HIMTools.Applications
{
    public class ApplicationCollection : Dictionary<string, IApplicationBase>, IApplicationCollection, INotifyPropertySet
    {
        public ApplicationCollection()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void SendPropertyChanged(params string[] propertyNames) { foreach (string propertyName in propertyNames) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public interface IApplicationCollection
    {
        void Add(string key, IApplicationBase application);
        Dictionary<string, IApplicationBase>.ValueCollection Values { get; }
    }
}
