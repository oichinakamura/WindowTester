using HIMTools.Xml;
using System;
using System.Xml.Linq;

namespace HIMTools.AppSystem
{
    using HIMTools.Controls;
    using SysCtrl = System.Windows.Controls;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Input;
    using System.Windows.Data;
    using HIMTools.DBManSrv;
    using HIMTools.Interface;

    internal class SystemConfigRoot : TabPageElement, INotifyPropertySet, ICommand
    {
        public SystemConfigRoot() : base("SystemConfig")
        {
            Values = new EditDictionary(this);
        }

        public override object Header => "SystemConfig";
        public override object TabHeader => "SystemConfig";
        public override object Icon => "≡";

        public override object Content
        {
            get
            {
                var container = new DockPanel()
                {
                    Children =
                    {
                        new StackPanel()
                        {
                            Dock = SysCtrl.Dock.Bottom,
                            Children =
                            {
                                new Button(){ Content="新規",Command=this,CommandParameter="新規" }
                            }
                        },
                        new GridLayoutPanel()
                        {
                            Dock = SysCtrl.Dock.Top,
                            ColumnCount = 3,
                            ColumnDistributions = new object[] { "0.5*", "*", "0.5*"},
                            Children = {
                                new TextBlock{ Text = "Entry" },
                                new ComboBox{
                                    DisplayMemberPath = "Header",
                                    ItemsSourceBinding = new Binding("Entries"){ Source = SystemAD.SysAD },
                                    Bindings = {
                                        BindingTuple.Create(Property:ComboBox.SelectedItemProperty, Path:"SelectedEntry", Source:this,UpdateSourceTrigger:UpdateSourceTrigger.PropertyChanged)
                                    }
                                },
                                new Button{ Content="開く", Command = this, CommandParameter = "Entryを開く" }
                            },
                            Margin = new System.Windows.Thickness(10)
                        },
                        new ListView
                        {
                            Margin = new System.Windows.Thickness(10),
                            ListViewOwner = UsedFiles,
                        },

                    }
                };

                return container;
            }
        }

        public IEntry SelectedEntry { get => Values[nameof(SelectedEntry)] as IEntry; set => Values.Add(nameof(SelectedEntry), value); }
        public UsedFileCollection UsedFiles
        {
            get
            {
                if (SelectSingleNode("//UsedFiles") is XeElement element)
                    if (element is UsedFileCollection usedFileCollection)
                        return usedFileCollection;
                return this.AppendChild<UsedFileCollection>(new UsedFileCollection());
            }
        }
        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            switch (parameter)
            {
                case "新規":
                    {
                        var saveFileDialog = new Microsoft.Win32.SaveFileDialog() { Title = "Select a file to add", Filter = "ShapeXml(*.xml)|*.xml|All Files|*.*", CheckPathExists = true, OverwritePrompt = false, };
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            var newDoc = new ShapeDoc.ShapeDocument(new System.IO.FileInfo(saveFileDialog.FileName));

                            if (newDoc != null)
                            {
                                var newFile = UsedFiles.AddUsedFile(saveFileDialog.FileName);
                                (OwnerDocument as CSystemAD)?.Save();
                            }
                        }
                        break;
                    }
                case "Entryを開く":
                    if (SelectedEntry != null)
                        if (Values.TryGet<IEntry>(nameof(SelectedEntry), out IEntry result))
                        {
                            SystemAD.SysAD.CurrentEntry = SelectedEntry;
                            SystemAD.SysAD.QuadTabPages.R1.Add<IEntry>(result, true);
                        }
                    break;
            }
        }
        public EditDictionary Values { get; }
        public void SendPropertyChanged(params string[] propertyNames) { foreach (string propertyName in propertyNames) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class UsedFileCollection : XeElement, IListViewOwner
    {
        public UsedFileCollection() : base("UsedFiles")
        {
            this.Changed += (s, e) =>
            {
                switch (e.ObjectChange)
                {
                    case XObjectChange.Add: CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add)); break;
                    case XObjectChange.Value: CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace)); break;
                    case XObjectChange.Remove: CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove)); break;
                    default: CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); break;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
            };
        }

        public UsedFileElement AddUsedFile(string path)
        {
            var usedFile = new UsedFileElement() { Path = path };
            AppendChild(usedFile);
            return usedFile;
        }
        public IEnumerable Items => ChildNodes;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class UsedFileElement : XeElement, IListViewItem
    {
        public UsedFileElement() : base("File")
        {
        }
        public string Path
        {
            get => GetAttribute("Path");
            set => SetAttribute("Path", value);
        }

        public object Header => Path.IndexOf('\\') > 0 ? Path.Substring(Path.LastIndexOf('\\') + 1) : Path;


        public override Guid ID { get => HasAttribute("ID") ? base.ID : CreateID(); set => base.ID = value; }
        public Guid CreateID() { SetAttribute("ID", Guid.NewGuid()); return base.ID; }

        public void OnMouseDoubleClick()
        {
            if (!(Program.SysAD.QuadTabPages.Contains(ID)))
            {
                var shapeDoc = new ShapeDoc.ShapeDocument(new System.IO.FileInfo(Path));
                shapeDoc.ID = ID;
                Program.SysAD.Tool001.OperationTargets.Add(shapeDoc);
                Program.SysAD.Applications.CurrentTarget = shapeDoc;
                Program.SysAD.QuadTabPages.R1.Add<ITabPage>(shapeDoc, true);
                Program.SysAD.MapViewer.MapRefresh();
            }
        }
    }

    public class TabPageElement : XeElement, ITabPage
    {
        public TabPageElement(XName name) : base(name)
        {
        }

        public virtual object Header => throw new NotImplementedException();
        public virtual object Icon => throw new NotImplementedException();
        public virtual object Content { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual object TabHeader => throw new NotImplementedException();
    }
}
