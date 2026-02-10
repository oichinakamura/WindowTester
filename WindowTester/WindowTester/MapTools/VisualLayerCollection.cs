using HIMTools.MapTools.MapControls;
using HIMTools.MapTools.RasterContentLib;
using System.Collections.Generic;

namespace HIMTools.MapTools
{

    public class VisualLayerCollection : Dictionary<string, IVisualLayer>, IVisualLayerCollection
    {
        public VisualLayerCollection()
        {
            VectorDrawingVisual = new VectorDrawingVisual();
        }

        public void ClearAll()
        {
            this.Clear();
            if (VectorDrawingVisual != null)
            {
                VectorDrawingVisual.Dispatcher.Invoke(() => VectorDrawingVisual.Children.Clear());
                VectorDrawingVisual = null;
            }
            VectorDrawingVisual = new VectorDrawingVisual();

        }

        public IRastrVisualLayer BaseRastrLayer { get; set; }
        public VectorDrawingVisual VectorDrawingVisual { get; set; }
    }

    public interface IVisualLayerCollection
    {
        IRastrVisualLayer BaseRastrLayer { get; set; }
        VectorDrawingVisual VectorDrawingVisual { get; set; }
    }
}
