using System;
using System.Windows.Forms;
using Infrastructure.Common.Cache;
using Domain.Models;
using Domain.ValueObjects;
using System.Drawing;

namespace UI.WinForm
{
    public partial class FormUserProfile : Form
    {
        public FormUserProfile()
        {
            InitializeComponent();          
        }

        private void FormUserProfile_Load(object sender, EventArgs e)
        {
            initializeEditControls();
            loadUserData();                         
        }

        private void loadUserData()
        { //Cargamos los datos a las etiquetas y textboxs// We Load the data to the labels And textboxs
          //Profile view. Vista de perfil
            lblLastName.Text = ActiveUser.c_lastName;
            lblMail.Text = ActiveUser.c_email;
            lblUser.Text = ActiveUser.c_userName;
            lblName.Text = ActiveUser.c_firstName;
            lblPosition.Text = ActiveUser.c_position;
            if (ActiveUser.c_profile != null) pictureBoxProfile.Image = ConvertItem.binaryToImage(ActiveUser.c_profile);
            //Edit view. vista Editar
            txtEmail.Text = ActiveUser.c_email;
            txtFirstName.Text = ActiveUser.c_firstName;
            txtLastName.Text = ActiveUser.c_lastName;
            txtPassword.Text = ActiveUser.c_password;
            txtConfirmPass.Text = ActiveUser.c_password;
            txtUsername.Text = ActiveUser.c_userName;
            if (ActiveUser.c_profile != null) editedPictureBoxProfile.Image = ConvertItem.binaryToImage(ActiveUser.c_profile);

        }

        private void initializeEditControls()
        {
            
            LinkEditPass.Text = "Edit";
            lblConfimPass.Visible = false;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Enabled = false;
            txtConfirmPass.UseSystemPasswordChar = true;
            txtConfirmPass.Visible = false;
        }

        private void reset() //reiniciamos todo // all reset
        {
            Panel1.Visible = false;
            initializeEditControls();
            loadUserData(); //volvemos a invocar los metodo para actualizar los datos en la pantalla (en el formulario), con los nuevos datos del usuario, o los datos existentes, si no actualizó.
        }

        private void linkEditProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {            
            Panel1.Visible = true;                      
        }

        private void LinkEditPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (LinkEditPass.Text == "Edit")
            {
                LinkEditPass.Text = "Cancel";
                lblConfimPass.Visible = true;
                txtPassword.Enabled = true;
                txtPassword.Text = "";
                txtConfirmPass.Text = "";
                txtConfirmPass.Visible = true;
            }
            else if (LinkEditPass.Text == "Cancel")
            {
                //reiniciamos solo en editar contraseña // We restart only in edit password
                initializeEditControls();
                txtPassword.Text = ActiveUser.c_password;
                txtConfirmPass.Text = ActiveUser.c_password;               
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((txtPassword.Text == txtConfirmPass.Text))// Validar que las contraseñas coincidan
            {
                UserModel userModel = new UserModel();// Crear el objeto usuario

                userModel.Id = ActiveUser.c_id;
                userModel.UserName = txtUsername.Text;
                userModel.Password = txtPassword.Text;
                userModel.FirstName = txtFirstName.Text;
                userModel.LastName = txtLastName.Text;
                userModel.Position = ActiveUser.c_position; //enviamos el mismo valor del cache, los usuarios no tienen permitido editar su cargo
                userModel.Email = txtEmail.Text;
                userModel.Profile = ConvertItem.imageToBinary(editedPictureBoxProfile.Image);
                userModel.State = EntityState.Edited;//cambiamos el estado a editar

                userModel.editMyUserProfile = true;//indicamos que si el usuario logueado es quien edita su perfil

                var validateData = new DataValidation(userModel).Validate(); // Validar todos los campos

                if ((validateData == true))
                {
                    var formValidpass = new FormCustomPopup();
                    // validar contraseña actual
                    formValidpass.ShowDialog();
                    if ((formValidpass.correct == true))
                    {
                        var result = userModel.saveChanges();
                        reset();// reiniciamos todo // all reset
                        MessageBox.Show(result);                        
                    }
                }
            }
            else {
                MessageBox.Show(("the passwords do not match, do you want to try again?" + ("\r\n" + "Las contrase�as no coinciden, �quieres intentarlo de nuevo?")));
                txtPassword.Focus();
            }
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
