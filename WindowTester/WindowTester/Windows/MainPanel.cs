
namespace HIMTools.Windows
{
    using HIMTools.Controls;
    using HIMTools.Windows.Ribbons;
    using HIMTools.Windows.TabSystem;
    using System.Windows;
    using SysCtrl = System.Windows.Controls;

    public class MainPanel : DockPanel, IMainPanel
    {
        public MainPanel()
        {
            QuadTabPages = new QuadTabControl();
            Ribbons = new RibbonContainer() { Dock = SysCtrl.Dock.Top, MinHeight = 80 };
            Children.Add(Ribbons as UIElement);
            Children.Add(QuadTabPages as UIElement);
        }

        public IQuadTabControl QuadTabPages { get; }
        public IRibbonContainer Ribbons { get; }
    }

    public interface IMainPanel
    {
        IRibbonContainer Ribbons { get; }
        IQuadTabControl QuadTabPages { get; }
    }
}
