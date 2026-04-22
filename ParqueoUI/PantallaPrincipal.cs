using FontAwesome.Sharp;
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


namespace ParqueoUI
{
    public partial class PantallaPrincipal : Form
    {
        private IconButton currentButton;
        private Panel leftBorderBtn;
        private Form subFormActual;
        public PantallaPrincipal()
        {
            InitializeComponent();
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);

            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        private void abrirSubForm(Form subForm)
        {
            if(subFormActual != null)
            {
                subFormActual.Close();
            }
            subFormActual = subForm;
            subForm.TopLevel = false;
            subForm.FormBorderStyle = FormBorderStyle.None; 
            subForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(subForm);
            panelMain.BringToFront();
            subForm.Show();
            lblTItleBar.Text = currentButton.Text;
        }

        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                //Desmarcar el botón anterior
                DisableButton();


                currentButton = (IconButton)senderBtn;
                currentButton.BackColor = Color.FromArgb(37, 36, 81);
                currentButton.ForeColor = color;
                currentButton.TextAlign = ContentAlignment.MiddleCenter;
                currentButton.IconColor = color;
                currentButton.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentButton.ImageAlign = ContentAlignment.MiddleRight;

                //Borde izquierdo del botón
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentButton.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                iconCurrentForm.IconChar = currentButton.IconChar;
                iconCurrentForm.IconColor = color;
            }
        }

        private void DisableButton()
        {
            if (currentButton != null)
            {
                currentButton.BackColor = Color.FromArgb(31, 30, 68);
                currentButton.ForeColor = Color.Gainsboro;
                currentButton.TextAlign = ContentAlignment.MiddleLeft;
                currentButton.IconColor = Color.Gainsboro;
                currentButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentButton.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }
        private void PantallaPrincipal_Load(object sender, EventArgs e)
        {

        }

        // Estructura de colores para cada sección

        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(172, 126, 241);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            abrirSubForm(new FormRecientes());
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color2);
            abrirSubForm(new FormGenerarTicket());
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color3);
            abrirSubForm(new FormCobrarTicket());
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color4);
            abrirSubForm(new FormReportes());
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color5);
            abrirSubForm(new FormUsuarios());
        }

        private void lblTItleBar_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (subFormActual != null)
            {
                subFormActual.Close();
            }
            Reset();
        }

        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            iconCurrentForm.IconChar = IconChar.Home;
            iconCurrentForm.IconColor = Color.MediumPurple;
            lblTItleBar.Text = "Home";
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);



        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void iconPictureBox2_Click_1(object sender, EventArgs e)
        {
            
        }

        private void iconPictureBox3_Click(object sender, EventArgs e)
        {
           
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void panelTitleBar_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
