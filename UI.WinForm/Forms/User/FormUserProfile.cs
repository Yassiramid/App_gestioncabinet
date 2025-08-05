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
        { //nous chargeons les données sur les textbox
          
            lblLastName.Text = ActiveUser.c_lastName;
            lblMail.Text = ActiveUser.c_email;
            lblUser.Text = ActiveUser.c_userName;
            lblName.Text = ActiveUser.c_firstName;
            lblPosition.Text = ActiveUser.c_position;
            if (ActiveUser.c_profile != null) pictureBoxProfile.Image = ConvertItem.binaryToImage(ActiveUser.c_profile);
            //Modifier vue
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

        private void reset() //tout réinitialiser
        {
            Panel1.Visible = false;
            initializeEditControls();
            loadUserData(); //Nous invoquons à nouveau les méthodes de mise à jour des données à l'écran (dans le formulaire), avec les nouvelles données utilisateur, ou les données existantes, si elles ne sont pas mises à jour.
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
                //Nous commençons uniquement par modifier le mot de passe
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
            if ((txtPassword.Text == txtConfirmPass.Text))//Validez que les mots de passe correspondent
            {
                UserModel userModel = new UserModel();// Create the user object

                userModel.Id = ActiveUser.c_id;
                userModel.UserName = txtUsername.Text;
                userModel.Password = txtPassword.Text;
                userModel.FirstName = txtFirstName.Text;
                userModel.LastName = txtLastName.Text;
                userModel.Position = ActiveUser.c_position; //nous envoyons la même valeur de cache, les utilisateurs ne sont pas autorisés à modifier leur position
                userModel.Email = txtEmail.Text;
                userModel.Profile = ConvertItem.imageToBinary(editedPictureBoxProfile.Image);
                userModel.State = EntityState.Edited;//nous changeons l'état pour éditer

                userModel.editMyUserProfile = true;//Nous indiquons que si l'utilisateur connecté est celui qui modifie son profil

                var validateData = new DataValidation(userModel).Validate(); // Valider tous les champs

                if ((validateData == true))
                {
                    var formValidpass = new FormCustomPopup();
                    // valider le mot de passe actuel
                    formValidpass.ShowDialog();
                    if ((formValidpass.correct == true))
                    {
                        var result = userModel.saveChanges();
                        reset();// tout réinitialiser
                        MessageBox.Show(result);                        
                    }
                }
            }
            else {
                MessageBox.Show("the passwords do not match, do you want to try again?");
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
            openFile.Filter = "Images(.jpg,.png)|*.png;*.jpg";//filtrer uniquement les types de fichiers d'images.
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                editedPictureBoxProfile.Image = new Bitmap(openFile.FileName);
            }
        }
    }
}
