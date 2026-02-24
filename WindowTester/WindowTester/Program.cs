using System;

namespace HIMTools
{
    using HIMTools.AppSystem;
    public static class Program
    {
        public static CSystemAD SysAD { get => SystemAD.SysAD; set => SystemAD.SysAD = value; }

        [STAThread]
        public static void Main()
        {
            SysAD = new CSystemAD();

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }

    public static class SystemAD
    {
        public static CSystemAD SysAD { get; set; }

    }
}
