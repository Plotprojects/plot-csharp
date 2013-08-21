using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestJSON
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(ReportError);
            Application.Run(new mainForm());
        }

        /// <summary>
        /// Easy and simple, like Delphi does it...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ReportError(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
#if DEBUG
            MessageBox.Show(e.Exception.ToString());
#else
            MessageBox.Show(e.Exception.Message);
#endif
        }
    }
}
