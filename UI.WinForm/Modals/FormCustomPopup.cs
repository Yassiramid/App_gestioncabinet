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
    public partial class FormCustomPopup : Form
    {
        public bool correct = false;

        public FormCustomPopup()
        {
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (txtPass.Text == ActiveUser.c_password) {
                correct = true;
                this.Close();
            }
            else
            {
                correct = false;
                lblResult.Text = "Your current password you entered is incorrect, try again\n"+
                    "Su contraseña actual que ingresó es incorrecta, intente nuevamente";
                lblResult.Visible = true;
            }
        }

        private void FormCustomPopup_Load(object sender, EventArgs e)
        {
            lblResult.Visible = false;
        }
    }
}
