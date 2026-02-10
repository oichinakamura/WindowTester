
namespace HIMTools.MapTools
{
    using HIMTools.Controls;
    using HIMTools.Input;
    using HIMTools.MapTools.MapControls.RasterView;
    using System;
    using Media = System.Windows.Media;
    using SysCtrl = System.Windows.Controls;
    using SysWin = System.Windows;

    public partial class MapViewer : IMapViewer
    {
        public MapViewer()
        {
            BaseRasterLayer = new RasterContentControl(Control, this);

            Control = new MapControl(this) { Background = Media.Brushes.AliceBlue };


            var scrollH = new ScrollBar() { Orientation = SysCtrl.Orientation.Horizontal, Height = 20, Dock = System.Windows.Controls.Dock.Bottom };
            var scrollV = new ScrollBar() { Orientation = SysCtrl.Orientation.Vertical, Width = 20, Dock = System.Windows.Controls.Dock.Right };

            var body = new DockPanel();

            body.Children.Add(scrollH);
            body.Children.Add(scrollV);

            MapNaviGrd = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition() { Width = new SysWin.GridLength(0) },
                    new ColumnDefinition() { Width = SysWin.GridLength.Auto },
                    new ColumnDefinition() { Width = new SysWin.GridLength(1.0, SysWin.GridUnitType.Star) }
                },
                Children = { Control.MapBorder }
            };

            body.Children.Add(MapNaviGrd);

            Content = body;
        }
        public MapControl Control { get; set; }
        public Grid MapNaviGrd { get; set; }
        public RasterContentControl BaseRasterLayer { get; set; }

        public IVisualLayerCollection VisualLayerCollection { get; } = new VisualLayerCollection();


        private IInputController inputController;
        public IInputController InputController
        {
            get => inputController;
            set
            {
                inputController = value;
                MapRefresh();
            }
        }

        public void MapRefresh()
        {
            Control.SetMapDrawImage();
        }
    }



    public interface IMapViewer
    {
        RasterContentControl BaseRasterLayer { get; set; }
        IVisualLayerCollection VisualLayerCollection { get; }
        void MapRefresh();
        IInputController InputController { get; set; }
    }

    public partial class MapViewer : ITabPage
    {

        public void CreateContent()
        {

        }
        public object Icon { get => "🖥️"; }
        public object Header { get => "地図"; }
        public object Content { get; set; }

        public Guid ID { get; } = Guid.NewGuid();
    }

}

