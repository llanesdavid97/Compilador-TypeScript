using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Compilador_JavaScript
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //private Propiedades[] arregloTokens = new Propiedades[115];
        private void Form1_Load(object sender, EventArgs e)
        {
            //    ------ Una manera de mostrar los token en grilla ------
            //arregloTokens[0] = new Propiedades("Id"     ,"Primitivo"     ,  -1, 0);
            //arregloTokens[1] = new Propiedades("Entero" ,"Primitivo"     ,  -2, 0);
            //arregloTokens[2] = new Propiedades("Decimal","Primitivo"     ,  -3, 0);
            //arregloTokens[3] = new Propiedades("Cadena" ,"Primitivo"     ,  -4, 0);
            //arregloTokens[4] = new Propiedades("+"      ,"Op. Aritmetico",  -5, 0);
            //arregloTokens[5] = new Propiedades("-"      ,"Op. Aritmetico",  -6, 0);
            //arregloTokens[6] = new Propiedades("*"      ,"Op. Aritmetico",  -7, 0);
            //arregloTokens[7] = new Propiedades("**"     ,"Op. Arimetico" ,  -8, 0);
            //arregloTokens[8] = new Propiedades("/"      ,"Op. Aritmetico",  -9, 0);
            //arregloTokens[9] = new Propiedades("%"      ,"Op. Aritmetico", -10, 0);

            //dataGridView1.DataSource = arregloTokens;

            List<Propiedades> propiedades = new List<Propiedades>();
            propiedades.Add(new Propiedades() { Lexema = "Id", tipo_Lexema = "Primitivo", Token = -1, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "Entero", tipo_Lexema = "Primitivo", Token = -2, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "Decimal", tipo_Lexema = "Primitivo", Token = -3, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "Cadena", tipo_Lexema = "Primitivo", Token = -4, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "Caracter", tipo_Lexema = "Primitivo", Token = -5, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "+", tipo_Lexema = "Op. Aritmetico", Token = -6, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "-", tipo_Lexema = "Op. Aritmetico", Token = -7, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "*", tipo_Lexema = "Op. Aritmetico", Token = -8, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "**", tipo_Lexema = "Op. Aritmetico", Token = -9, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "/", tipo_Lexema = "Op. Aritmetico", Token = -10, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "%", tipo_Lexema = "Op. Aritmetico", Token = -11, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "++", tipo_Lexema = "Op. Asignacion", Token = -12, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "--", tipo_Lexema = "Op. Asignacion", Token = -13, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "=", tipo_Lexema = "Op. Asignacion", Token = -14, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "+=", tipo_Lexema = "Op. Asignacion", Token = -15, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "-=", tipo_Lexema = "Op. Asignacion", Token = -16, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "*=", tipo_Lexema = "Op. Asiganacion", Token = -17, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "/=", tipo_Lexema = "Op. Asiganacion", Token = -18, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "%=", tipo_Lexema = "Op. Asiganacion", Token = -19, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "**=", tipo_Lexema = "Op. Asignacion", Token = -20, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "<<=", tipo_Lexema = "Op. Asignacion", Token = -21, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ">>=", tipo_Lexema = "Op. Asignacion", Token = -22, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ">>>=", tipo_Lexema = "Op. Asignacion", Token = -23, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "&&", tipo_Lexema = "Op. Logico", Token = -30, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "||", tipo_Lexema = "Op. Logico", Token = -31, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "!", tipo_Lexema = "Op. Logico", Token = -32, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "typeof", tipo_Lexema = "Op. Tipos", Token = -33, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "instanceof", tipo_Lexema = "Op. Tipos", Token = -34, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "&", tipo_Lexema = "Op. Bitwise", Token = -35, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "|", tipo_Lexema = "Op. Bitwise", Token = -35, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "~", tipo_Lexema = "Op. Bitwise", Token = -37, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "^", tipo_Lexema = "Op. Bitwise", Token = -38, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "<<", tipo_Lexema = "Op. Bitwise", Token = -39, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ">>", tipo_Lexema = "Op. Bitwise", Token = -40, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ">>>", tipo_Lexema = "Op. Bitwise", Token = -41, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "&=", tipo_Lexema = "Op. Bitwise", Token = -42, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "^=", tipo_Lexema = "Op. Bitwise", Token = -43, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "|=", tipo_Lexema = "Op. Bitwise", Token = -44, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "==", tipo_Lexema = "Op. Comparacion", Token = -45, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "===", tipo_Lexema = "Op. Comparacion", Token = -46, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "!=", tipo_Lexema = "Op. Comparacion", Token = -47, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "!==", tipo_Lexema = "Op. Comparacion", Token = -48, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ">", tipo_Lexema = "Op. Comparacion", Token = -49, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "<", tipo_Lexema = "Op. Comparacion", Token = -50, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ">=", tipo_Lexema = "Op. Comparacion", Token = -51, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "<=", tipo_Lexema = "Op. Comparacion", Token = -52, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "?", tipo_Lexema = "Op. Comparacion", Token = -53, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "var", tipo_Lexema = "Op. Declaraciones", Token = -59, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "let", tipo_Lexema = "Op. Declaraciones", Token = -60, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "const", tipo_Lexema = "Op. Declaraciones", Token = -61, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "(", tipo_Lexema = "Sim. Simples", Token = -62, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ")", tipo_Lexema = "Sim. Simples", Token = -63, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "{", tipo_Lexema = "Sim. Simples", Token = -64, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "}", tipo_Lexema = "Sim. Simples", Token = -65, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "[", tipo_Lexema = "Sim. Simples", Token = -66, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "]", tipo_Lexema = "Sim. Simples", Token = -67, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ";", tipo_Lexema = "Sim. Simples", Token = -68, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ":", tipo_Lexema = "Sim. Simples", Token = -69, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = ",", tipo_Lexema = "Sim. Simples", Token = -70, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "static", tipo_Lexema = "Alcance", Token = -76, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "class", tipo_Lexema = "POO", Token = -76, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "extends", tipo_Lexema = "POO", Token = -76, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "function", tipo_Lexema = "POO", Token = -76, Linea = 0 });

            propiedades.Add(new Propiedades() { Lexema = "if", tipo_Lexema = "Sentencia", Token = -95, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "else", tipo_Lexema = "Sentencia", Token = -96, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "else if", tipo_Lexema = "Sentencia", Token = -97, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "switch", tipo_Lexema = "Sentencia", Token = -98, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "case", tipo_Lexema = "Sentencia", Token = -99, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "break", tipo_Lexema = "Sentencia", Token = -100, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "default", tipo_Lexema = "Sentencia", Token = -101, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "for", tipo_Lexema = "Sentencia", Token = -102, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "in", tipo_Lexema = "Sentencia", Token = -103, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "while", tipo_Lexema = "Sentencia", Token = -104, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "do", tipo_Lexema = "Sentencia", Token = -105, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "continue", tipo_Lexema = "Sentencia", Token = -106, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "return", tipo_Lexema = "Sentencia", Token = -107, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "this", tipo_Lexema = "Sentencia", Token = -108, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "super", tipo_Lexema = "Sentencia", Token = -109, Linea = 0 });
            propiedades.Add(new Propiedades() { Lexema = "new", tipo_Lexema = "Sentencia", Token = -110, Linea = 0 });

            var list = new BindingList<Propiedades>(propiedades);
            dataGridView1.DataSource = list;
        }

        private void panel_Top_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
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
                    streamWriter.Write(rt_Path.Text);

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
        }
        #endregion
    }
}
