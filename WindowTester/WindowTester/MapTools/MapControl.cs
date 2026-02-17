
namespace HIMTools.MapTools
{
    using HIMTools.Applications;
    using HIMTools.Controls;
    using HIMTools.Input;
    using HIMTools.MapTools.MapControls;
    using HIMTools.MapTools.MapControls.RasterView;
    using HIMTools.MapTools.RasterContentLib;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    public partial class MapControl : Canvas //WindowsTester用
    {
        public MapControl(IMapViewer mapViewer)
        {
            parentViewer = mapViewer;

            MapBorder ??= new Border()
            {
                GridColumn = 2,
                Child = this,
                BorderBrush = System.Windows.Media.Brushes.DarkCyan,
                BorderThickness = new System.Windows.Thickness(1)
            };

            Children.Add(image);
            Children.Add(new Line() { X1 = 12, Y1 = 12, X2 = 14, Y2 = 14, Stroke = Brushes.Black, StrokeThickness = 1 });
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (Program.SysAD.MapViewer.InputController is IInputController inputController && inputController.IsEnabled)
                Program.SysAD.MapViewer.InputController.PenDown(e.GetPosition(this));
            else if (Program.SysAD.Applications.CurrentTarget != null)
                if (Program.SysAD.Applications.CurrentTarget.DefaultController != null)
                    Program.SysAD.Applications.CurrentTarget.DefaultController.PenDown(e.GetPosition(this));
            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {

            if (Program.SysAD.MapViewer.InputController is IInputController inputController && inputController.IsEnabled)
                Program.SysAD.MapViewer.InputController.PenMove(e.GetPosition(this), e.LeftButton);
            else if (Program.SysAD.Applications.CurrentTarget != null)
                if (Program.SysAD.Applications.CurrentTarget.DefaultController != null)
                    Program.SysAD.Applications.CurrentTarget.DefaultController.PenMove(e.GetPosition(this), e.LeftButton);

            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (Program.SysAD.MapViewer.InputController is IInputController inputController && inputController.IsEnabled)
                Program.SysAD.MapViewer.InputController.PenUp(e.GetPosition(this));
            else if (Program.SysAD.Applications.CurrentTarget != null)
                if (Program.SysAD.Applications.CurrentTarget.DefaultController != null)
                    Program.SysAD.Applications.CurrentTarget.DefaultController.PenUp(e.GetPosition(this));
            base.OnPreviewMouseUp(e);
        }


        public Border MapBorder { get; private set; }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            SetMapDrawImage();
            base.OnRenderSizeChanged(sizeInfo);
        }
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            //SetMapDrawImage();
        }

        private IMapViewer parentViewer;


        private Image image = new Image() { Stretch = Stretch.None };
        public void SetMapDrawImage()
        {
            image.Source = null;

            var vectorDrawingVisual = new VectorDrawingVisual();
            vectorDrawingVisual.Open();
            foreach (IApplicationBase applicationBase in Program.SysAD.Applications.Values)
            {
                applicationBase.Draw(vectorDrawingVisual);
            }

            if (Program.SysAD.MapViewer.InputController is IInputController mapViewerInputController && mapViewerInputController.Visibility == Visibility.Visible)
                mapViewerInputController.Draw(vectorDrawingVisual);
            else if (Program.SysAD.Applications.CurrentTarget != null)
                if (Program.SysAD.Applications.CurrentTarget.DefaultController != null)
                    Program.SysAD.Applications.CurrentTarget.DefaultController.Draw(vectorDrawingVisual);

            vectorDrawingVisual.Close();

            RenderTargetBitmap bmpstr = new RenderTargetBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);

            bmpstr.Render(vectorDrawingVisual);
            bmpstr.Freeze();


            image.Source = bmpstr;
        }


        // 描画イメージ作成処理
        public IRastrVisualLayer RasterLayer
        {
            get
            {
                if (parentViewer.VisualLayerCollection.BaseRastrLayer != null)
                    return parentViewer.VisualLayerCollection.BaseRastrLayer;
                else
                {
                    parentViewer.VisualLayerCollection.BaseRastrLayer = new RasterContentControl(this, parentViewer);
                    //parentViewer.VisualLayerCollection.BaseRastrLayer.PreviewMouseDoubleClick += (s, e) => { MouseDoubleClick(); };
                    Children.Add(parentViewer.VisualLayerCollection.BaseRastrLayer as System.Windows.Controls.ContentControl);

                    return parentViewer.VisualLayerCollection.BaseRastrLayer;
                }
            }
        }
    }
}
