namespace HIMTools.AppSystem
{
    using HIMTools.Applications;
    using HIMTools.Controls;
    using HIMTools.DBManSrv;
    using HIMTools.Interface;
    using HIMTools.MapTools;
    using HIMTools.SharedObjects;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using SysWin = System.Windows;

    public class CSystemAD : SystemADSK, ISystemAD, IQuadTabOwner, INotifyPropertySet
    {
        private QuadTabPageCollection quadTabPages = new QuadTabPageCollection();
        public QuadTabPageCollection QuadTabPages => quadTabPages;
        public CSystemAD()
        {
            if (System.IO.File.Exists(FilePath))
                Load(FilePath);
            else
            {
                AppendChild(CreateElement("SystemConfig"));
                DocumentElement.SetAttribute("IPAddress", "127.0.0.1");
                DocumentElement.SetAttribute("Port", "9002");
                Save(FilePath);
            }

            Console.WriteLine(OuterFormatedXml);
            MapViewer = quadTabPages.L1.Add<MapViewer>(new MapViewer(), true);
            quadTabPages.L1.Add(new TabDummy());

            quadTabPages.R1.Add<ITabPage>(DocumentElement as ITabPage, true);

            quadTabPages.PropertyChanged += (s, e) =>
            {
                SendPropertyChanged(e.PropertyName);
            };

            Applications = new ApplicationCollection();
            Tool001 = new ApplicationTOOL001();
            Applications.Add("TOOL001", Tool001);
        }
        public IApplicationCollection Applications { get; }
        public ApplicationTOOL001 Tool001 { get; }
        public IQuadTabOwner QuadOwner => this;
        public IMapViewer MapViewer { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public TabPageCollection L1 => quadTabPages.L1;
        public TabPageCollection R1 => quadTabPages.R1;

        private DBSourceInfo dbSourceInfo;
        public DBSourceInfo DBSourceInfo
        {
            get
            {
                if (dbSourceInfo is null)
                {
                    dbSourceInfo = new DBSourceInfo(SystemAD.SysAD);
                    dbSourceInfo.Load();
                    SendPropertyChanged(nameof(DBSourceInfo), nameof(Entries));
                }

                return dbSourceInfo;
            }
        }
        public IEnumerable Entries => DBSourceInfo.SelectNodes("//Entry");

        public DBManServer Server { get; } = new DBManSrv.DBManServer();
        public IEntry CurrentEntry { get; set; }

        public void SendPropertyChanged(params string[] propertyNames) { foreach (string propertyName in propertyNames) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class TabDummy : ITabPage
    {
        public object Header => "DBSourceInfo";
        public object Icon => "�";
        public object Content => new DockPanel()
                    {
                        Children ={
                            new RichTextBox() {
                                Bindings = {
                                    BindingTuple.Create(Property: RichTextBox.TextProperty, Path:"Source",Source:this, Mode:SysWin.Data.BindingMode.OneWay)
                                },
                                HorizontalScrollBarVisibility = SysWin.Controls.ScrollBarVisibility.Visible,
                                VerticalScrollBarVisibility = SysWin.Controls.ScrollBarVisibility.Visible,
                            }
                        }
                    };

        public Guid ID { get; } = Guid.NewGuid();

        public object TabHeader => Header;
        public string Source => SystemAD.SysAD.DBSourceInfo.OuterFormatedXml;

        public void Actived()
        {
            throw new NotImplementedException();
        }
    }
}
