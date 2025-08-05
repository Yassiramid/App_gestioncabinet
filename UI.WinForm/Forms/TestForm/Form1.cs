using Infrastructure.Common.Cache;
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
    public partial class Form1 : Form
    {
        //BindingSource BS = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        public void Actualiser()
        {
            dataGridView1.DataSource = Program.db.patients.Select(p => new { NumPatient = p.numP, Nom = p.nomP, Prénom = p.prénomP, Sexe = p.sexe, Age = p.age, DateCréation = p.dateCreation, Téléphone = p.télèphone, SituationFamiliale = p.situationF, Mutuelle = p.mutuelle, Adresse = p.adresse }).ToList();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ///Gérer les autorisations
            if (ActiveUser.c_position == Positions.secretariat) {

                //btnRemove.Enabled = false;
            }
            if (ActiveUser.c_position == Positions.comptable) {
                //btnRemove.Enabled = false;
                //btnAdd.Enabled = false;
                //btnEdit.Enabled = false;
            }
            //BS.DataSource = Program.db.patient.ToList();
            //dataGridView1.DataSource = BS;
            //txtNum.DataBindings.Add("Text", BS, "numP");
            //txtNom.DataBindings.Add("Text", BS, "nomP");
            //txtPrénom.DataBindings.Add("Text", BS, "prénomP");
            //txtSexe.DataBindings.Add("Text", BS, "sexe");
            //dateTimePicker1.DataBindings.Add("Text", BS, "dateCreation");
            //txtTél.DataBindings.Add("Text", BS, "télèphone");
            //txtST.DataBindings.Add("Text", BS, "situationF");
            //txtM.DataBindings.Add("Text", BS, "mutuelle");
            //txtAdresse.DataBindings.Add("Text", BS, "adresse");
            //txtAge.DataBindings.Add("Text", BS, "age");

            Actualiser();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
 


        private void btnAdd_Click(object sender, EventArgs e)
        {
            Nov_Patient frm = new Nov_Patient();
            AddOwnedForm(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            this.Tag = frm;
            frm.BringToFront();
            frm.Show();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Ajouter_RDV frm = new Ajouter_RDV();
            AddOwnedForm(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            this.Tag = frm;
            frm.BringToFront();
            frm.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
               
                {
                    patient p = Program.db.patients.Where(pa => pa.numP == txtNum.Text).First();
                    p.nomP = txtNom.Text;
                    p.prénomP = txtPrénom.Text;
                    p.sexe = txtSexe.Text;
                    p.age = int.Parse(txtAge.Text);
                    p.dateCreation = dateTimePicker1.Value;
                    p.télèphone = txtTél.Text;
                    p.situationF = txtST.Text;
                    p.mutuelle = txtM.Text;
                    p.adresse = txtAdresse.Text;
                    Program.db.SaveChanges();
                    MessageBox.Show("patient Modifier avec Succes");
                    Actualiser();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Patient n'existe pas");
            }
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try { 
            var req = (from x in Program.db.patients where x.numP == textBox1.Text  select x ).FirstOrDefault();
            if (req != null)
            {
                txtNum.Text = req.numP;
                txtNom.Text = req.nomP;
                txtPrénom.Text = req.prénomP;
                txtSexe.Text = req.sexe;
                txtAge.Text = req.age.ToString();
                dateTimePicker1.Value = req.dateCreation.Value;
                txtTél.Text = req.télèphone;
                txtST.Text = req.situationF;
                txtM.Text = req.mutuelle;
                txtAdresse.Text = req.adresse;
            }
            }
            catch(Exception)
            {
                MessageBox.Show("Patient n'existe pas");
            }











        }

        private void button3_Click(object sender, EventArgs e)
        {
            les_Antécedants frm = new les_Antécedants();
            AddOwnedForm(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            this.Tag = frm;
            frm.BringToFront();
            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Fichier_Medical frm = new Fichier_Medical();
            AddOwnedForm(frm);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.Controls.Add(frm);
            this.Tag = frm;
            frm.BringToFront();
            frm.Show();
        }

       

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                txtNum.Text = row.Cells["NumPatient"].Value.ToString();
                txtNom.Text = row.Cells["Nom"].Value.ToString();
                txtPrénom.Text = row.Cells["Prénom"].Value.ToString();
                txtSexe.Text = row.Cells["Sexe"].Value.ToString();
                txtAge.Text = row.Cells["Age"].Value.ToString();
                dateTimePicker1.Text = row.Cells["DateCréation"].Value.ToString();
                txtTél.Text = row.Cells["Téléphone"].Value.ToString();
                txtST.Text = row.Cells["SituationFamiliale"].Value.ToString();
                txtM.Text = row.Cells["Mutuelle"].Value.ToString();
                txtAdresse.Text = row.Cells["Adresse"].Value.ToString();
            }
        }
    }
}

     