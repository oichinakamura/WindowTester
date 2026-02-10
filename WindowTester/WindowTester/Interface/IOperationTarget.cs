using HIMTools.MapTools;
using HIMTools.MapTools.RasterContentLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIMTools.Interface
{
    public interface IOperationTarget
    {
        void Draw(IVisualLayerCollection visualLayers);
        void Draw(VectorDrawingVisual vectorDrawingVisual);
    }
}
