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
    public partial class Ajouter_RDV : Form
    {
        public Ajouter_RDV()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            RDV R = new RDV();
            R.patient = comboBox1.SelectedValue.ToString();
            R.dateR = dateTimePicker1.Value;
            R.heure = textBox1.Text;
            R.motifR = comboBox2.SelectedItem.ToString();
            Program.db.RDVs.Add(R);
            Program.db.SaveChanges();

            MessageBox.Show("Opperation Réussi");
        }

        private void Ajouter_RDV_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Program.db.patients.
              Select(x => new
              {
                  nom = x.nomP+" "+x.prénomP,
                  id=x.numP
              })
              .ToList();
            comboBox1.DisplayMember = "nom";
            comboBox1.ValueMember = "id";

        }

      
    }
}
