using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.Design.WebControls;
using System.Windows.Forms;
using System.Xml.Schema;

namespace Compilador_JavaScript
{
    #region class Token
    public class Token
    {
        private int token;
        private string Lexema;
        private int linea;
        private TipoToken TipoToken;

        public int _Token { get => token; set => token = value; }
        public string _Lexema { get => Lexema; set => Lexema = value; }
        public int _Linea { get => linea; set => linea = value; }
        public TipoToken _TipoToken { get => TipoToken; set => TipoToken = value; }
    }
    #endregion

    #region class Error
    public class Error
    {
        public int codigo;
        public string mensajeError;
        public tipoError tipo;
        public int Linea;
    }
    #endregion

    #region tipoError enum
    public enum tipoError
    {
        Lexico, 
        Sintactico,
        Semantico,
        Ejecucion
    }
    #endregion

    #region TipoToken enum
    public enum TipoToken{
        Identificador,
        Entero,
        Decimal,
        Cadena, 
        OpAritmeticos,
        OpAsignacion,
        OpLogicos,
        OpBitwise,
        OpComparacion,
        Declaraciones,
        SimSimples,
        Alcance,
        POO,
        OpTipos,
        Sentencia,
        TipoDato,
        OC
    }
    #endregion

    public class Lexico
    {
        public List<Token> listaTokens; // Atributo de salida lexico
        private string codigoFuente;     // Atributo entrada lexico
        public  List<Error> listaError;  // Atributo Errores

        private int lineaPublica;

        #region MATRIZ DE TRANSICION
        private int[,] matrizTransicion =
        { 
                      //   0    1    2    3    4    5    6    7    8    9    10   11  12   13   14   15   16    17  18    19   20   21  22   23   24   25    26   27  28   29   30   31               
                     // || L || D || . || " || ' || / || * || + || - || % || = || & ||'|'|| ! || > || < || ? || ( || ) || {	|| } || [ || ] || ; || : || , || _ ||" "||\n ||EOF||\t ||OC ||
         /* 0  */      {   1,   2,   7,   8,   9,  10,  14,  15,  16,  17,  18,  20,  21,  22,  24,  25, -50, -62, -63, -64, -65, -66, -67, -68, -69, -70,   1,   0,   0,   0,   0, -500 },    //  0
         /* 1  */      {   1,   1,  -1,-503,-503,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,-504,  -1,  -1,  -1,  -1,   1,  -1,  -1,  -1,  -1 ,-500 },
         /* 2  */      {   3,   2,   4,-503,-503,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,-504,  -2,  -2,  -2,  -2,   2,  -2,  -2,  -2,  -2, -501 },    //  1
         /* 3  */      {   3,   3,   3,-503,-503,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501, -501 },    //  2
         /* 4  */      {   6,   5,   6,-503,-503,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,   6,-501,-501,-501,-501, -501 },    //  3
         /* 5  */      {   6,   5,   6,-503,-503,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3, -501,},    //  4
         /* 6  */      {   6,   6,   6,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501,-501, -501,},    //  5
         /* 7  */      { -71,   5,   6, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -71, -501 },    //  5
         /* 8  */      {   8,   8,   8,  -4,-506,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,   8,-505,   8,   8,    8 },    //  6
         /* 9  */      {   9,   9,   9,-506,  -4,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,-505,   9,   9,    9 },    //  7
         /* 10 */      {  -9,  -9,  -9,  -9,  -9,  11,  12,  -9,  -9,  -9, -18,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9, -501 },    //  8
         /* 11  */     {  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,  11,   0,  11,  11,  11, },    //  9
         /* 12 */      {  12,  12,  12,  12,  12,  12,  13,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12,  12  },    // 10
         /* 13 */      {  13,  13,  13,  13,  13,   0,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13,  13  },    // 11
         /* 14 */      {  -8,  -8,  -8,  -8,  -8,  -8,-501,-501,-501,  -8, -17,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8, -501 },    // 12
         /* 15 */      {  -6,  -6,  -6,  -6,  -6,  -6,-501, -12,-508,  -6, -15,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6, -501 },    // 13
         /* 16 */      {  -7,  -7,  -7,  -7,  -7,  -7,-501,-509, -13,  -7, -16,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7, -501 },    // 14 
         /* 17 */      { -10, -10, -10, -10, -10, -10, -10, -10, -10,-501, -47, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -501 },    // 15
         /* 18 */      { -14, -14, -14, -14, -14, -14, -14, -14, -14, -14,  19, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -501 },    // 16
         /* 19 */      { -45, -45, -45, -45, -45, -45, -45, -45, -45, -45,-501, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -45, -501 },    // 17
         /* 20 */      { -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -30, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -35, -501 },    // 18
         /* 21 */      { -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -31, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -36, -501 },    // 19
         /* 22 */      { -32, -32, -32, -32, -32, -32, -32, -32, -32, -32,  23, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -32, -501 },    // 20
         /* 23 */      { -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -46, -501 },    // 21 
         /* 24 */      { -47, -47, -47, -47, -47, -47, -47, -47, -47, -47, -49, -47, -47, -47,-501,-501, -47, -47, -47, -47, -47, -47, -47, -47, -47, -47, -47, -47, -47, -47, -47, -501 },    // 22
         /* 25 */      { -48, -48, -48, -48, -48, -48, -48, -48, -48, -48, -50, -48, -48, -48,-501,-501, -48, -48, -48, -48, -48, -48, -48, -48, -48, -48, -48, -48, -48, -48, -48, -501 }     // 23
        };
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Codigo"> El contenido del archivo que abrimos </param>

