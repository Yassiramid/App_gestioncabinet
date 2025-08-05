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
            DataGridView1.RowTemplate.Height = 100;//establecer una altura considerable para visualizar las fotos de perfil del usuario.
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (DataGridView1.SelectedRows.Count > 0)
            {
                Panel1.Enabled = true;
                userModel.Id = Convert.ToInt32(DataGridView1.CurrentRow.Cells[0].Value);
                userModel.State = EntityState.Removed; // cambiar estado a eliminar
                var result = userModel.saveChanges(); // guardios cambios (estado)
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
                if (DataGridView1.CurrentRow.Cells[7].Value != null)//pasamos imagen de usuario si lo tiviese, caso contrario pictureBox permanecerá con la imagen por defecto
                {
                    editedPictureBoxProfile.Image = ConvertItem.binaryToImage((byte[])DataGridView1.CurrentRow.Cells[7].Value);
                }
                // Enviar id a editar
                userModel.Id =Convert.ToInt32( DataGridView1.CurrentRow.Cells[0].Value);
                userModel.State = EntityState.Edited; // cambiar estado a editar
            }
            else
                MessageBox.Show("Selected a row");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Panel1.Enabled = true;
            userModel.State = EntityState.Added; // cambiar estado a agregar
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

            var valid = new DataValidation(userModel).Validate(); // validar datos(vacios, correo, longitud minino, solo letras, etc)
            if (valid == true)
            {
                var result = userModel.saveChanges(); // guardar cambios (estado)
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
            editedPictureBoxProfile.Image = Properties.Resources.defaultImageProfileUser;//Cargar imagen por defecto desde la carpeta de recursos
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
            openFile.Filter = "Images(.jpg,.png)|*.png;*.jpg";//filtrar archivos  solo de tipo imagenes
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                editedPictureBoxProfile.Image = new Bitmap(openFile.FileName);
            }
        }
    }
}
