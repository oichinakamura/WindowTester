namespace HIMTools.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using SysCtrl = System.Windows.Controls;
    using SysData = System.Windows.Data;
    using SysWin = System.Windows;

    public partial class QuadTabControl : IQuadTabControl
    {
        public QuadTabControl()
        {
            L1 = new TabControl() { };
            R1 = new TabControl() { };
            L2 = new TabControl() { GridRow = 2 };
            R2 = new TabControl() { GridRow = 2 };

            var LGrid = new Grid() { RowSplit = new object[] { 1d, 8, 0d }, GridColumn = 0, Children = { L1, GetSplitterH(1), L2 } };
            var RGrid = new Grid() { RowSplit = new object[] { 1d, 8, 0d }, GridColumn = 2, Children = { R1, GetSplitterH(1), R2 } };

            ColumnDefinitions.Add(new ColumnDefinition() { InnerContent = LGrid, Width = new SysWin.GridLength(3, SysWin.GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition() { InnerContent = GetSplitterV(1), Width = new SysWin.GridLength(8, SysWin.GridUnitType.Pixel) });
            ColumnDefinitions.Add(new ColumnDefinition() { InnerContent = RGrid, Width = new SysWin.GridLength(1, SysWin.GridUnitType.Star) });

            foreach (SysCtrl.ColumnDefinition column in ColumnDefinitions)
                if (column is ColumnDefinition columnDefinition)
                    if (columnDefinition.InnerContent != null)
                        Children.Add(columnDefinition.InnerContent);

            Children.Add(new GridSplitter() { GridColumn = 1 });
        }

        public TabControl L1 { get; private set; }
        public TabControl R1 { get; private set; }
        public TabControl L2 { get; private set; }
        public TabControl R2 { get; private set; }

        private GridSplitter GetSplitterH(int row) => new GridSplitter() { GridRow = row };
        private GridSplitter GetSplitterV(int column) => new GridSplitter() { GridColumn = column };


        public IQuadTabOwner QuadTabOwner
        {
            get => GetValue(QuadTabOwnerProperty) as IQuadTabOwner;
            set
            {
                SetValue(QuadTabOwnerProperty, value);
                L1.Itemssource = new SysData.Binding("L1") { Source = value };
                L1.SetBinding(SysCtrl.Primitives.Selector.SelectedItemProperty, new SysData.Binding("L1.SelectedTab") { Source = value, Mode = SysData.BindingMode.TwoWay });
                R1.Itemssource = new SysData.Binding("R1") { Source = value };
                R1.SetBinding(SysCtrl.Primitives.Selector.SelectedItemProperty, new SysData.Binding("R1.SelectedTab") { Source = value, Mode = SysData.BindingMode.TwoWay });
            }
        }

        public static readonly SysWin.DependencyProperty QuadTabOwnerProperty =
        SysWin.DependencyProperty.Register("QuadTabOwner", typeof(IQuadTabOwner), typeof(QuadTabControl), new SysWin.PropertyMetadata(null, OnQuadTabOwnerChanged));
        private static void OnQuadTabOwnerChanged(SysWin.DependencyObject d, SysWin.DependencyPropertyChangedEventArgs e)
        {
            var control = d as QuadTabControl;
            var owner = e.NewValue as IQuadTabOwner;
        }

    }

    public class ColumnDefinition : SysCtrl.ColumnDefinition
    {
        public ColumnDefinition()
        {
        }
        protected SysWin.UIElement innerContent;
        public SysWin.UIElement InnerContent { get; set; }
    }
    public class RowDefinition : SysCtrl.RowDefinition
    {
        public RowDefinition()
        {
        }
        protected SysWin.UIElement innerContent;
        public SysWin.UIElement InnerContent { get; set; }
    }


    public class QuadTabPageCollection : INotifyPropertyChanged
    {
        public QuadTabPageCollection()
        {
            L1.SelectedTabChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("L1.SelectedItem"));
            R1.SelectedTabChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("R1.SelectedItem"));
            L2.SelectedTabChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("L2.SelectedItem"));
            R2.SelectedTabChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("R2.SelectedItem"));
        }

        public bool Contains(ITabPage tabPage)
        {
            if (L1.Contains(tabPage)) return true;
            else if (R1.Contains(tabPage)) return true;
            else if (L2.Contains(tabPage)) return true;
            else if (R2.Contains(tabPage)) return true;
            else return false;
        }
        public bool Contains(Guid id)
        {
            if (L1.Contains(id)) return true;
            else if (R1.Contains(id)) return true;
            else if (L2.Contains(id)) return true;
            else if (R2.Contains(id)) return true;
            else return false;
        }
        public bool Contains(Guid id, out TabPageCollection HasCollection)
        {
            if (L1.Contains(id)) { HasCollection = L1; return true; }
            else if (R1.Contains(id)) { HasCollection = R1; return true; }
            else if (L2.Contains(id)) { HasCollection = L2; return true; }
            else if (R2.Contains(id)) { HasCollection = L2; return true; }
            else { HasCollection = null; return false; }
        }


        public TabPageCollection L1 = new TabPageCollection();
        public TabPageCollection R1 = new TabPageCollection();
        public TabPageCollection L2 = new TabPageCollection();
        public TabPageCollection R2 = new TabPageCollection();

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TabPageCollection : ObservableCollection<ITabPage>
    {
        public T Add<T>(ITabPage tabPage, bool select = false)
        {
            base.Add((ITabPage)tabPage);
            if (select)
                SelectedTab = tabPage;
            return (T)tabPage;
        }

        private int selectedIndex = -1;
        public ITabPage SelectedTab
        {
            get => selectedIndex >= 0 && selectedIndex < Count ? this[selectedIndex] : null;
            set
            {
                selectedIndex = IndexOf(value);
                SelectedTabChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(SelectedTab)));
            }
        }

        public bool Contains(Guid id)
        {
            foreach (var tab in this)
                if (tab.ID == id) return true;
            return false;
        }

        public event EventHandler SelectedTabChanged;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (selectedIndex >= Count)
            {
                selectedIndex = Count - 1;
                SelectedTabChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(SelectedTab)));
            }
            base.OnCollectionChanged(e);
        }
    }

    public interface IQuadTabControl
    {
        IQuadTabOwner QuadTabOwner { get; set; }
    }

    public interface IQuadTabOwner
    {
        QuadTabPageCollection QuadTabPages { get; }
        TabPageCollection L1 { get; }
        //ITabPage SelectedL1 { get; set; }
        TabPageCollection R1 { get; }
        //ITabPage SelectedR1 { get; set; }
    }


}
