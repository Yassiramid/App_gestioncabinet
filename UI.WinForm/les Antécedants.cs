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
    public partial class les_Antécedants : Form
    {
        public les_Antécedants()
        {
            InitializeComponent();
        }

        private void les_Antécedants_Load(object sender, EventArgs e)
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
        }

     
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Antécedant ant = new Antécedant();
                ant.nomA = textBox1.Text;
                ant.dateD = dateTimePicker1.Value;
                ant.patient = comboBox1.SelectedValue.ToString();
                ant.Observation = textBox2.Text;
                if (radioButton2.Checked)
                    ant.traiter = radioButton1.Text;
                else
                    ant.traiter = radioButton2.Text;
                Program.db.Antécedant.Add(ant);
                Program.db.SaveChanges();
                MessageBox.Show("ajout réussi");
            }
            catch(Exception)
            {
                MessageBox.Show("Opération échoue");
            }
           

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
