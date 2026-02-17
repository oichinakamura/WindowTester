using HIMTools.Input;
using HIMTools.MapTools;
using HIMTools.MapTools.RasterContentLib;

namespace HIMTools.Interface
{
    public interface IOperationTarget
    {

        IInputController DefaultController { get; }
        void Draw(IVisualLayerCollection visualLayers);
        void Draw(VectorDrawingVisual vectorDrawingVisual);
    }
}
