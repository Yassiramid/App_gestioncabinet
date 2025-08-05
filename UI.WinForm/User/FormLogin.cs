using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Domain.Models;
using Infrastructure.Common.Cache;

namespace UI.WinForm
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }
        #region Form Methods
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);


        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtuser.Text == "Username or email")
            {
                txtuser.Text = "";
                txtuser.ForeColor = Color.WhiteSmoke;
            }
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (txtuser.Text == "")
            {
                txtuser.Text = "Username or email";
                txtuser.ForeColor = Color.Silver;
            }
        }

        private void txtpass_Enter(object sender, EventArgs e)
        {
            if (txtpass.Text == "Password")
            {
                txtpass.Text = "";
                txtpass.ForeColor = Color.WhiteSmoke;
                txtpass.UseSystemPasswordChar = true;
            }
        }

        private void txtpass_Leave(object sender, EventArgs e)
        {
            if (txtpass.Text == "")
            {
                txtpass.Text = "Password";
                txtpass.ForeColor = Color.Silver;
                txtpass.UseSystemPasswordChar = false;
            }
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            login();
        }
        private void txtpass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void login()
        {
            if (txtuser.Text != "Username or email" && txtuser.TextLength > 2)
            {
                if (txtpass.Text != "Password")
                {
                    UserModel user = new UserModel();
                    var validLogin = user.logIn(txtuser.Text, txtpass.Text);
                    if (validLogin == true)
                    {
                        this.Hide();
                        FormMainMenu mainMenu = new FormMainMenu();
                        FormWelcome welcome = new FormWelcome();
                        welcome.ShowDialog();
                        mainMenu.Show();
                        mainMenu.FormClosed += Logout;

                    }
                    else {
                        msgError("Incorrect username or password entered. \n   Please try again.");
                        txtpass.Text = "Password";
                        txtpass.UseSystemPasswordChar = false;
                        txtuser.Focus();
                    }
                }
                else msgError("Please enter password.");
            }
            else msgError("Please enter username or email.");
        }
        private void msgError(string msg)
        {
            lblErrorMessage.Text = "    " + msg;
            lblErrorMessage.Visible = true;
        }
        private void Logout(object sender, FormClosedEventArgs e)
        {
            //reiniciar formulario login
            txtpass.Text = "Password";
            txtpass.UseSystemPasswordChar = false;
            txtuser.Text = "Username or email";
            lblErrorMessage.Visible = false;
            //limpiar cache de usuario
            ActiveUser.c_id = 0;
            ActiveUser.c_userName = null;
            ActiveUser.c_password = null;
            ActiveUser.c_firstName = null;
            ActiveUser.c_lastName = null;
            ActiveUser.c_position = null;
            ActiveUser.c_email = null;
            ActiveUser.c_profile = null;
            //mostrar formulario
            this.Show();
        }

        private void linkpass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var recoverPassword = new FormRecoverPassword();
            recoverPassword.ShowDialog();
        }


    }
}
