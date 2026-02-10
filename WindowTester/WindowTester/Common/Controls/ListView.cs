using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIMTools.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using SysCtrl = System.Windows.Controls;

    public class ListView : SysCtrl.ListView
    {
        private IListViewOwner listViewOwner;

        public ListView()
        {
            GenerateGridView();
        }

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (SelectedItem is IListViewItem listViewItem)
            {
                listViewItem.OnMouseDoubleClick();
            }
            base.OnPreviewMouseDoubleClick(e);
        }

        private void GenerateGridView()
        {
            GridView gridView = new GridView();
            View = gridView;

            // 1列目: 名前 (TextBlock)
            GridViewColumn col1 = new GridViewColumn();
            col1.Header = "Name";
            col1.DisplayMemberBinding = new Binding("Header");
            gridView.Columns.Add(col1);

            // 2列目: SubItem1 (DataTemplateでカスタム定義)
            GridViewColumn col2 = new GridViewColumn();
            col2.Header = "Path";
            col2.CellTemplate = CreateDataTemplate("Path");
            gridView.Columns.Add(col2);

            // 3列目: SubItem2 (DataTemplateでハイパーリンクなど)
            GridViewColumn col3 = new GridViewColumn();
            col3.Header = "Sub 2";
            col3.CellTemplate = CreateDataTemplate("SubItem2");
            gridView.Columns.Add(col3);
        }
        private DataTemplate CreateDataTemplate(string bindingPath)
        {
            DataTemplate template = new DataTemplate();

            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));

            Binding binding = new Binding(bindingPath);
            textBlockFactory.SetBinding(TextBlock.TextProperty, binding);

            //textBlockFactory.SetValue(TextBlock.ForegroundProperty, System.Windows.Media.Brushes.Blue);

            template.VisualTree = textBlockFactory;
            return template;
        }

        public IListViewOwner ListViewOwner
        {
            get => listViewOwner;
            set
            {
                listViewOwner = value;
                if (value != null)
                {
                    this.SetBinding(ItemsSourceProperty, new Binding("Items") { Source = value });
                }
                else
                    this.ItemsSource = null;
            }
        }

        public SysCtrl.Dock Dock { get => DockPanel.GetDock(this); set => DockPanel.SetDock(this, value); }
    }
    public interface IListViewOwner : INotifyCollectionChanged, INotifyPropertyChanged
    {
        IEnumerable Items { get; }
    }
    public interface IListViewItem
    {
        object Header { get; }
        void OnMouseDoubleClick();
    }
}
