using HIMTools.Interface;
using HIMTools.MapTools;
using HIMTools.MapTools.RasterContentLib;
using HIMTools.Xml;
using System;
using System.Collections.ObjectModel;

namespace HIMTools.Applications
{
    public abstract class ApplicationBase : XeDocument, IApplicationBase
    {

        public OperationTargets.OperationTargetCollection OperationTargets { get; } = new OperationTargets.OperationTargetCollection();

        public abstract void Draw(IVisualLayerCollection visualLayers);
        public abstract void Draw(VectorDrawingVisual vectorDrawingVisual);
    }
    public interface IApplicationBase
    {
        void Draw(IVisualLayerCollection visualLayers);
        void Draw(VectorDrawingVisual vectorDrawingVisual);
        OperationTargets.OperationTargetCollection OperationTargets { get; }
    }
}
namespace HIMTools.OperationTargets
{
    public class OperationTargetCollection : ObservableCollection<IOperationTarget>, IOperationTargetCollection
    {

    }

    public interface IOperationTargetCollection
    {

    }
}