        #region CONSTRUCTOR
        public Lexico(string CodigoFuenteInterface)
        {
            codigoFuente = CodigoFuenteInterface + " ";
            listaTokens = new List<Token>();
            listaError = new List<Error>();
        }
        #endregion


        /// <summary>
        /// Metodo 
        /// </summary>
        /// <param name="lexema"></param>
        /// <returns> Metodo para regresar el token de la palabra reservada</returns>

        #region METODO PALABRA RESERVADA
        private int esPalabraReservada(string lexema)
        {
            switch (lexema)
            {
                case "++":
                    return -12;
                case "+=":
                    return -15;
                case "--":
                    return -13;
                case "%=":
                    return -47;
                case "==":
                    return -45;
                case "&&":
                    return -30;
                case "||":
                    return -31;
                case "!=":
                    return -46;
                case ">=":
                    return -49;
                case "<=":
                    return -50;
                case "typeof":
                    return -33;
                case "instanceof":
                    return -34;
                case "var":
                    return -59;
                case "let":
                    return -60;
                case "const":
                    return -61;
                case "string":
                    return -72;
                case "number":
                    return -73;
                case "boolean":
                    return -74;
                case "static":
                    return -76;
                case "public":
                    return -77;
                case "private":
                    return -78;
                case "protected":
                    return -79;
                case "void":
                    return -84;
                case "interface":
                    return -85;
                case "constructor":
                    return -86;
                case "abstract":
                    return -87;
                case "class":
                    return -88;
                case "extends":
                    return -89;
                case "function":
                    return -90;
                case "prototype":
                    return -91;
                case "length":
                    return -92;
                case "get":
                    return -93;
                case "set":
                    return -94;
                case "if":
                    return -95;
                case "else":
                    return -96;
                 case "switch":
                    return -98;
                case "case":
                    return -99;
                case "break":
                    return -100;
                case "default":
                    return -101;
                case "for":
                    return -102;
                case "in":
                    return -103;
                case "while":
                    return -104;
                case "do":
                    return -105;
                case "continue":
                    return -106;
                case "return":
                    return -107;
                case "this":
                    return -108;
                case "super":
                    return -109;
                case "new":
                    return -110;
                case "try":
                    return -111;
                case "catch":
                    return -112;
                case "finally":
                    return -113;
                case "throw":
                    return -114;
                case "false":
                    return -115;
                case "true":
                    return -116;
                case "null":
                    return -117;
                case "undefined":
                    return -118;
                case "import":
                    return -119;
                case "console":
                    return -120;
                case "log":
                    return -121;
                default: 
                    return -1;
            }
        }
        #endregion


        /// <summary>
        /// Metodo
        /// </summary>
        /// <param name="i"></param>
        /// <returns>Metodo para regresar el siguiente caracter del codigo fuente</returns>

        #region METODO SIGUIENTE CARACTER
        private char SiguienteCaracter(int i)
        {
            return Convert.ToChar(codigoFuente.Substring(i, 1));
        }
        #endregion


