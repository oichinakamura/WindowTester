using HIMTools.Controls;
using HIMTools.Interface;
using HIMTools.SharedObjects;
using HIMTools.Xml;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace HIMTools.Users
{
    using SysCtrl = System.Windows.Controls;
    public partial class OrganizationMap // WindowTester専用
    {
        public OrganizationMap(DBManSrv.IEntry entry)
        {
            parentEntry = entry;
            LoadOrganizationMap();
            CommandProxyHandler = new OrganizationMapCommandProxy(this);
        }

        protected async void LoadOrganizationMap()
        {
            var response = await SendCommand("DownLoadXML", $"DateTime={DateTime.Now}");
            {
                LoadXml(response);
                this.Changed += (s, e) =>
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    SendPropertyChanged();
                };
                SendPropertyChanged();
                AsyncLoaded();
                //if (!ParentEntry.HasOrganizationMap)
                //    ParentEntry.HasOrganizationMap = true;

                IsLoaded = true;
                SendPropertyChanged("IsLoaded");
            }
        }

        public XeElement CurrentUser { get; set; }

        public OrganizationGroup CurrentGroup { get; set; }

        public void Execute(object param)
        {

        }

        public IMetaElement CreateElementSub(XName name)
        {
            return base.CreateElement(name);
        }


    }

    public partial class OrganizationDirectory // WindowTester専用
    {
        protected OrganizationDirectory(XName name) : base(name)
        {
        }

        public virtual IEnumerable Children => null;
        public virtual void OnDragOver(object sender, DragEventArgs e) { }
        public virtual void OnDrop(object sender, DragEventArgs e) { }
        public virtual void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e) { }
    }

    public partial class OrganizationGroupRoot  // WindowTester専用
    {
        public override object Header => $"OrganizationMap:{Name.LocalName}";
        public override IEnumerable Children => ChildNodes;

        public override void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _ = new ContextMenu()
            {
                Items =
                {
                    new SysCtrl.MenuItem{ Header="共有図形リストの追加",Command=OrganizationMap.CommandProxyHandler,CommandParameter=new CommandParameterArgs(this,"共有図形リストの追加") },
                },
                IsOpen = true,
            };
        }
    }

    public partial class OrganizationGroup  // WindowTester専用
    {
        public OrganizationGroup() : base("Group")
        {
        }
        public override object Header => $"Group:{GetAttribute("GroupName")}";
        public override IEnumerable Children => ChildNodes;

        public override void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _ = new ContextMenu()
            {
                Items =
                {
                    new SysCtrl.MenuItem{ Header="共有図形リストの追加",Command=OrganizationMap.CommandProxyHandler,CommandParameter = new CommandParameterArgs(this,"共有図形リストの追加") },
                },
                IsOpen = true,
            };
        }
    }

    public partial class OrganizationUser // WindowTester専用
    {
        public override object Header => $"User:{GetAttribute("Name")}{(GetAttribute("UserName") is string userName ? (userName.IsNullOrEmpty() ? "" : $"({userName})") : "")}";
    }
    public partial class OrganizationUsableApplication // WindowTester専用
    {
        public override object Header => $"Application:{GetAttribute("ModelID")}";
    }

    public partial class OrganizationComplexGroup // WindowTester専用
    {
        public override object Header => $"共有グループ:{GetAttribute("Title")}";
    }

    public partial class OrganizationImportTable // WindowTester専用
    {
        public override object Header => $"共有テーブル:{GetAttribute("Title")}";
    }

    public partial class OrganizationSharedShapeOld // WindowTester専用
    {
        public override object Header => $"旧共有図形:{GetAttribute("Name")}";
    }

    public partial class OrganizationSharedShape : INotifyPropertySet // WindowTester専用
    {
        public bool IsActive
        {
            get => sharedCommonShapes != null  && SystemAD.SysAD.QuadTabPages.Contains(sharedCommonShapes);
            set
            {
                if (value)
                {
                    if (sharedCommonShapes is null)
                        LoadAsync(value);
                    else if(!SystemAD.SysAD.QuadTabPages.Contains(sharedCommonShapes))
                        SystemAD.SysAD.QuadTabPages.R1.Add(sharedCommonShapes);
                }
                else if (sharedCommonShapes != null)
                    SystemAD.SysAD.QuadTabPages.Remove(sharedCommonShapes);

                SendPropertyChanged(nameof(IsActive), nameof(IsChecked));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SendPropertyChanged(params string[] propertyNames) { foreach (string propertyName in propertyNames) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        private async void LoadAsync(bool ShowList)
        {
            sharedCommonShapes = await SharedCommonShapes.Load(this, GetAttribute("Path"));
            if (sharedCommonShapes != null)
            {
                if (ShowList)
                {
                    SystemAD.SysAD.QuadTabPages.R1.Add(sharedCommonShapes);
                }
            }
        }
    }

    public partial class OrganizationCustomerConfig // WindowTester専用
    {
        public override object Header => $"カスタマーコンフィグ";
    }

    public partial class OrganizationMapCommandProxy : CommandProxy　// WindowTester専用
    {
        public OrganizationMapCommandProxy(OrganizationMap organizationMap)
        {
            OrganizationMap = organizationMap;
        }
        protected OrganizationMap OrganizationMap;


        public override bool CanExecute(object owner, object[] parameter)
        {
            return true;
        }

        public async override void Execute(object owner, params object[] Params)
        {
            switch (Params[0])
            {
                case "共有図形リストの追加":
                    switch (owner)
                    {
                        case OrganizationGroup group:
                            if (SystemAD.SysAD.CurrentEntry != null)
                                if (Dialog.InputBox.ShowDialog("共有図形リストの名称", "共有図形リストの追加", out string result))
                                {
                                    if (result.Contains("\\"))
                                        await SharedObjects.SharedCommonShapes.Create(group, result.Substring(result.LastIndexOf('\\') + 1), result.Substring(0, result.LastIndexOf('\\')));
                                    else
                                        await SharedObjects.SharedCommonShapes.Create(group, result, "");
                                    group.SendPropertyChanged("SharedItems");
                                    //    TreeUpdate(SystemAD.SysAD.CurrentEntry.OrganizationMap.CurrentGroup);
                                }
                            break;
                    }
                    break;
                default:
                    ExecuteSub(owner, Params);
                    break;

            }
        }

    }
}
