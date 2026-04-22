using ParqueoDAL;
using ParqueoEntidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParqueoUI
{
    public partial class FormUsuarios : Form
    {
        public FormUsuarios()
        {
            InitializeComponent();

            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        private void FormUsuarios_Load(object sender, EventArgs e)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            dataGridUsers.DataSource = usuarioDAL.ObtenerTodos();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            labelMode.Text = "(Modo: Editar)";
            var fila = dataGridUsers.CurrentRow;

            idSeleccionado = (int)fila.Cells["IdUsuario"].Value;

            textBoxName.Text = fila.Cells["NombreUsuario"].Value.ToString();
            textBoxLogin.Text = fila.Cells["Login"].Value.ToString();

            textBoxPassword.Text = "";

            textBoxName.Enabled = true;
            //textBoxLogin.Enabled = true;
            textBoxPassword.Enabled = true;
        }

        bool esNuevo = false;
        int idSeleccionado = 0;
        private void newBtn_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            idSeleccionado = 0;
            labelMode.Text = "(Modo: Crear)";

            textBoxName.Text = "";
            textBoxLogin.Text = "";
            textBoxPassword.Text = "";

            textBoxName.Enabled = true;
            textBoxLogin.Enabled = true;
            textBoxPassword.Enabled = true;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            UsuarioDAL dal = new UsuarioDAL();

            if (esNuevo)
            {
                Usuario nuevo = new Usuario
                {
                    NombreUsuario = textBoxName.Text,
                    Login = textBoxLogin.Text,
                    Password = dal.HashSHA256(textBoxPassword.Text)
                };

                dal.Insertar(nuevo);
            }
            else
            {
                Usuario editar = new Usuario
                {
                    IdUsuario = idSeleccionado,
                    NombreUsuario = textBoxName.Text,
                    Login = textBoxLogin.Text,
                    Password = string.IsNullOrEmpty(textBoxPassword.Text)
                        ? null
                        : dal.HashSHA256(textBoxPassword.Text)
                };

                dal.Actualizar(editar);
            }

            
            dataGridUsers.DataSource = dal.ObtenerTodos();

            textBoxName.Text = "";
            textBoxLogin.Text = "";
            textBoxPassword.Text = "";

            textBoxName.Enabled = false;
            textBoxLogin.Enabled = false;
            textBoxPassword.Enabled = false;
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (dataGridUsers.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario");
                return;
            }

            int id = (int)dataGridUsers.CurrentRow.Cells["IdUsuario"].Value;
            string login = dataGridUsers.CurrentRow.Cells["Login"].Value.ToString();

            if (login == "admin")
            {
                MessageBox.Show("No se puede eliminar el administrador");
                return;
            }

            var confirm = MessageBox.Show(
                "¿Seguro que desea eliminar este usuario?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;
            //Eliminar (Baja logica)
            UsuarioDAL dal = new UsuarioDAL();
            dal.CambiarEstado(id, false);

            dataGridUsers.DataSource = dal.ObtenerTodos();
        }
    }
}
