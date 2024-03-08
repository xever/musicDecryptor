using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tomusic
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWin form = new Tomusic.MainWin();
            ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root.AddAppender(form);
            BasicConfigurator.Configure(form);
            ((Hierarchy)LogManager.GetRepository()).Root.Level = Level.Info;

            Application.Run(form);
        }
    }
}
