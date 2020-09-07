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
     //		0	   1	 2	   3	 4	    5	   6      7	     8	    9	  10	  11    12     13	  14	  15	 16	     17	     18	      19	  20     21	     22 	23	      24	   25	   26	   27	    28	  29	  30	   31	
//		||  L  ||  D  || . ||  "  ||  ' ||  /  ||  *  ||  +  ||  -  ||  %  ||  =  ||  &  || '|' ||  !  ||  >  ||  <  ||  ?   ||  (    ||  )   ||  {   ||  }   ||  [   ||  ]  ||  ;   ||   :    ||  ,   ||  _  ||	" " || \n  || EOF || \t    ||  OC    ||	
/*0*/	{    1,    2,    7,    8,      9,   10,    15,    17,   20,    23,    25,    27,    29,    31,     33,    35,     37,      38,     39,     40,     41,     42,    43,     44,      45,     46,     1,       0,    0,       0,      0,    -500    },
/*1*/	{    1,    1,   -1, -503,   -503,   -1,    -1,    -1,   -1,    -1,    -1,    -1,    -1,    -1,     -1,    -1,     -1,      -1,     -1,     -1,     -1,   -504,    -1,     -1,      -1,     -1,     1,      -1,    -1,     -1,     -1,    -500    },
/*2*/	{    3,    2,    4, -503,   -503,   -2,    -2,    -2,   -2,    -2,    -2,    -2,    -2,    -2,     -2,    -2,     -2,      -2,     -2,     -2,     -2,   -504,    -2,      -2,     -2,     -2,     3,      -2,    -2,     -2,     -2,    -501    },
/*3*/	{    3,    3,    3, -503,   -503,  -501, -501,  -501, -501,  -501,  -501,  -501,   -501, -501,   -501,   -501,  -501,    -501,   -501,   -501,   -501,   -501,  -501,    -501,   -501,   -501,  -501,    -501,   -501,   -501,   -501,    -501    },
/*4*/	{    6,    5,    6, -503,   -503,  -501, -501,  -501, -501,  -501,  -501,  -501,   -501, -501,   -501,   -501,  -501,    -501,   -501,   -501,   -501,   -501,  -501,    -501,   -501,   -501,  -501,    -501,   -501,   -501,   -501,    -501    },
/*5*/	{    6,    5,    6, -503,   -503,    -3,   -3,    -3,   -3,    -3,    -3,    -3,     -3,   -3,     -3,     -3,    -3,      -3,     -3,     -3,     -3,     -3,    -3,      -3,     -3,     -3,  -501,      -3,     -3,     -3,     -3,    -501    },
/*6*/	{    6,    6,    6, -501,   -501,  -501, -501,  -501, -501,  -501,  -501,  -501,   -501, -501,   -501,   -501,  -501,    -501,   -501,   -501,   -501,   -501,  -501,    -501,   -501,   -501,   -501,   -501,   -501,   -501,   -501,    -501    },
/*7*/	{  -71,    5,    6,  -71,    -71,   -71,  -71,   -71,  -71,   -71,   -71,   -71,    -71,  -71,    -71,    -71,   -71,     -71,    -71,    -71,    -71,    -71,    -71,    -71,    -71,    -71,    -71,    -71,    -71,    -71,    -71,    -501    },
/*8*/	{    8,    8,    8,   -4,   -506,     8,    8,     8,    8,     8,     8,     8,      8,    8,      8,      8,     8,       8,      8,      8,      8,      8,      8,      8,      8,      8,      8,      8,   -505,      8,      8,       8    },
/*9*/	{    9,    9,    9, -506,     -4,     9,    9,     9,    9,     9,     9,     9,      9,    9,      9,      9,     9,       9,      9,      9,      9,      9,      9,      9,      9,      9,      9,      9,   -505,      9,      9,       8    },
/*10*/	{   -9,   -9,   -9,   -9,     -9,    12,   13,    -9,   -9,    -9,    11,    -9,     -9,   -9,     -9,     -9,    -9,      -9,     -9,     -9,     -9,     -9,     -9,     -9,     -9,     -9,     -9,     -9,     -9,     -9,     -9,    -501    },
/*11*/	{  -18,  -18,  -18,  -18,    -18,   -18,  -18,   -18,  -18,   -18,   -18,   -18,    -18,  -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -18,    -501    },
/*12*/	{   12,   12,   12,   12,     12,    12,   12,    12,   12,    12,    12,    12,     12,   12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,     12,      0,     12,     12,      12    }, 
/*13*/	{   13,   13,   13,   13,     13,    13,   14,    13,   13,    13,    13,    13,     13,   13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     13,     13,      13    },
/*14*/	{   14,   14,   14,   14,     14,     0,   14,    14,   14,    14,    14,    14,     14,   14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,     14,      14    },
/*15*/	{   -8,   -8,   -8,   -8,     -8,    -8,   -8,    -8,   -8,    -8,    16,    -8,     -8,   -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,     -8,    -501    },
/*16*/	{  -17,  -17,  -17,  -17,    -17,   -17,  -17,   -17,  -17,    -17,  -17,    -17,   -17,  -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -17,    -501    },
/*17*/	{   -6,   -6,   -6,   -6,     -6,    -6,   -6,    18,   -6,    -6,    19,     -6,    -6,   -6,     -6,     -6,     -6,     -6,     -6,     -6,     -6,     -6,     -6,     -6,     -6,     -6,    -6,     -6,      -6,     -6,     -6,    -501    },
/*18*/	{  -12,  -12,  -12,  -12,    -12,   -12,  -12,   -12,  -12,    -12,  -12,    -12,   -12,  -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -12,    -501    },
/*19*/	{  -15,  -15,  -15,  -15,    -15,   -15,  -15,   -15,  -15,    -15,  -15,    -15,   -15,  -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -15,    -501    },
/*20*/	{   -7,   -7,   -7,   -7,     -7,    -7,   -7,    -7,   21,    -7,    22,     -7,    -7,   -7,     -7,     -7,     -7,     -7,     -7,     -7,     -7,     -7,     -7,     -7,     -7,     -7,    -7,      -7,     -7,    -7,      -7,    -501    },
/*21*/	{   -13, -13,  -13,  -13,    -13,   -13,  -13,   -13,  -13,    -13,  -13,    -13,   -13,  -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -13,    -501    },
/*22*/	{   -16, -16,  -16,  -16,    -16,   -16,  -16,   -16,  -16,    -16,  -16,    -16,   -16,  -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -16,    -501    },
/*23*/	{   -10, -10,  -10,  -10,    -10,   -10,  -10,   -10,  -10,    -10,   24,    -10,   -10,  -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -10,    -501    },
/*24*/	{   -47, -47,  -47,  -47,    -47,   -47,  -47,   -47,  -47,    -47,  -47,    -47,   -47,  -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -501    },
/*25*/	{   -14, -14,  -14,  -14,    -14,   -14,  -14,   -14,  -14,    -14,   26,    -14,   -14,  -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -14,    -501    },
/*26*/	{   -45, -45,  -45,  -45,    -45,   -45,  -45,   -45,  -45,    -45,  -45,    -45,   -45,  -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -45,    -501    },
/*27*/	{   -35, -35,  -35,  -35,    -35,   -35,  -35,   -35,  -35,    -35,  -35,     28,   -35,  -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -35,    -501    },
/*28*/	{   -30, -30,  -30,  -30,    -30,   -30,  -30,   -30,  -30,    -30,  -30,    -30,   -30,  -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -30,    -501    },
/*29*/	{   -36, -36,  -36,  -36,    -36,   -36,  -36,   -36,  -36,    -36,  -36,    -36,    30,  -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -36,    -501    },
/*30*/	{   -31, -31,  -31,  -31,    -31,   -31,  -31,   -31,  -31,    -31,  -31,    -31,   -31,  -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -31,    -501    },
/*31*/	{   -32, -32,  -32,  -32,    -32,   -32,  -32,   -32,  -32,    -32,   32,    -32,   -32,  -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -32,    -501    },
/*32*/	{   -46, -46,  -46,  -46,    -46,   -46,  -46,   -46,  -46,    -46,  -46,    -46,   -46,  -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -46,    -501    },
/*33*/	{   -47, -47,  -47,  -47,    -47,   -47,  -47,   -47,  -47,    -47,   34,    -47,   -47,  -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -47,    -501    },
/*34*/	{   -49, -49,  -49,  -49,    -49,   -49,  -49,   -49,  -49,    -49,  -49,    -49,   -49,  -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -49,    -501    },
/*35*/	{   -48, -48,  -48,  -48,    -48,   -48,  -48,   -48,  -48,    -48,   36,    -48,   -48,  -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -48,    -501    },
/*36*/	{   -50, -50,  -50,  -50,    -50,   -50,  -50,   -50,  -50,    -50,  -50,    -50,   -50,  -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -50,    -501    },
/*37*/	{   -51, -51,  -51,  -51,    -51,   -51,  -51,   -51,  -51,    -51,  -51,    -51,   -51,  -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -51,    -501    },
/*38*/	{   -62, -62,  -62,  -62,    -62,   -62,  -62,   -62,  -62,    -62,  -62,    -62,   -62,  -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -62,    -501    },
/*39*/	{   -63, -63,  -63,  -63,    -63,   -63,  -63,   -63,  -63,    -63,  -63,    -63,   -63,  -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -63,    -501    },
/*40*/	{   -64, -64,  -64,  -64,    -64,   -64,  -64,   -64,  -64,    -64,  -64,    -64,   -64,  -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -64,    -501    },
/*41*/	{   -65, -65,  -65,  -65,    -65,   -65,  -65,   -65,  -65,    -65,  -65,    -65,   -65,  -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -65,    -501    },
/*42*/	{   -66, -66,  -66,  -66,    -66,   -66,  -66,   -66,  -66,    -66,  -66,    -66,   -66,  -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -66,    -501    },
/*43*/	{   -67, -67,  -67,  -67,    -67,   -67,  -67,   -67,  -67,    -67,  -67,    -67,   -67,  -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -67,    -501    },
/*44*/	{   -68, -68,  -68,  -68,    -68,   -68,  -68,   -68,  -68,    -68,  -68,    -68,   -68,  -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -68,    -501    },
/*45*/	{   -69, -69,  -69,  -69,    -69,   -69,  -69,   -69,  -69,    -69,  -69,    -69,   -69,  -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -69,    -501    },
/*46*/	{   -70, -70,  -70,  -70,    -70,   -70,  -70,   -70,  -70,    -70,  -70,    -70,   -70,  -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -70,    -501    },
/*47*/	{  -500,-500, -500, -500,   -500,  -500, -500,  -500, -500,   -500, -500,   -500,  -500, -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,   -500,    -501    }

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
                case "++":           return -12;
                case "+=":           return -15;
                case "--":           return -13;
                case "%=":           return -47;
                case "==":           return -45;
                case "&&":           return -30;
                case "||":           return -31;
                case "!=":           return -46;
                case ">=":           return -49;
                case "<=":           return -50;
                case "typeof":       return -33;
                case "instanceof":   return -34;
                case "var":          return -59;
                case "let":          return -60;
                case "const":        return -61;
                case "string":       return -72;
                case "number":       return -73;
                case "boolean":      return -74;
                case "static":       return -76;
                case "public":       return -77;
                case "private":      return -78;
                case "protected":    return -79;
                case "void":         return -84;
                case "interface":    return -85;
                case "constructor":  return -86;
                case "abstract":     return -87;
                case "class":        return -88;
                case "extends":      return -89;
                case "function":     return -90;
                case "prototype":    return -91;
                case "length":       return -92;
                case "get":          return -93;
                case "set":          return -94;
                case "if":           return -95;
                case "else":         return -96;
                case "switch":       return -98;
                case "case":         return -99;
                case "break":        return -100;
                case "default":      return -101;
                case "for":          return -102;
                case "in":           return -103;
                case "while":        return -104;
                case "do":           return -105;
                case "continue":     return -106;
                case "return":       return -107;
                case "this":         return -108;
                case "super":        return -109;
                case "new":          return -110;
                case "try":          return -111;
                case "catch":        return -112;
                case "finally":      return -113;
                case "throw":        return -114;
                case "false":        return -116;
                case "true":         return -115;
                case "null":         return -117;
                case "undefined":    return -118;
                case "import":       return -119;
                case "console":      return -120;
                case "log":          return -121;
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
            if (char.IsLetter(Caracter))     {  return 0;  }

            else if (char.IsDigit(Caracter)) {  return 1;  }

            else if (Caracter.Equals('.'))   {  return 2;  }

            else if (Caracter.Equals('"'))   {  return 3;  }

            else if (Caracter.Equals('\''))  {  return 4;  }

            else if (Caracter.Equals('/'))   {  return 5;  }

            else if (Caracter.Equals('*'))   {  return 6;  }

            else if (Caracter.Equals('+'))   {  return 7;  }

            else if (Caracter.Equals('-'))   {  return 8;  }

            else if (Caracter.Equals('%'))   {  return 9;  }

            else if (Caracter.Equals('='))   {  return 10; }

            else if (Caracter.Equals('&'))   {  return 11; }

            else if (Caracter.Equals('|'))   {  return 12; }

            else if (Caracter.Equals('!'))   {  return 13; }

            else if (Caracter.Equals('>'))   {  return 14; }

            else if (Caracter.Equals('<'))   {  return 15; }

            else if (Caracter.Equals('?'))   { return 16;  }

            else if (Caracter.Equals('('))   {  return 17; }

            else if (Caracter.Equals(')'))   {  return 18; }

            else if (Caracter.Equals('{'))   {  return 19; }

            else if (Caracter.Equals('}'))   {  return 20; }

            else if (Caracter.Equals('['))   {  return 21; }

            else if (Caracter.Equals(']'))   {  return 22; }

            else if (Caracter.Equals(';'))   {  return 23; }
            
            else if (Caracter.Equals(':'))   {  return 24; }

            else if (Caracter.Equals(','))   {  return 25; }

            else if (Caracter.Equals('_'))   {  return 26; }

            else if (Caracter.Equals(' '))   {  return 27; }

            else if (Caracter.Equals('\n'))  {  return 28; }

            else if (Caracter.Equals(' '))   {  return 29; }  //EOF

            else if (Caracter.Equals('\t'))  {  return 30; }

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
                case -1:  return TipoToken.Identificador;
                case -2:  return TipoToken.Entero;
                case -3:  return TipoToken.Decimal;
                case -4:  return TipoToken.Cadena;
                case -5:  return TipoToken.Cadena;
                case -6:  return TipoToken.OpAritmeticos;
                case -7:  return TipoToken.OpAritmeticos;
                case -8:  return TipoToken.OpAritmeticos;
                case -9:  return TipoToken.OpAritmeticos;
                case -10: return TipoToken.OpAritmeticos;
                case -11: return TipoToken.OpAritmeticos;
                case -12: return TipoToken.OpAsignacion;
                case -13: return TipoToken.OpAsignacion;
                case -14: return TipoToken.OpAsignacion;
                case -15: return TipoToken.OpAsignacion;
                case -16: return TipoToken.OpAsignacion;
                case -17: return TipoToken.OpAsignacion;
                case -18: return TipoToken.OpAsignacion;
                case -19: return TipoToken.OpAsignacion;
                case -30: return TipoToken.OpLogicos;
                case -31: return TipoToken.OpLogicos;
                case -32: return TipoToken.OpLogicos;
                case -33: return TipoToken.OpTipos;
                case -34: return TipoToken.OpTipos;
                case -35: return TipoToken.OpBitwise;
                case -36: return TipoToken.OpBitwise;
                case -45: return TipoToken.OpComparacion;
                case -46: return TipoToken.OpComparacion;
                case -47: return TipoToken.OpComparacion;
                case -48: return TipoToken.OpComparacion;
                case -49: return TipoToken.OpComparacion;
                case -50: return TipoToken.OpComparacion;
                case -51: return TipoToken.OpComparacion;
                case -59: return TipoToken.Declaraciones;
                case -60: return TipoToken.Declaraciones;
                case -61: return TipoToken.Declaraciones;
                case -62: return TipoToken.SimSimples;
                case -63: return TipoToken.SimSimples;
                case -64: return TipoToken.SimSimples;
                case -65: return TipoToken.SimSimples;
                case -66: return TipoToken.SimSimples;
                case -67: return TipoToken.SimSimples;
                case -68: return TipoToken.SimSimples;
                case -69: return TipoToken.SimSimples;
                case -70: return TipoToken.SimSimples;
                case -71: return TipoToken.SimSimples;
                case -72: return TipoToken.TipoDato;
                case -73: return TipoToken.TipoDato;
                case -74: return TipoToken.TipoDato;
                case -76: return TipoToken.Alcance;
                case -77: return TipoToken.Alcance;
                case -78: return TipoToken.Alcance;
                case -79: return TipoToken.Alcance;
                case -84: return TipoToken.Sentencia;
                case -85: return TipoToken.POO;
                case -86: return TipoToken.POO;
                case -87: return TipoToken.POO;
                case -88: return TipoToken.POO;
                case -89: return TipoToken.POO;
                case -90: return TipoToken.POO;
                case -91: return TipoToken.POO;
                case -92: return TipoToken.POO;
                case -93: return TipoToken.POO;
                case -94: return TipoToken.POO;
                case -95: return TipoToken.Sentencia;
                case -96: return TipoToken.Sentencia;
                case -97: return TipoToken.Sentencia;
                case -98: return TipoToken.Sentencia;
                case -99: return TipoToken.Sentencia;
                case -100: return TipoToken.Sentencia;
                case -101: return TipoToken.Sentencia;
                case -102: return TipoToken.Sentencia;
                case -103: return TipoToken.Sentencia;
                case -104: return TipoToken.Sentencia;
                case -105: return TipoToken.Sentencia;
                case -106: return TipoToken.Sentencia;
                case -107: return TipoToken.Sentencia;
                case -108: return TipoToken.Sentencia;
                case -109: return TipoToken.Sentencia;
                case -110: return TipoToken.Sentencia;
                case -111: return TipoToken.Sentencia;
                case -112: return TipoToken.Sentencia;
                case -113: return TipoToken.Sentencia;
                case -114: return TipoToken.Sentencia;
                case -115: return TipoToken.Sentencia;
                case -116: return TipoToken.Sentencia;
                case -117: return TipoToken.Sentencia;
                case -118: return TipoToken.Sentencia;
                case -119: return TipoToken.Sentencia;
                case -120: return TipoToken.POO;
                case -121: return TipoToken.POO;

                default: return TipoToken.OC;
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
                case -500: mensajeError = "Símbolo desconocido"; break;
                case -501: mensajeError = "Formato incorrecto, no se esperaba -71 [.]  o  -4 [cadena]"; break;
                case -502: mensajeError = "EOF Inesperado"; break;
                case -503: mensajeError = "No se esperaba cadena"; break;
                case -504: mensajeError = "No se esperaba '['"; break;
                case -505: mensajeError = "No se esperaba salto de linea"; break;
                case -506: mensajeError = "Formato incorrecto para cerrar cadena"; break;
                case -507: mensajeError = "Se esperaba cerrar cadena"; break;
                case -508: mensajeError = "No se esperba operador de tipo aritmético '-' "; break;
                case -509: mensajeError = "No se esperaba operador de tipo aritmético '+'"; break;
                case -510: mensajeError = "Se esperaba operador aritmetico o digito"; break;

                default: mensajeError = "Error inesperado"; break;
            }
            return new Error() { codigo = estado, mensajeError = mensajeError, tipo = tipoError.Lexico, Linea = lineaPublica -1};
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
                    #region VALIDADAR RETORNO DE CARACTER
                    if (lexema.Length > 1 && (lexema.Last().Equals('"')) != true && (lexema.Last().Equals('"')) != true && (lexema.Last() == Convert.ToChar("'")) != true && (lexema.Last().Equals("|")) != true)
                    {
                        if (lexema.Last().Equals('\n'))
                            linea--;
                        lexema = lexema.Remove(lexema.Length - 1);
                        puntero--;
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
                #region ERRORES
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
            #endregion
            return listaTokens;
        }

        #endregion



    }

}
