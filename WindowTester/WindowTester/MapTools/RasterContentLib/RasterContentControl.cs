namespace HIMTools.MapTools.MapControls.RasterView
{
    using HIMTools.Applications;
    using HIMTools.Input;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Media = System.Windows.Media;
    using SysCtrl = System.Windows.Controls;

    public partial class RasterContentControl : IRastrVisualLayer //WindowsTester用
    {
        public RasterContentControl(MapControl parentControl, IMapViewer mapViewer)
        {
            Body = new Controls.Image() { Stretch = Media.Stretch.None };
            //Content = Body;
            RasterContent = new RasterContent();

            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            visualParent = mapViewer.VisualLayerCollection;
        }
        IVisualLayerCollection visualParent;
        IVisualLayerCollection IVisualLayer.Parent { get => visualParent; set => visualParent = value; }
        public void DrawLine(double X1, double Y1, double X2, double Y2, Pen pen)
        {
            throw new System.NotImplementedException();
        }
        public void DrawLine(double X1, double Y1, double X2, double Y2, double width, Color color)
        {
            throw new System.NotImplementedException();
        }

        public void Rendering(double width, double height, double dpi)
        {
            Body.Source = null;

            foreach (IApplicationBase applicationBase in Program.SysAD.Applications.Values)
            {
                applicationBase.Draw(visualParent);
            }
            if (Program.SysAD.MapViewer.InputController is IInputController mapViewerInputController)
            {
                mapViewerInputController.Draw(visualParent);
            }



            //RenderTargetBitmap bmpstr = new RenderTargetBitmap((int)width, (int)height, dpi, dpi, PixelFormats.Pbgra32);
            //bmpstr.Render(visualParent.VectorDrawingVisual);
            //bmpstr.Freeze();
            var wstringbmp = new WriteableBitmap((int)width, (int)height, dpi, dpi, PixelFormats.Pbgra32, null);
            drawSub(10, 10, wstringbmp, Colors.Red);
            //wstringbmp.Freeze();
            Body.Source = wstringbmp;
        }

        private void drawSub(int x, int y, WriteableBitmap bitmap, Color color)
        {
            try
            {
                // 1. ロックしてバックバッファへのアクセスを開始
                bitmap.Lock();

                // 2. ピクセル情報の計算
                int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8; // 通常はBGRAの4バイト
                int stride = bitmap.BackBufferStride;

                // 点の色（BGRAの順）
                byte[] colorData = { color.B, color.G, color.R, color.A };

                // 3. バックバッファへ書き込み
                unsafe
                {
                    byte* pBackBuffer = (byte*)bitmap.BackBuffer;
                    // 該当するピクセルの位置を計算
                    int offset = (y * stride) + (x * bytesPerPixel);

                    // ピクセルデータをコピー
                    for (int i = 0; i < colorData.Length; i++)
                    {
                        *(pBackBuffer + offset + i) = colorData[i];
                    }
                }

                // 4. 変更した領域を通知
                bitmap.AddDirtyRect(new Int32Rect(x, y, 1, 1));
            }
            finally
            {
                // 5. ロックを解除
                bitmap.Unlock();
            }
        }
    }
}