        /// <summary>
        /// Metodo
        /// </summary>
        /// <param name="Caracter"></param>
        /// <returns>Metodo para regresar numero de columna</returns>
        /// 
        #region METODO REGRESAR COLUMNA
        private int RegresarColumna(char Caracter)
        {
            if (char.IsLetter(Caracter))
            {
                return 0;
            }
            else if (char.IsDigit(Caracter))
            {
                return 1;
            }
            else if (Caracter.Equals('.'))
            {
                return 2;
            }
            else if (Caracter.Equals('"'))
            {
                return 3;
            }
            else if (Caracter.Equals('\''))
            {
                return 4;
            }
            else if (Caracter.Equals('/'))
            {
                return 5;
            }
            else if (Caracter.Equals('*'))
            {
                return 6;
            }
            else if (Caracter.Equals('+'))
            {
                return 7;
            }
            else if (Caracter.Equals('-'))
            {
                return 8;
            }
            else if (Caracter.Equals('%'))
            {
                return 9;
            }
            else if (Caracter.Equals('='))
            {
                return 10;
            }
            else if (Caracter.Equals('&'))
            {
                return 11;
            }
            else if (Caracter.Equals('|'))
            {
                return 12;
            }
            else if (Caracter.Equals('!'))
            {
                return 13;
            }
            else if (Caracter.Equals('>'))
            {
                return 14;
            }
            else if (Caracter.Equals('<'))
            {
                return 15;
            }
            else if (Caracter.Equals('?'))
            {
                return 16;
            }
            else if (Caracter.Equals('('))
            {
                return 17;
            }
            else if (Caracter.Equals(')'))
            {
                return 18;
            }
            else if(Caracter.Equals('{'))
            {
                return 19;
            }
            else if (Caracter.Equals('}'))
            {
                return 20;
            }
            else if (Caracter.Equals('['))
            {
                return 21;
            }
            else if (Caracter.Equals(']'))
            {
                return 22;
            }
            else if (Caracter.Equals(';'))
            {
                return 23;
            }
            else if (Caracter.Equals(':'))
            {
                return 24;
            }
            else if (Caracter.Equals(','))
            {
                return 25;
            }
            else if (Caracter.Equals('_'))
            {
                return 26;
            }
            else if(Caracter.Equals(' '))  // ESPACIO
            {
                return 27;
            }
            else if (Caracter.Equals('\n')) // ENTER
            {
                return 28;
            }
            else if (Caracter.Equals(' ')) // AGREGAR EOF?
            {
                return 29;
            }
            else if(Caracter.Equals('\t')) // AGREGAR TAP
            {
                return 30;
            }
            else
            {
                return 31;
            }
        }
        #endregion


