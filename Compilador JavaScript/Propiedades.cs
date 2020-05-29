using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_JavaScript
{
    public class Propiedades
    {
        #region ATRIBUTOS
        public string Lexema
        { get; set; }

        public string tipo_Lexema
        { get; set; }

        public int Token
        { get; set; }

        public int Linea
        { get; set; }

        #endregion

        #region CONSTRUCTORES
        public Propiedades(string lexema, string tipo_lexema, int token, int linea)
        {
            this.Lexema = lexema;
            this.tipo_Lexema = tipo_lexema;
            this.Token = token;
            this.Linea = linea;
        }

        public Propiedades()
        { }

        #endregion
    }
}
