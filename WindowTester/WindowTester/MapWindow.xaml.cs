using System.Windows;

namespace HIMTools
{
    /// <summary>
    /// MapWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow()
        {
            InitializeComponent();

            this.DataContext = Program.SysAD;
        }
    }
}
