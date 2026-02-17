namespace HIMTools.AppSystem
{
    using HIMTools.Applications;
    using HIMTools.Controls;
    using HIMTools.Interface;
    using HIMTools.MapTools;
    using HIMTools.SharedObjects;
    using System;
    using System.ComponentModel;

    public class SystemAD : SystemADSK, IQuadTabOwner, INotifyPropertySet
    {
        private QuadTabPageCollection quadTabPages = new QuadTabPageCollection();
        public QuadTabPageCollection QuadTabPages => quadTabPages;
        public SystemAD()
        {
            if (System.IO.File.Exists(FilePath))
                Load(FilePath);
            else
            {
                AppendChild(CreateElement("SystemConfig"));
                Save(FilePath);
            }
            Console.WriteLine(OuterFormatedXml);
            MapViewer = quadTabPages.L1.Add<MapViewer>(new MapViewer(), true);
            quadTabPages.L1.Add(new TabDumy());

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
        //public ITabPage SelectedL1 { get => quadTabPages.L1.SelectedTab; set { quadTabPages.L1.SelectedTab = value; SendPropertyChanged(nameof(SelectedL1)); } }

        public TabPageCollection R1 => quadTabPages.R1;
        //public ITabPage SelectedR1 { get => quadTabPages.R1.SelectedTab; set { quadTabPages.R1.SelectedTab = value; SendPropertyChanged(nameof(SelectedR1)); } }

        public void SendPropertyChanged(params string[] propertyNames) { foreach (string propertyName in propertyNames) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class TabDumy : ITabPage
    {
        public object Header => "Dumy";
        public object Icon => "�";
        public object Content => null;

        public Guid ID { get; } = Guid.NewGuid();

        public object TabHeader => Header;
    }
}
