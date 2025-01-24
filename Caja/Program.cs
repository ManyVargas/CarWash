using System;
using System.Windows.Forms;

namespace Caja
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // Inicializa configuraciones modernas de Windows Forms
            System.Windows.Forms.Application.Run(new Form1()); // Cambia "CajaForm" por el nombre correcto de tu formulario principal
        }
    }
}
