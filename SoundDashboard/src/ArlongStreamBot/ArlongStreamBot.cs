using System;
using System.Windows.Forms;
using log4net;

namespace ArlongStreambot
{
    static class ArlongStreamBot
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ArlongStreamBot));

        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new MainFormView());
        }
    }
}