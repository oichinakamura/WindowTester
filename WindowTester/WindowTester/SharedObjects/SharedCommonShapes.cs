namespace HIMTools.SharedObjects
{
    using HIMTools.Controls;
    using HIMTools.Input;
    using HIMTools.Interface;
    using HIMTools.MapTools;
    using HIMTools.MapTools.Data;
    using HIMTools.MapTools.RasterContentLib;
    using HIMTools.OperationTargets;
    using HIMTools.Xml;
    using System.Collections;
    using System.Data;
    using System.Xml.Linq;

    public partial class SharedCommonShapes : IOperationTarget, ITabPage // WindowsTest専用
    {
        public IInputController DefaultController => throw new System.NotImplementedException();

        public object Icon => "😬";

        public IEnumerable Children => ChildNodes;

        protected DataView dataSource;
        public DataView DataSource
        {
            get
            {
                if (dataSource == null)
                {
                    //if (SelectSingleNode($"//{SharedCommon.DataSetNamespace}") is SharedShapeZMLDataSet dataSetElement)
                    //{
                    //    var table = dataSetElement.GetTable(SharedCommon.DataTableName);
                    //    table.TableName = tableName;
                    //    Columns = new ObservableCollection<DataGridColumn>();
                    //}
                    dataSource = new DataView();
                }
                return dataSource;
            }
        }


        public void Draw(IVisualLayerCollection visualLayers)
        {
        }

        public void Draw(VectorDrawingVisual vectorDrawingVisual)
        {
        }


        public void Draw(IMapCanvases canvases, IFormattedTextCreator textCreator = null)
        {

        }


        protected void InitDefaultSetting()
        {

        }

        private IMetaElement CreateElementSub(XName name)
        {
            return new SharedDataElement(name);
        }

    }

    public class SharedDataElement : XeElement
    {
        public SharedDataElement(XName name) : base(name)
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
