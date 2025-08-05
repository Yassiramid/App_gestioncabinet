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
    public partial class RGLRDV : Form
    {
        BindingSource BS = new BindingSource();
        public RGLRDV()
        {
            InitializeComponent();
        }
        public void Actualiser()
        {
            dataGridView1.DataSource = Program.db.RDVs.Select(x => new { Patient = x.patient1.nomP + " " + x.patient1.prénomP, Date = x.dateR, x.heure, Motif = x.motifR }).ToList();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RGLRDV_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Program.db.patients.
             Select(x => new
             {
                 nom = x.nomP + " " + x.prénomP,
                 id = x.numP
             })
           .ToList();
            comboBox1.DisplayMember = "nom";
            comboBox1.ValueMember = "id";
            Actualiser();
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                comboBox1.Text= row.Cells["Patient"].Value.ToString();
                dateTimePicker1.Text = row.Cells["Date"].Value.ToString();
                textBox1.Text = row.Cells["heure"].Value.ToString();
                comboBox2.Text = row.Cells["Motif"].Value.ToString();

            }
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var req = (from x in Program.db.RDVs where x.patient == comboBox1.SelectedValue.ToString() select x).First();
                req.dateR = dateTimePicker1.Value;
                req.heure = textBox1.Text;
                req.motifR = comboBox2.Text;
                Program.db.SaveChanges();
                MessageBox.Show("Opération réussi");
            }

            catch(Exception)
            {
                MessageBox.Show("Opération échoue");
            }
        }

        
    }
    }
