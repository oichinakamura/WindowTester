using System.Windows.Media;

namespace HIMTools.MapTools.RasterContentLib
{
    public class VectorDrawingVisual : DrawingVisual
    {
        private DrawingContext dc;

        public void Open()
        {
            dc = this.RenderOpen();
        }

        public void Close()
        {
            dc.Close();
            dc = null;
        }

        public void DrawPoint(double X, double Y, Brush brush, double size)
        {
            dc.DrawEllipse(brush, null, new System.Windows.Point(X, Y), size / 2, size / 2);
        }

        public void DrawLine(double X1, double Y1, double X2, double Y2, Pen pen)
        {
            dc.DrawLine(pen, new System.Windows.Point(X1, Y1), new System.Windows.Point(X2, Y2));
        }

        public void DrawPolyline(System.Windows.Point[] points, Pen pen)
        {
            for(int n=1;points.Length>n;n++)
            dc.DrawLine(pen, new System.Windows.Point(points[n-1].X, points[n - 1].Y), new System.Windows.Point(points[n].X, points[n].Y));
        }

    }
}
