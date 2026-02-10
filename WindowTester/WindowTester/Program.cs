using System;

namespace HIMTools
{
    using HIMTools.AppSystem;
    public static class Program
    {
        public static SystemAD SysAD { get; private set; }

        [STAThread]
        public static void Main()
        {
            SysAD = new SystemAD();

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
