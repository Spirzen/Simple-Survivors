using System;
using System.Windows.Forms;
namespace Simple_Survivors
{
    /// <summary>
    /// Основной класс для запуска.
    /// </summary>
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			ApplicationConfiguration.Initialize();
            Application.Run(new GameForm());
        }
    }
}