using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Common.Cache;

namespace UI.WinForm
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ///Manage Permissions
            if (ActiveUser.c_position == Positions.Receptionist)
            {                            
                btnRemove.Enabled = false;
            }
            if (ActiveUser.c_position == Positions.Accounting)
            {
                btnRemove.Enabled = false;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
