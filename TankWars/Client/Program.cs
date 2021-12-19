using System;
using System.Windows.Forms;

namespace TankWars
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
            GameController c = new GameController();
            TankGameWindow theForm = new TankGameWindow(c);
            Application.Run(theForm);
        }
    }
}
