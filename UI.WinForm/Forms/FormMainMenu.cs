using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain;
using Infrastructure.Common.Cache;
using Domain.Models;
using System.IO;

namespace UI.WinForm
{
    public partial class FormMainMenu : Form
    {


        public FormMainMenu()
        {
            InitializeComponent();
        }
        #region "FONCTIONNALITÉS DU FORMULAIRE"
        // REDIMENSIONNER LE FORMULAIRE - CHANGER LA TAILLE-----------------------------------------------
        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------EXCLUDE CORNER PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));

            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);

            region.Exclude(sizeGripRectangle);
            this.panelContenedor.Region = region;
            this.Invalidate();
        }
        //----------------COULEUR ET ADHÉRENCE DU RECTANGLE/CORNER //COLOR AND GRIP OF LOWER RECTANGLE/CORNER
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(244, 244, 244));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);

            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }


        //MIN, MAX, RESTAURER LE FORMULAIRE
        int lx, ly;
        int sw, sh;
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnMaximizar.Visible = true;
            btnRestaurar.Visible = false;
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
        }
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        ///DRAG / MOVE THE FORM
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelBarraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        #endregion
        #region OPEN AND CLOSE FORM METHODS____MÉTHODES DE FORMES OUVERTES ET FERMÉES
        //MÉTHODES DE FORMES OUVERTES ET FERMÉES//METHOD OF OPENING FORM IN THE CONTAINER PANEL

        private void openFormOnPanel<MiForm>() where MiForm : Form, new()
        {
          
            Form formulario;
            formulario = panelformularios.Controls.OfType<MiForm>().FirstOrDefault();//Recherchez le formulaire dans la collection.
            //si le formulaire / l'instance n'existe pas        
            if (formulario == null)
            {
                //Fermer le formulaire précédent, à l'exception du formulaire principal et de la connexion au formulaire / Si vous souhaitez autoriser l'ouverture de plusieurs formulaires dans le panneau, supprimez simplement le code
                Application.OpenForms.Cast<Form>().Except(new Form[] { this, Program.mainForm }).ToList().ForEach(x => x.Close());
                //'Ouvrir le formulaire dans le panneau

                formulario = new MiForm();
                formulario.SuspendLayout();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;                
                panelformularios.Controls.Add(formulario);
                panelformularios.Tag = formulario;
                formulario.Show();                
                formulario.BringToFront();
                formulario.FormClosed += new FormClosedEventHandler(closedForm);
                formulario.ResumeLayout();           
            }

            else {
                //si le formulaire / instance existe, nous le mettons au premier plan
                formulario.BringToFront();
            }
        }

        //MÉTHODE/ÉVÉNEMENT LORS DE LA CLÔTURE DES FORMULAIRES//METHOD / EVENT WHEN CLOSING FORMS
        private void closedForm(object sender, FormClosedEventArgs e)
        {
            
            if (Application.OpenForms["FormUserProfile"] == null)
            {
                timer1.Stop(); //We stop the timer once the user finishes editing his profile And closes the form
                               //Nous arrêtons le chronomètre une fois que l'utilisateur a fini de modifier son profil et ferme le formulaire
                              
            }
        }
        private void activatedButton(Button currentButton)
        {
            foreach (Control previousButton in panelMenu.Controls)
            {
                if (previousButton.GetType()==typeof(Button))
                    previousButton.BackColor = Color.FromArgb(37, 54, 75);
            }
            if(currentButton!=null)
            currentButton.BackColor = Color.FromArgb(0, 100, 182);
        }

        #endregion
        #region DÉCONNEXION ET SORTIE D'APPLICATION___LOGOUT AND APPLICATION EXIT

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to close the application?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                Application.Exit();
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to log out?", "Warning",
               MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
        }
        #endregion
        //LOAD
        private void FormPrincipal_Load(object sender, EventArgs e)
        {           
            LoadUserData();
            ManagePermissions();
        }

        private void LoadUserData()
        {
            lblUserName.Text = ActiveUser.c_userName;
            lblPosition.Text = ActiveUser.c_position;
            lblEmail.Text = ActiveUser.c_email;
            if (ActiveUser.c_profile != null) pictureProfile.Image = ConvertItem.binaryToImage(ActiveUser.c_profile);
        }


        private void ManagePermissions()
        {
            //Gérer les autorisations
            if (ActiveUser.c_position == Positions.comptable)
            {
                btnPacient.Enabled = false;
                btnClinicalHistory.Enabled = false;
                btnUsers.Enabled = false;
                btnCalendar.Enabled = false;
            }
            if (ActiveUser.c_position == Positions.secretariat)
            {
                Button1.Enabled = false;
                Button2.Enabled = false;
                btnUsers.Enabled = false;
            }
            if (ActiveUser.c_position == Positions.Doctor)
            {
                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadUserData(); //update the user's data in the menu (in this form), every 1 second at the time of the edition, on user profile form
                            //mettre à jour les données de l'utilisateur dans le menu (dans ce formulaire), toutes les 1 seconde au moment de l'édition, sur le formulaire de profil utilisateur
        }
        //"Des BOUTONS POUR OUVRIR LES FORMULAIRES  xx.show(); "
        //  |
        //  |
        // \ /
        //  -
        private void btnPacient_Click(object sender, EventArgs e)
        {
            openFormOnPanel<Form1>();
            activatedButton(btnPacient);

        }

        private void btnClinicalHistory_Click(object sender, EventArgs e)
        {
            openFormOnPanel<Form2>();
            activatedButton(btnClinicalHistory);
        }

       

        private void btnUsers_Click(object sender, EventArgs e)
        {
            openFormOnPanel<FormUsers>();
            activatedButton(btnUsers);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
          
        }

        private void panelformularios_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            openFormOnPanel<RGLRDV>();
            activatedButton(btnCalendar);
        }

        private void ptbProfile_Click(object sender, EventArgs e)
        {
            openFormOnPanel<FormUserProfile>();
            activatedButton(null);
            timer1.Start();
        }

        private void linkProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFormOnPanel<FormUserProfile>();
            activatedButton(null);
            timer1.Start();
        }


    }
}
