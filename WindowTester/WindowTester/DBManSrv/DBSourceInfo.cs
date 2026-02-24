using HIMTools.Xml;
using System.Xml.Linq;

namespace HIMTools.DBManSrv
{
    using HIMTools.Controls;
    using HIMTools.Users;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DBSourceInfo // WindowTester専用
    {
        public IMetaElement CreateElementSub(XName name)
        {
            return base.CreateElement(name);
        }
    }

    public abstract partial class DBSourceInfoElement : XeElement // WindowTester専用
    {
        public DBSourceInfoElement(string name) : base(name)
        {
        }

    }
    public partial class Entries : DBSourceInfoElement  // WindowTester専用
    {
        public Entries(bool dymmy) : base("Entries")
        {
            dymmy = !dymmy;
        }
    }

    public partial class Entry // WindowTester専用
    {
        public Entry(string name) : base(name)
        {
        }

        public Users.OrganizationMap OrganizationMap
        {
            get
            {
                if (organizationMap is null)
                {
                    organizationMap = new Users.OrganizationMap(this);
                    organizationMap.PropertyChanged += (s, e) =>
                    {
                        switch (e.PropertyName)
                        {
                            case "IsLoaded":
                                SendPropertyChanged("Chidlren");
                                break;
                        }
                    };
                    organizationMap.DMLUri = $"http://{SystemAD.SysAD.ServerAddress}/{EntryName}/OrganizationMap.DML/";
                }
                return organizationMap;
            }
        }
        public string Header => GetAttribute("Name");
        public object TabHeader => GetAttribute("Name");

        public object Icon => "⚒️";
        public object Content => new DockPanel
        {
            Children = {
                    new GridLayoutPanel{
                        ColumnCount = 2,
                        ColumnDistributions=new object[]{ "*","*" },
                        Children =
                        {
                            new TextBlock { Text = "名称", },
                            new TextBox {
                                IsReadOnly = true,
                                Bindings = {
                                    BindingTuple.Create(Property:TextBox.TextProperty,Path:"Header",Source:this,Mode:System.Windows.Data.BindingMode.OneWay )
                                }
                            },
                        },
                        Dock=System.Windows.Controls.Dock.Top
                    },
                    new HTreeView()
                    {
                        Bindings ={
                            BindingTuple.Create(Property:TreeView.ItemsSourceProperty,Path:"Chidlren",Source:this)
                        }
                    }
                }
        };
        public IEnumerable Chidlren
        {
            get
            {
                var items = new List<object> { OrganizationMap.DocumentElement };
                return items;
            }

        }

        public async Task<string> SendCommandAsync(string command, string commandParams, string data = "")
        {
            if (data.IsNullOrEmpty())
                return await DBSourceInfo.SysAD.Server.GetText($"?cmd={command}&CommandTarget=Entry&Entry={GetAttribute("Name")}{(commandParams.IsNullOrEmpty() ? "" : $"&{commandParams}")}");
            else
                return await DBSourceInfo.SysAD.Server.SendText($"?cmd={command}&CommandTarget=Entry&Entry={GetAttribute("Name")}{(commandParams.IsNullOrEmpty() ? "" : $"&{commandParams}")}", data);
        }
    }

    public partial interface IEntry : ITabPage
    {

    }
}