        /// <summary>
        /// Metodo
        /// </summary>
        /// <param name="estado"></param>
        /// <returns>Tipo de Token</returns>
        /// 
        #region METODO esTipo
        private TipoToken esTipo(int estado)
        {
            switch (estado)
            {
                case -1:
                    return TipoToken.Identificador;
                case -2:
                    return TipoToken.Entero;
                case -3:
                    return TipoToken.Decimal;
                case -4:
                    return TipoToken.Cadena;
                case -5:
                    return TipoToken.Cadena;
                case -6:
                    return TipoToken.OpAritmeticos;
                case -7:
                    return TipoToken.OpAritmeticos;
                case -8:
                    return TipoToken.OpAritmeticos;
                case -9:
                    return TipoToken.OpAritmeticos;
                case -10:
                    return TipoToken.OpAritmeticos;
                case -11:
                    return TipoToken.OpAritmeticos;
                case -12:
                    return TipoToken.OpAsignacion;
                case -13:
                    return TipoToken.OpAsignacion;
                case -14:
                    return TipoToken.OpAsignacion;
                case -15:
                    return TipoToken.OpAsignacion;
                case -16:
                    return TipoToken.OpAsignacion;
                case -17:
                    return TipoToken.OpAsignacion;
                case -18:
                    return TipoToken.OpAsignacion;
                case -19:
                    return TipoToken.OpAsignacion;
                case -30:
                    return TipoToken.OpLogicos;
                case -31:
                    return TipoToken.OpLogicos;
                case -32:
                    return TipoToken.OpLogicos;
                case -33:
                    return TipoToken.OpTipos;
                case -34:
                    return TipoToken.OpTipos;
                case -35:
                    return TipoToken.OpBitwise;
                case -36:
                    return TipoToken.OpBitwise;
                case -45:
                    return TipoToken.OpComparacion;
                case -46:
                    return TipoToken.OpComparacion;
                case -47:
                    return TipoToken.OpComparacion;
                case -48:
                    return TipoToken.OpComparacion;
                case -49:
                    return TipoToken.OpComparacion;
                case -50:
                    return TipoToken.OpComparacion;
                case -51:
                    return TipoToken.OpComparacion;
                case -59:
                    return TipoToken.Declaraciones;
                case -60:
                    return TipoToken.Declaraciones;
                case -61:
                    return TipoToken.Declaraciones;
                case -62:
                    return TipoToken.SimSimples;
                case -63:
                    return TipoToken.SimSimples;
                case -64:
                    return TipoToken.SimSimples;
                case -65:
                    return TipoToken.SimSimples;
                case -66:
                    return TipoToken.SimSimples;
                case -67:
                    return TipoToken.SimSimples;
                case -68:
                    return TipoToken.SimSimples;
                case -69:
                    return TipoToken.SimSimples;
                case -70:
                    return TipoToken.SimSimples;
                case -71:
                    return TipoToken.SimSimples;
                case -72:
                    return TipoToken.TipoDato;
                case -73:
                    return TipoToken.TipoDato;
                case -74:
                    return TipoToken.TipoDato;
                case -76:
                    return TipoToken.Alcance;
                case -77:
                    return TipoToken.Alcance;
                case -78:
                    return TipoToken.Alcance;
                case -79:
                    return TipoToken.Alcance;
                case -84:
                    return TipoToken.Sentencia;
                case -85:
                    return TipoToken.POO;
                case -86:
                    return TipoToken.POO;
                case -87:
                    return TipoToken.POO;
                case -88:
                    return TipoToken.POO;
                case -89:
                    return TipoToken.POO;
                case -90:
                    return TipoToken.POO;
                case -91:
                    return TipoToken.POO;
                case -92:
                    return TipoToken.POO;
                case -93:
                    return TipoToken.POO;
                case -94:
                    return TipoToken.POO;
                case -95:
                    return TipoToken.Sentencia;
                case -96:
                    return TipoToken.Sentencia;
                case -97:
                    return TipoToken.Sentencia;
                case -98:
                    return TipoToken.Sentencia;
                case -99:
                    return TipoToken.Sentencia;
                case -100:
                    return TipoToken.Sentencia;
                case -101:
                    return TipoToken.Sentencia;
                case -102:
                    return TipoToken.Sentencia;
                case -103:
                    return TipoToken.Sentencia;
                case -104:
                    return TipoToken.Sentencia;
                case -105:
                    return TipoToken.Sentencia;
                case -106:
                    return TipoToken.Sentencia;
                case -107:
                    return TipoToken.Sentencia;
                case -108:
                    return TipoToken.Sentencia;
                case -109:
                    return TipoToken.Sentencia;
                case -110:
                    return TipoToken.Sentencia;
                case -111:
                    return TipoToken.Sentencia;
                case -112:
                    return TipoToken.Sentencia;
                case -113:
                    return TipoToken.Sentencia;
                case -114:
                    return TipoToken.Sentencia;
                case -115:
                    return TipoToken.Sentencia;
                case -116:
                    return TipoToken.Sentencia;
                case -117:
                    return TipoToken.Sentencia;
                case -118:
                    return TipoToken.Sentencia;
                case -119:
                    return TipoToken.Sentencia;
                case -120:
                    return TipoToken.POO;
                case -121:
                    return TipoToken.POO;
                default:
                    return TipoToken.OC;
            }
        }
        #endregion


        /// <summary>
        /// Metodo
        /// </summary>
        /// <param name="estado"></param>
        /// <returns>Errores</returns>
        /// 
        #region METODO MANEJO DE ERRORES
        private Error ManejoErrores(int estado)
        {
            string mensajeError;
            switch (estado)
            {
                case -500:
                    mensajeError = "Símbolo desconocido";
                    break;
                case -501:
                    mensajeError = "Formato incorrecto, se esperaba digito";
                    break;
                case -502:
                    mensajeError = "EOF Inesperado";
                    break;
                case -503:
                    mensajeError = "No se esperaba cadena";
                    break;
                case -504:
                    mensajeError = "No se esperaba '['";
                    break;
                case -505:
                    mensajeError = "No se esperaba salto de linea";
                    break;
                case -506:
                    mensajeError = "Formato incorrecto para cerrar cadena";
                    break;
                case -507:
                    mensajeError = "Se esperaba cerrar cadena";
                    break;
                case -508:
                    mensajeError = "No se esperba operador de tipo aritmético '-' ";
                        break;
                case -509:
                    mensajeError = "No se esperaba operador de tipo aritmético '+'";
                    break;
                case -510:
                    mensajeError = "Se esperaba operador aritmetico o digito";
                    break;
                default:
                    mensajeError = "Error inesperado";
                    break;
            }
            return new Error() { codigo = estado, mensajeError = mensajeError, tipo = tipoError.Lexico, Linea = lineaPublica};
        }
        #endregion


