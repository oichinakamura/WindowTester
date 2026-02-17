using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;

    public partial class TabControl : SysCtrl.TabControl
    {
        public TabControl()
        {
            AllowDrop = true;
        }

        private Binding itemssource;
        public Binding Itemssource
        {
            get => itemssource;
            set
            {
                itemssource = value;

                var iconFactory = new FrameworkElementFactory(typeof(TextBlock));
                iconFactory.SetBinding(TextBlock.TextProperty, new Binding("Icon"));


                var contentFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                contentFactory.SetBinding(ContentPresenter.ContentProperty, new Binding("TabHeader") );
                contentFactory.SetValue(TextBlock.BackgroundProperty, Brushes.Transparent);
                contentFactory.SetValue(TextBlock.PaddingProperty, new Thickness(5));

                contentFactory.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(TextBlock_LeftButtonDown));
                contentFactory.AddHandler(UIElement.PreviewMouseMoveEvent, new MouseEventHandler(TextBlock_MouseMove));
                contentFactory.AddHandler(UIElement.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBlock_LeftButtonUp));

                var headerPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
                headerPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                headerPanelFactory.AppendChild(iconFactory);
                headerPanelFactory.AppendChild(contentFactory);

                DataTemplate headerTemplate = new DataTemplate() { VisualTree = headerPanelFactory };

                ItemTemplate = headerTemplate;

                var contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenterFactory.SetBinding(ContentPresenter.ContentProperty, new Binding("Content"));
                DataTemplate contentTemplate = new DataTemplate() { VisualTree = contentPresenterFactory };

                ContentTemplate = contentTemplate;

                SetBinding(ItemsSourceProperty, value);
            }
        }


        private Point? startPos = null;
        private void TextBlock_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPos = e.GetPosition(null);
        }
        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && startPos.HasValue)
            {
                var pos = e.GetPosition(null) - startPos;
                if (pos.Value.X * pos.Value.X + pos.Value.Y * pos.Value.Y > 8)
                    if (sender is FrameworkElement fe)
                        if (fe.TemplatedParent is FrameworkElement fp)
                            if (fp.TemplatedParent is FrameworkElement fpp)
                                if (fpp.DataContext is ITabPage tabPagePP)
                                    switch (DragDrop.DoDragDrop((System.Windows.DependencyObject)sender, tabPagePP, DragDropEffects.Move))
                                    {
                                        case DragDropEffects.Move:
                                            {
                                                var itemsSource = ItemsSource as ObservableCollection<ITabPage>;
                                                itemsSource.Remove(tabPagePP);
                                            }
                                            startPos = null;
                                            break;
                                    }

            }
        }
        private void TextBlock_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            startPos = null;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            foreach (string format in e.Data.GetFormats())
            {
                var obj = e.Data.GetData(format);

                if (obj is ITabPage item)
                {
                    var itemsSource = ItemsSource as ObservableCollection<ITabPage>;
                    {
                        if (itemsSource.Contains(item))
                        {
                            itemsSource.Remove(item);
                            itemsSource.Add(item);
                            e.Effects = DragDropEffects.Scroll;
                            return;
                        }
                        else
                        {
                            itemsSource.Add(item);
                            e.Effects = DragDropEffects.Move;
                            return;
                        }
                    }
                }
            }
            e.Effects = DragDropEffects.None;
            base.OnDrop(e);
        }

        public int GridColumn { get => Grid.GetColumn(this); set => Grid.SetColumn(this, value); }
        public int GridRow { get => Grid.GetRow(this); set => Grid.SetRow(this, value); }
        public SysCtrl.Dock Dock { get => SysCtrl.DockPanel.GetDock(this); set => SysCtrl.DockPanel.SetDock(this, value); }
    }

    public class StringITabPageHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public interface ITabPage
    {
        Guid ID { get; }
        object TabHeader { get; }
        object Icon { get; }
        object Content { get; }
    }
}
