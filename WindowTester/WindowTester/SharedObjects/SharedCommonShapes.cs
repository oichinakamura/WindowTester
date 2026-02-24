namespace HIMTools.SharedObjects
{
    using HIMTools.Controls;
    using HIMTools.Input;
    using HIMTools.Interface;
    using HIMTools.MapTools;
    using HIMTools.MapTools.RasterContentLib;
    using HIMTools.OperationTargets;
    using System.Collections;

    public partial class SharedCommonShapes : IOperationTarget, ITabPage // WindowsTest専用
    {
        public IInputController DefaultController => throw new System.NotImplementedException();

        public object TabHeader => Title;
        public object Icon => "😬";

        public object Content => new DataGrid()
        {
            Bindings = { BindingTuple.Create(
                Property:DataGrid.ItemsSourceProperty,
                Path:"Children",
                Source:this)
            }
        };

        public IEnumerable Children => ChildNodes;

        public void Draw(IVisualLayerCollection visualLayers)
        {
        }

        public void Draw(VectorDrawingVisual vectorDrawingVisual)
        {
        }

        protected void InitDefaultSetting()
        {

        }
    }

    public class SharedCommonShapesExt : OperationTargetExt // WindowsTest専用
    {
        public SharedCommonShapesExt(string title, IOperationTarget operationTarget) : base(title, operationTarget)
        {
        }
    }
}
