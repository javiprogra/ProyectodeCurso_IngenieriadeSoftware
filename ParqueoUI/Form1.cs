using ParqueoBLL;
using ParqueoEntidades;
using ParqueoDAL;
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

namespace ParqueoUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if(textBoxUser.Text == "Usuario")
            {
                textBoxUser.Text = "";
                textBoxUser.ForeColor = Color.Gainsboro;

            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if(textBoxUser.Text == "")
            {
                textBoxUser.Text = "Usuario";
                textBoxUser.ForeColor = Color.LightGray;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "Contraseña")
            {
                textBoxPassword.Text = "";
                textBoxPassword.ForeColor = Color.Gainsboro;
                textBoxPassword.UseSystemPasswordChar = true;
            }

        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

     

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_MouseDown_1(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //Boton de acceso
        private void iconButton1_Click(object sender, EventArgs e)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            string login = textBoxUser.Text;
            string password = textBoxPassword.Text;

            //Encriptar contraseña
            string passwordHash = usuarioDAL.HashSHA256(password);

            Usuario usuario = usuarioDAL.ObtenerPorCredenciales(login, passwordHash);

            if (usuario != null)
            {
                MessageBox.Show("Bienvenido " + usuario.NombreUsuario);
                PantallaPrincipal pantallita = new PantallaPrincipal();
                pantallita.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
            }
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "")
            {
                textBoxPassword.Text = "Contraseña";
                textBoxPassword.ForeColor = Color.Gainsboro;
                textBoxPassword.UseSystemPasswordChar = false;
            }
        }

        private void viewPasswordBtn_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !textBoxPassword.UseSystemPasswordChar;
        }
    }
}
