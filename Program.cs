using System;
using System.Windows.Forms;

namespace WindowsTweaker
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для суверенного оптимизатора WindowsTweaker.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Нативный, прямой запуск главного ядра формы твикера
            Application.Run(new TweakerForm());
        }
    }
}
