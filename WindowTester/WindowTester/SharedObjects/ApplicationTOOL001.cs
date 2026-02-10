using HIMTools.Applications;
using HIMTools.Interface;
using HIMTools.MapTools;
using HIMTools.MapTools.MapControls;
using HIMTools.MapTools.RasterContentLib;
using System.Collections.ObjectModel;

namespace HIMTools.SharedObjects
{
    public class ApplicationTOOL001 : ApplicationBase
    {
        public ApplicationTOOL001()
        {


        }



        public override void Draw(IVisualLayerCollection visualLayers)
        {
            foreach (IOperationTarget target in this.OperationTargets)
                target.Draw(visualLayers);
        }

        public override void Draw(VectorDrawingVisual vectorDrawingVisual)
        {
            foreach (IOperationTarget target in this.OperationTargets)
                target.Draw(vectorDrawingVisual);
        }
    }
}
