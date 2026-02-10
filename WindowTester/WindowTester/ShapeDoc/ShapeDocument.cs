namespace HIMTools.ShapeDoc
{
    using HIMTools.Applications;
    using HIMTools.Controls;
    using HIMTools.Input;
    using HIMTools.Interface;
    using HIMTools.MapTools;
    using HIMTools.MapTools.MapControls;
    using HIMTools.MapTools.RasterContentLib;
    using HIMTools.Xml;
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Xml.Linq;
    using SysCtrl = System.Windows.Controls;

    public class ShapeDocument : XeDocument, ITabPage, IOperationTarget, ICommand, INotifyPropertySet
    {
        public ShapeDocument(FileInfo fileInfo)
        {
            if (fileInfo.Exists)
                Load(fileInfo.FullName);
            else
            {
                AppendChild(CreateElement("DocumentRoot"));
                DocumentElement.AppendChild(CreateElement("UserExtDataSet"));

                Save(fileInfo.FullName);
            }
            FilePath = fileInfo.FullName;
        }
        public Guid ID { get; set; }
        public string FilePath { get; private set; }

        public object Header => FilePath.IndexOf('\\') > 0 ? FilePath.Substring(FilePath.LastIndexOf('\\') + 1) : FilePath;

        public object Icon => "📒";
        private object content;

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public event PropertyChangedEventHandler PropertyChanged;

        public object Content
        {
            get
            {
                if (content == null)
                {
                    var gridLayout = new GridLayoutPanel()
                    {
                        Dock = SysCtrl.Dock.Bottom,
                        ColumnCount = 3,
                        Children =
                            {
                                new Button(){ Content="追加",Command=this,CommandParameter="追加" },
                                new ComboBox()
                                {
                                    Items = {
                                        "ラベル",
                                        "ポリライン",
                                        "ポリゴン"
                                    },
                                    SelectedItem = new Binding("ShapeType"){ Source = this, Mode=BindingMode.TwoWay }
                                },
                                new Button(){ Content="保存",Command=this,CommandParameter="保存" },
                            },
                    };


                    var dataGrid = new DataGrid()
                    {
                        AutoGenerateColumns = false,
                        Columns = {
                            new SysCtrl.DataGridTextColumn(){ Header="Title",Binding = new Binding("Title") },
                            new SysCtrl.DataGridTextColumn(){ Header="タイプ",Binding = new Binding("Type") }
                        }
                    };
                    dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Items") { Source = this });

                    content = new DockPanel() { Children = { gridLayout, dataGrid } };
                }
                return content;
            }
        }

        public override IMetaElement CreateElement(XName name) => this.CreateElement(name.LocalName);
        public override IMetaElement CreateElement(XName name, bool IsRoot) => this.CreateElement(name);
        public override IMetaElement CreateElement(string name)
        {
            switch (name)
            {
                case "DocumentRoot": return new ShapeDocumentRoot();
                case "UserExtDataSet": return new UserExtDataSet();
                case "ColorTable": return new ColorTable();
                case "UserExtTable": return new ShapeItem();
                case "point": return new ShapePoint();
            }
            return new XeElement(name);
        }

        public bool CanExecute(object parameter) => true;
        private string shapeType = "ポリライン";
        public string ShapeType { get => shapeType; set { shapeType = value; SendPropertyChanged(nameof(ShapeType)); } }

        public IEnumerable Items => DataSet.Items;

        public UserExtDataSet DataSet
        {
            get
            {
                if (DocumentElement.SelectSingleNode("//UserExtDataSet") is null)
                {
                    var dataSet = CreateElement("UserExtDataSet") as UserExtDataSet;
                    DocumentElement.AppendChild(dataSet);
                }
                return DocumentElement.SelectSingleNode("//UserExtDataSet") as UserExtDataSet;
            }
        }

        public void Execute(object parameter)
        {
            switch (parameter)
            {
                case "追加":
                    {
                        var newItem = CreateElement("UserExtTable") as ShapeItem;
                        newItem.SetAttribute("ID", $"{Guid.NewGuid()}");
                        newItem.SetElementValue("ID", $"{newItem.ID}");
                        newItem.SetElementValue("Type", $"{ShapeType}");
                        newItem.SetElementValue("Name", "新しいアイテム");

                        DataSet.AppendChild(newItem);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
                    }
                    break;
                case "保存":
                    {
                        Save(FilePath);
                        MessageBox.Show("保存しました", "保存");
                    }
                    break;
            }
        }

        public void SendPropertyChanged(params string[] propertyNames) { foreach (string propertyName in propertyNames) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        public void Draw(IVisualLayerCollection visualLayers)
        {
        }

        public void Save()
        {
            Save(FilePath);
            SendPropertyChanged("Items");
        }

        public void Draw(VectorDrawingVisual vectorDrawingVisual)
        {
            foreach (XeElement element in Items)
            {
                if (element is ShapeItem shapeItem)
                {

                    shapeItem.Draw(vectorDrawingVisual);
                }
            }
        }
    }

    public class ShapeDocumentRoot : XeElement
    {
        public ShapeDocumentRoot() : base("DocumentRoot")
        {
        }
    }

    public class UserExtDataSet : XeElement
    {
        public UserExtDataSet() : base("UserExtDataSet")
        {
        }
        public IEnumerable Items
        {
            get => SelectNodes("//UserExtTable");
        }
    }

    public class ColorTable : XeElement
    {
        public ColorTable() : base("ColorTable")
        {
        }
    }

    public partial class ShapeItem : XeElement
    {
        public ShapeItem() : base("UserExtTable")
        {
        }
        public string Title
        {
            get => GetElementValue("Name");
            set => SetElementValue("Name", value);
        }
        public string Type
        {
            get => GetElementValue("Type");
            set => SetElementValue("Type", value);
        }


        public void Draw(VectorDrawingVisual vectorDrawingVisual)
        {
            switch (GetElementValue("Type"))
            {
                case "ポリライン":
                    if (Points is null)
                    {
                        Points = new ObservableCollection<Point>();
                        foreach (ShapePoint point in SelectNodes(".//point"))
                            Points.Add(new Point() { X = point.X, Y = point.Y });
                    }

                    vectorDrawingVisual.DrawPolyline(Points.ToArray(), new Pen { Brush = Brushes.Black, Thickness = 1 });
                    break;
            }
        }
        private ObservableCollection<Point> Points;
    }

    public partial class ShapeItem : ITabPage, ICommand, INotifyPropertySet, IMapInputTarget, IDataGridItem
    {
        public override Guid ID { get => HasAttribute("ID") ? base.ID : CreateID(); set => base.ID = value; }
        public Guid CreateID() { SetAttribute("ID", Guid.NewGuid()); return base.ID; }
        public object Header => Title;

        public object Icon => Type switch
        {
            "ラベル" => "🏷️",
            "ポリライン" => "〰",
            "ポリゴン" => "⬡",
            _ => "�"
        };

        private object content;
        public object Content =>
             Type switch
             {
                 "ラベル" => CreateLabelPage(),
                 "ポリライン" => CreatePolyLinePage(),
                 "ポリゴン" => CreatePolygonPage(),
                 _ => null
             };

        private object CreateLabelPage()
        {
            content = new DockPanel()
            {
                Children =
                {
                    new TextBlock()
                    {
                        Text="ラベル編集用ページ",
                        Margin=new System.Windows.Thickness(10)
                    }
                }
            };
            return content;
        }

        private object CreatePolyLinePage()
        {
            var fontsize = 16.0;
            inputController = new InputPolyLineController(this, SelectNodes(".//point"));

            content = new DockPanel()
            {
                Children =
                {
                    new GridLayoutPanel()
                    {
                        Dock = SysCtrl.Dock.Top,
                        ColumnCount = 2,
                        ColumnDistributions = new float[]{1.0F,2.0F },
                        Children =
                        {
                            new TextBlock(){Text = "タイトル", VerticalAlignment = System.Windows.VerticalAlignment.Center},
                            new TextBox()
                            {
                                Bindings = { BindingTuple.Create(Property: TextBox.TextProperty,Path: "Values[Name]", Source: inputController, Mode: BindingMode.TwoWay) },
                                Margin = new System.Windows.Thickness(10),
                                HorizontalAlignment= System.Windows.HorizontalAlignment.Stretch,
                                VerticalContentAlignment=System.Windows.VerticalAlignment.Center
                            },
                        }
                    },
                        new GridLayoutPanel()
                        {
                            Dock = SysCtrl.Dock.Bottom,
                            ColumnCount = 5,
                            Children =
                            {
                                new Button()
                                {
                                    FontSize = fontsize,
                                    Bindings={
                                        BindingTuple.Create(Property: Button.ContentProperty, Path: "ButtonMode", Source: this, Mode: BindingMode.OneWay),
                                    },
                                    Command =this,
                                    CommandParameter ="入力モード切替",
                                },
                                new Button() { FontSize = fontsize, Content = "🗑️", Command = this, CommandParameter = "座標クリア" },
                                new Button() { FontSize = fontsize, Content = "\uE7A7",FontFamily=new FontFamily("Segoe MDL2 Assets") , Command = this, CommandParameter = "座標を戻す" },
                                new Button() { FontSize = fontsize, Content = "💾", Command = this, CommandParameter = "保存" },
                                new Button() { FontSize = fontsize, Content = "✖", Command = this, CommandParameter = "閉じる" }
                            }
                        },
                        new DataGrid()
                        {
                            AutoGenerateColumns = false,
                            Columns={
                                new SysCtrl.DataGridTextColumn(){ Header="座標X",Binding=new Binding("X") },
                                new SysCtrl.DataGridTextColumn(){ Header="座標Y",Binding=new Binding("Y") }
                            },
                            ItemsSource=(inputController as InputPolyLineController).Points
                        },
                }
            };



            return content;
        }

        private object CreatePolygonPage()
        {
            content = new DockPanel()
            {
                Children =
                {
                    new TextBlock()
                    {
                        Text="ポリゴン編集用ページ",
                        Margin=new System.Windows.Thickness(10)
                    }
                }
            };

            return content;
        }

        protected IInputController inputController;
        public string ButtonMode { get; set; } = "🖊";

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public void OnMouseDoubleClick()
        {
            if (Program.SysAD.QuadTabPages.Contains(ID, out TabPageCollection hit))
                hit.SelectedTab = this;
            else
                Program.SysAD.QuadTabPages.R1.Add<ITabPage>(this, true);
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            switch (parameter)
            {
                case "入力モード切替":
                    switch (ButtonMode)
                    {
                        case "🖊":
                            ButtonMode = "⏸️";
                            Program.SysAD.MapViewer.InputController = inputController;
                            inputController.IsEnabled = true;
                            Program.SysAD.MapViewer.MapRefresh();
                            break;
                        case "⏸️":
                            ButtonMode = "▶️";
                            inputController.IsEnabled = false;
                            Program.SysAD.MapViewer.MapRefresh();
                            break;
                        case "▶️":
                            ButtonMode = "⏸️";
                            inputController.IsEnabled = true;
                            Program.SysAD.MapViewer.MapRefresh();
                            break;
                    }
                    SendPropertyChanged("ButtonMode");
                    break;
                case "座標クリア":
                    {
                        if (inputController is InputPolyLineController polyLineController)
                            polyLineController.Points.Clear();
                    }
                    break;
                case "座標を戻す":
                    inputController.Undo();
                    break;
                case "保存":
                    {
                        switch (inputController)
                        {
                            case InputPolyLineController polyLineController:
                                if (!(SelectSingleNode(".//PointList") is XeElement pointList))
                                {
                                    pointList = CreateElement("PointList");
                                    AppendChild(pointList);
                                }
                                pointList.RemoveAll();
                                foreach (Point point in polyLineController.Points)
                                {
                                    var shapePoint = CreateElement("point") as ShapePoint;
                                    shapePoint.X = point.X;
                                    shapePoint.Y = point.Y;
                                    pointList.AppendChild(shapePoint);
                                }
                                Points.Clear();
                                foreach (ShapePoint point in SelectNodes(".//point"))
                                    Points.Add(new Point() { X = point.X, Y = point.Y });
                                break;
                        }
                        foreach (string key in inputController.Values.Keys)
                            SetElementValue(key, inputController.Values[key]);

                        if(OwnerDocument is ShapeDocument shapeDocument)
                            shapeDocument.Save();
                        MessageBox.Show("保存しました", "保存");
                    }
                    break;
                case "閉じる":
                    {
                        if (Program.SysAD.QuadTabPages.Contains(ID, out TabPageCollection hit))
                        {
                            hit.Remove(this);
                            Program.SysAD.MapViewer.InputController = null;
                        }
                    }
                    break;
            }
        }

        public void SendPropertyChanged(params string[] propertyNames) { foreach (string propertyName in propertyNames) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }




    public class ShapePoint : XeElement
    {
        public ShapePoint() : base("point")
        {
        }
        public double X
        {
            get => double.TryParse(GetAttribute("X"), out double x) ? x : 0;
            set => SetAttribute("X", $"{value}");
        }
        public double Y
        {
            get => double.TryParse(GetAttribute("Y"), out double y) ? y : 0;
            set => SetAttribute("Y", $"{value}");
        }
    }

    public class InputPolyLineController : Input.InputControllerBase
    {
        public InputPolyLineController(IMapInputTarget inputTarget, IEnumerable points) : base()
        {
            Owner = inputTarget;
            Values.Add("Name", Owner.GetElementValue("Name"));


            foreach (ShapePoint point in points)
                Points.Add(new Point() { X = point.X, Y = point.Y });
        }

        public ObservableCollection<Point> Points = new ObservableCollection<Point>();
        public override void Draw(IVisualLayerCollection visualLayerCollection)
        {
        }
        public override void Draw(VectorDrawingVisual vectorDrawingVisual)
        {
            if (Points.Count != 0)
            {
                for (int i = 1; i < Points.Count; i++)
                    if (Points[i - 1] is Point firstPos && Points[i] is Point secondPos)
                        vectorDrawingVisual.DrawLine(firstPos.X, firstPos.Y, secondPos.X, secondPos.Y, new Pen { Brush = Brushes.Red, Thickness = 1 });


                for (int i = 0; i < Points.Count; i++)
                    if (Points[i] is Point firstPos)
                        vectorDrawingVisual.DrawPoint(firstPos.X, firstPos.Y, Brushes.Red, 4);
            }
        }

        public override void PenDown(Point point)
        {
            Points.Add(point);
            Program.SysAD.MapViewer.MapRefresh();
        }

        public override void Undo()
        {
            if (Points.Count > 0)
            {
                Points.RemoveAt(Points.Count - 1);
                Program.SysAD.MapViewer.MapRefresh();
            }
        }
    }
}
