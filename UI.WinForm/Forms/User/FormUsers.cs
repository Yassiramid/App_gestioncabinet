using Domain.Models;
using Domain.ValueObjects;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI.WinForm
{
    public partial class FormUsers : Form
    {
        private UserModel userModel = new UserModel();
        public FormUsers()
        {           
            InitializeComponent();
             
        }       

        private void FormUsers_Load(object sender, EventArgs e)
        {
            loadUserData();
            reset();
        }

        private void loadUserData()
        {
            DataGridView1.DataSource = userModel.getAllUsers();
            DataGridView1.RowTemplate.Height = 100;//définissez une hauteur considérable pour afficher les photos de profil de l'utilisateur.
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (DataGridView1.SelectedRows.Count > 0)
            {
                Panel1.Enabled = true;
                userModel.Id = Convert.ToInt32(DataGridView1.CurrentRow.Cells[0].Value);
                userModel.State = EntityState.Removed; 
                var result = userModel.saveChanges(); 
                MessageBox.Show(result);
                loadUserData();
            }
            else
                MessageBox.Show("Selected a row");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (DataGridView1.SelectedRows.Count > 0)
            {
                Panel1.Enabled = true;
                txtUsername.Text = DataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtPassword.Text = DataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtFirstName.Text = DataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtLastName.Text = DataGridView1.CurrentRow.Cells[4].Value.ToString();
                cmbPosition.Text = DataGridView1.CurrentRow.Cells[5].Value.ToString();
                txtEmail.Text = DataGridView1.CurrentRow.Cells[6].Value.ToString();
                if (DataGridView1.CurrentRow.Cells[7].Value != null)//nous passons l'image de l'utilisateur si je l'avais, cas contraire pictureBox restera avec l'image par défaut
                {
                    editedPictureBoxProfile.Image = ConvertItem.binaryToImage((byte[])DataGridView1.CurrentRow.Cells[7].Value);
                }
                // Envoyer id à éditer
                userModel.Id =Convert.ToInt32( DataGridView1.CurrentRow.Cells[0].Value);
                userModel.State = EntityState.Edited; 
            }
            else
                MessageBox.Show("Selected a row");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Panel1.Enabled = true;
            userModel.State = EntityState.Added; 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            userModel.UserName = txtUsername.Text;
            userModel.Password = txtPassword.Text;
            userModel.FirstName = txtFirstName.Text;
            userModel.LastName = txtLastName.Text;
            userModel.Position = cmbPosition.Text;
            userModel.Email = txtEmail.Text;
            userModel.Profile = ConvertItem.imageToBinary(editedPictureBoxProfile.Image);

            var valid = new DataValidation(userModel).Validate(); // valider les données (vides, courrier, longueur minimale,etc.)
            if (valid == true)
            {
                var result = userModel.saveChanges(); //enregistrer les modifications (statut)
                loadUserData();
                MessageBox.Show(result);
                reset();
            }
        }

        private void reset()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            editedPictureBoxProfile.Image = Properties.Resources.defaultImageProfileUser;//Charger l'image par défaut à partir du dossier de ressources
            Panel1.Enabled = false;
            cmbPosition.SelectedIndex = 0;
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                DataGridView1.DataSource = userModel.getAUserByValue(txtSearch.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkChangePictureProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Images(.jpg,.png)|*.png;*.jpg";//filtrer uniquement les fichiers d'images de type
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                editedPictureBoxProfile.Image = new Bitmap(openFile.FileName);
            }
        }
    }
}
