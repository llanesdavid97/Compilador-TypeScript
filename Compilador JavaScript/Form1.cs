using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows.Forms;

namespace Compilador_JavaScript
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region CERRAR
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region menuStrip ABRIR ARCHIVO
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirArchivo();
        }
        #endregion

        #region PICTURE ABRIR ARCHIVO
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            abrirArchivo();

        }
        #endregion

        #region VOID ABRIR ARCHIVO
        private void abrirArchivo()
        {
            try
            {
                openFileDialog1.Title = "Abrir archivo";
                openFileDialog1.ShowDialog();

                string direccion = openFileDialog1.FileName;

                if (File.Exists(openFileDialog1.FileName))
                {
                    TextReader textReader = new StreamReader(direccion);
                    rt_Path.Text = textReader.ReadToEnd();
                    rt_Path_Colored.Text = rt_Path.Text;

                    textReader.Close();
                }
                lbDirectorio.Text = ">> " + direccion;
            }
            catch (Exception)
            {
                MessageBox.Show("Ha ocurrido un problema para abrir el archivo!");
            }
        }
        #endregion

        #region PICTURE GUARDAR ARCHIVO
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            guardarArchivo();
        }
        #endregion

        #region VOID GUARDAR ARCHIVO
        private void guardarArchivo()
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string directorio = saveFileDialog1.FileName;

                    StreamWriter streamWriter = File.CreateText(directorio);
                    streamWriter.Write(rt_Path_Colored.Text);

                    streamWriter.Flush(); // Libera espacio en memoria
                    streamWriter.Close(); // Libera recursos

                    lbDirectorio.Text = ">> " + directorio;
                }
                else
                {
                    //agregar actualizar
                    // - Guardar == Actualizar
                    // - Guardar como == Crear nuevo Archivo

                    // - PictureBox Guardar == Actualizar
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se ha podido guardar...!");
            }
        }
        #endregion

        #region menuStrip GUARDAR
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guardarArchivo();
            //file.apend para actualizar o guardar cambios
            // y ejecutar Ctrl + G
        }
        #endregion

        #region BTN_Compilar
        private void btn_Run_Click(object sender, EventArgs e)
        {
            rt_Path.Text = rt_Path_Colored.Text;

            Lexico tokenLista = new Lexico(rt_Path.Text);
            dataGridTokens.Rows.Clear();
            dataGridErroresLexico.Rows.Clear();

            if (rt_Path_Colored.Text.Length == 0)
            {
                MessageBox.Show("ERROR!, No se puede compilar sin código.");
            }
            else if (abrirToolStripMenuItem.Checked)
            {
                abrirArchivo();
                foreach (Token lista in tokenLista.listaTokens)
                {
                    dataGridTokens.Rows.Add(lista._Lexema, lista._Token, lista._TipoToken, lista._linea);
                }

                dataGridErroresLexico.AutoGenerateColumns = true;
                foreach (Error error in tokenLista.listaError)
                {
                    dataGridErroresLexico.Rows.Add(error.codigo, error.mensajeError, error.tipo, error.Linea);

                }
            }
            else if (rt_Path_Colored.Text.Length >= 1){
                rt_Path.Text = rt_Path_Colored.Text;
                tokenLista.EjecutarLexico();

                foreach (Token lista in tokenLista.listaTokens)
                {
                    dataGridTokens.Rows.Add(lista._Lexema, lista._Token, lista._TipoToken, lista._linea);
                }

                dataGridErroresLexico.AutoGenerateColumns = true;
                foreach (Error error in tokenLista.listaError)
                {
                    dataGridErroresLexico.Rows.Add(error.codigo, error.mensajeError, error.tipo, error.Linea);

                }
            }
        }
        #endregion

        #region ACERCA DE
        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAyuda frmAyuda = new frmAyuda();
            frmAyuda.Show();
        }
        #endregion

        #region MINIMIZAR
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

    }
}
