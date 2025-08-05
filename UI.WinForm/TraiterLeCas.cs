using System;
using Infrastructure.Common.Cache;
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
    public partial class TraiterLeCas : Form
    {
        public TraiterLeCas()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ValiderLaVisete frm = new ValiderLaVisete();
            AddOwnedForm(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            this.Tag = frm;
            frm.BringToFront();
            frm.Show();
        }

        private void TraiterLeCas_Load(object sender, EventArgs e)
        {
            ///Gérer les autorisations
            if (ActiveUser.c_position == Positions.secretariat)
            {

                button4.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
    }