        /// <summary>
        /// List<>
        /// </summary>
        /// <returns>Metodo List para ejecutar lexico</returns>


        #region LIST EJECUTAR LEXICO
        public List<Token> EjecutarLexico()
        {
            int estado = 0;    
            int columna = 0;


            char caracterActual;
            string lexema = string.Empty; // Lambda
            int linea = 1;
            
            for (int puntero = 0; puntero < codigoFuente.ToCharArray().Length; puntero++)
            {
               caracterActual = SiguienteCaracter(puntero);
                
               if (caracterActual.Equals('\n'))
               {
                    linea += 1;
               }

                lexema += caracterActual;
             
                columna = RegresarColumna(caracterActual);
                estado = matrizTransicion[estado, columna];

                 
                if (estado < 0 && estado > -500)
                {
                    #region VALIDACION LETRAS

                    if (lexema.Equals(".a") || lexema.Equals(".A"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".b") || lexema.Equals(".B"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".c") || lexema.Equals(".C"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".d") || lexema.Equals(".D"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".e") || lexema.Equals(".E"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".f") || lexema.Equals(".F"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".g") || lexema.Equals(".G"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".h") || lexema.Equals(".H"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".i") || lexema.Equals(".I"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".j") || lexema.Equals(".J"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".k") || lexema.Equals(".K"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".l") || lexema.Equals(".L"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".m") || lexema.Equals(".M"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".n") || lexema.Equals(".N"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".o") || lexema.Equals(".O"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".p") || lexema.Equals(".P"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".q") || lexema.Equals(".Q"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".r") || lexema.Equals(".R"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".s") || lexema.Equals(".S"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".t") || lexema.Equals(".T"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".u") || lexema.Equals(".U"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".v") || lexema.Equals(".V"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".w") || lexema.Equals(".W"))
                    { 
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".x") || lexema.Equals(".X"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".y") || lexema.Equals(".Y"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals(".z") || lexema.Equals(".Z"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    #endregion

                    #region VALIDACION NUMEROS
                   
                    if (lexema.Equals("0;") || lexema.Equals("0)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("1;") || lexema.Equals("1)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("2;") || lexema.Equals("2)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("3;") || lexema.Equals("3)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("4;") || lexema.Equals("4)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("5;") || lexema.Equals("5)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("6;") || lexema.Equals("5)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("7;") || lexema.Equals("7)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("8;") || lexema.Equals("8)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    if (lexema.Equals("9;") || lexema.Equals("9)"))
                    {
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
                    }
                    #endregion

                    #region VALIDADCION DE esTIPO
                    if (estado == -1)
                    {
                        
                        if (lexema.Length > 1)
                        {
                            lexema = lexema.Remove(lexema.Length - 1);
                            puntero--;
                        }
                    }
                    #endregion

                    #region AGREGAR TOKEN
                    Token nuevoToken = new Token()
                    { _Token = estado, _Lexema = lexema, _Linea = linea };
                    #endregion

                    #region VALIDADR IDENTIFICADOR
                    if (estado == -1)
                        
                        nuevoToken._Token = esPalabraReservada(nuevoToken._Lexema);
                        nuevoToken._TipoToken = esTipo(nuevoToken._Token);

                        listaTokens.Add(nuevoToken);
                    #endregion

                    #region RESETEO
                    estado = 0;
                        columna = 0;
                        lexema = string.Empty;
                        #endregion


                }
                else if (estado <= -500)
                 {
                    #region MANEJO DE ERRORES
                    lineaPublica = linea;
                    listaError.Add(ManejoErrores(estado));
                    estado = 0;
                    columna = 0;
                    lexema = string.Empty; // Lambda
                    #endregion
                }
                else if(estado == 0)
                {
                    columna = 0;
                    lexema = string.Empty;
                }
   
            }
            return listaTokens;
        }

        #endregion



    }

}
