using System;
using System.Windows;

namespace HIMTools
{
    public class Program
    {
        [STAThread] // WPFアプリケーションには必須
        public static void Main()
        {
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
