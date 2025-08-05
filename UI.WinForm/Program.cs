using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.WinForm
{
    static class Program
    {
       public static ProjetFinEtudeEntities db = new ProjetFinEtudeEntities();
       
        public static Form mainForm;
        [STAThread]        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm=new FormLogin());
        }
    }
}
