using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace UtilZ.Dotnet.SHPAutoPatch
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        // public string[] Args { get; private set; }

        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //this.Args = e.Args;
            base.OnStartup(e);
            var upgradeHelper = new UpgradeHelper();
            upgradeHelper.ExcuteUpgrade(e.Args);
            upgradeHelper.WriteUpgradeLog();
            App.Current.Shutdown();
        }
    }
}
