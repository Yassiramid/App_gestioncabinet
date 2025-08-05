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
    public partial class Nov_Patient : Form
    {
       
        public Nov_Patient()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void vider()
        {
            txtNum.Text = txtNomP.Text = txtPrenom.Text = cmbSexe.Text = txtAge.Text = txtTel.Text = txtAdresse.Text = cmbSF.Text = cmbSF.Text = DTdateC.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try {
                patient p = new patient();
                p.numP = txtNum.Text;
                p.nomP = txtNomP.Text;
                p.prénomP = txtPrenom.Text;
                p.sexe = cmbSexe.SelectedItem.ToString();
                p.age = int.Parse(txtAge.Text);
                p.télèphone = txtTel.Text;
                p.adresse = txtAdresse.Text;
                p.dateCreation = DTdateC.Value;
                p.mutuelle = cmbMut.SelectedItem.ToString();
                p.situationF = cmbSF.SelectedItem.ToString();
                Program.db.patients.Add(p);
                Program.db.SaveChanges();
                MessageBox.Show("ajout réussi");
                vider();
            }
            catch(Exception)
            {
                MessageBox.Show("Certaines Informations sont Incorrectes ");
            }
            

        }
    }
}
