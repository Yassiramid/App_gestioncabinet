using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.WinForm
{
    public partial class Fichier_Medical : Form
    {
        public Fichier_Medical()
        {
            InitializeComponent();
        }
        string chemain = "";
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Choisissez votre image";
            if(of.ShowDialog()==DialogResult.OK)
            {
                pictureBox1.ImageLocation = of.FileName;
                chemain = of.FileName;
            }
        }
    }
}
