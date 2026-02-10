
namespace HIMTools.Windows
{
    using HIMTools.Controls;
    using HIMTools.Windows.Ribbons;
    using System.Windows.Markup;
    using SysCtrl = System.Windows.Controls;
    using SysWin = System.Windows;


    public partial class MainPanel : IMainPanel　//Tester用
    {
        protected void InitPanel()
        {

            Ribbons = new RibbonContainer() { Dock = SysCtrl.Dock.Top, MinHeight = 80 };
            Children.Add(Ribbons as SysWin.UIElement);
            Children.Add(QuadTabPages as SysWin.UIElement);
        }

        public IQuadTabControl QuadTabPages { get; private set; } = new QuadTabControl();
        public IRibbonContainer Ribbons { get; private set; }


        public IQuadTabOwner QuadTabOwner
        {
            get => QuadTabPages.QuadTabOwner;
            set => QuadTabPages.QuadTabOwner = value;
        }
        public static readonly SysWin.DependencyProperty QuadTabOwnerProperty = SysWin.DependencyProperty.Register("QuadTabOwner", typeof(IQuadTabOwner), typeof(MainPanel), new SysWin.PropertyMetadata(null, OnQuadTabOwnerChanged));
        private static void OnQuadTabOwnerChanged(SysWin.DependencyObject d, SysWin.DependencyPropertyChangedEventArgs e)
        {
            var control = d as MainPanel;
            control.QuadTabOwner = e.NewValue as IQuadTabOwner;
        }
    }

    public partial interface IMainPanel
    {
        IRibbonContainer Ribbons { get; }
        IQuadTabControl QuadTabPages { get; }
    }
}
