using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Compilador_JavaScript
{
    public class Sintactico
    {
        
        public List<Error> listaError;
        List<Token> listaTokens; // Copia lista tokens original
        private int[] listaSintactico = new int[200]; // AS LIFO
        public bool error = false; // Bandera de errores
        bool revision = false; // Bandera para decidir si analizar o salir 

        /// <summary>
        /// Punteros de las lista tokens y AS
        /// </summary>
        int punteroLexico = 0;
        int punteroSintactico = 1;

        int intentosRecuperar;
        TipoRecuperacion tipoRecuperacion;

        enum TipoRecuperacion
        {
            Ninguna,
            Falta,
            Sobra,
            Diferentes,
            Urgencia,
            NoMas
        }



        #region MATRIZ SINTACTICA II
        public int[,] MatrizSintactica = new int[,]
    {
        	                                    //  -1	    -2	     -3	     -4	     -6	     -7	     -8	     -9	    -10	    -12	    -13	    -14	    -30	    -31	    -45	    -46	     -47	-48	     -49	 -50	-59	    -62	    -63  	-64	    -65	     -66	 -67	 -68	 -69	 -70	-71	    -72	    -73	     -74	-84	    -76	     -77	-78	     -79	 -88	-89	     -95	-96	     -98	-99	   -100	    -101	-102	-104	-105	-107	-115	-116	-119	-200	
		                                        //   0	     1	      2 	  3 	  4	      5  	  6	      7	      8	      9	     10	     11	     12	     13	     14	     15	      16	 17	      18	  19	 20 	 21	     22	     23	     24	      25	  26	  27	  28	  29	 30	     31	     32	      33	 34	     35	      36	  37	  38	  39	 40	      41     42	      43	 44	     45	      46	  47	 48	      49	  50	 51	     52	      53	 54	
		                                        //	  ID    INT	     DOU	 CAD	  +    	  -	      *	      /	      %	     ++	     --	      =	     &&	     ||	     ==	     !=	      >	      <	      >=	  <=	 var	  (	      )	      {	      }    	  [	      ]	      ;	      :	      ,	      .	   string	number	boolean	void	static	public	private	 prot   class  extends	 if  	else	switch	case	break	default	 for	while	  do	return	TRUE	FALSE	import	  $	
        /*0*/	/* 1000.  S */	                {   -600    ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,1      ,1      ,1      ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,-600   ,1      ,-600   },
        /*1*/	/* 1001. SCRIPT*/	            {   -601    ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,2      ,2      ,2      ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,-601   ,2      ,-601   },
        /*2*/	/* 1002. LIBREARIAS*/	        {   -602    ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,3      ,3      ,3      ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,-602   ,4      ,-602   },
        /*3*/	/* 1003. LIB*/	                {   -603    ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-602   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-601   ,-601   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,-603   ,5      ,-603   },
        /*4*/	/* 1004. LIB_1*/	            {   -604    ,-604   ,-604   ,6      ,-604   ,-604   ,-604   ,-604   ,-604   ,-603   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-601   ,-601   ,-603   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   ,-604   },
        /*5*/	/* 1005. CLASES*/	            {   -605    ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-604   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,7      ,7      ,7      ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   ,-605   },
        /*6*/	/* 1006. CLASE*/	            {   -606    ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,8      ,9      ,10     ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   ,-606   },
        /*7*/	/* 1007. CLASE_1*/	            {   -607    ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,12     ,12     ,12     ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,-607   ,11     },
        /*8*/	/* 1008. HERENCIA*/	            {   -608    ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,13     ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-602   ,-602   ,-608   ,-608   ,14     ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   ,-608   },
        /*9*/	/* 1009. MIEMBROS*/	            {   -609    ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-608   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,15     ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,16     ,16     ,16     ,16     ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   ,-609   },
        /*10*/	/* 1010. MIEMBRO */	            {   -610    ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,17     ,17     ,17     ,17     ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   ,-610   },
        /*11*/	/* 1011. MIEMBRO1*/	            {   -611    ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,18     ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,19     ,19     ,19     ,19     ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   ,-611   },
        /*12*/	/* 1012. METATRIPCON */         {   -612    ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,21     ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,20     ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   ,-612   },
        /*13*/	/* 1013. ACCESO */	            {   -613    ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,22     ,23     ,24     ,25     ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   ,-613   },
        /*14*/	/* 1014. TIPO_DATO */	        {   -614    ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,26     ,27     ,28     ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   ,-614   },
        /*15*/	/* 1015. PARAM */	            {     30    ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,29     ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   ,-615   },
        /*16*/	/* 1016. PARAMS */	            {   -616    ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,31     ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,32     ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   ,-616   },
        /*17*/	/* 1017. TIPO_METODO */     	{   -617    ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,33     ,34     ,35     ,36     ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   ,-617   },
        /*18*/	/* 1018. DECLARACION_PROPIE */	{   -618    ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,38     ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,37     ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   ,-618   },
        /*19*/	/* 1019. EXP */	                {     39    ,39     ,39     ,39     ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   ,-619   },
        /*20*/	/* 1020. FACTOR*/	            {     40    ,41     ,42     ,43     ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   ,-620   },
        /*21*/	/* 1021. TERMINO */	            {   -621    ,-621   ,-621   ,-621   ,45     ,45     ,45     ,45     ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,44     ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   ,-621   },
        /*22*/	/* 1022. OP_ARITMETICO */	    {   -622    ,-622   ,-622   ,-622   ,46     ,47     ,48     ,49     ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   ,-622   },
        /*23*/  /* 1023. COMPLEMENTO_DECLA */   {     50    ,50     ,50     ,50     ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,-623   ,51     ,52     ,-623   ,-623  },
        /*24*/	/* 1024. SENTENCIAS */	        {   -624    ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,53     ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   ,-624   },


     };
        #endregion
        // -201 = LAMBDA
        // -200 = $

        #region  REPOSITORIO DE REGLAS
        public int[,] RepositorioReglas = new int[,]
        {
            /*1*/	/* 1000. <S> */	                {   1001 ,-200   ,0      ,0      ,0     ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*2*/	/* 1001. <SCRIPT> */	        {   1005 ,1002   ,-200   ,0      ,0     ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*3*/	/* 1002. <LIBRERIAS> */	        {   -201 ,-200   ,0      ,0      ,0     ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*4*/	/* 1002. <LIBRERIAS> */	        {   1002 ,1003   ,-200   ,0      ,0     ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*5*/	/* 1003. <LIB> */	            {   1004 ,-119   ,-200   ,0      ,0     ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*6*/	/* 1004. <LIB1> */	            {   -68  ,-4     ,-200   ,0      ,0     ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*7*/	/* 1005. <CLASES> */	        {   1007 ,1006   ,-200   ,0      ,0     ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*8*/	/* 1006. <CLASE> */	            {   -65  ,1009   ,-64    ,1008   ,-1    ,-88    ,-77    ,-200   ,0  ,0  ,0  ,0  },
            /*9*/	/* 1006. <CLASE> */	            {   -65  ,1009   ,-64    ,1008   ,-1    ,-88    ,-78    ,-200   ,0  ,0  ,0  ,0  },
            /*10*/	/* 1006. <CLASE> */	            {   -65  ,1009   ,-64    ,1008   ,-1    ,-88    ,-79    ,-200   ,0  ,0  ,0  ,0  },
            /*11*/	/* 1007. <CLASES1> */	        {   -201 ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*12*/	/* 1007. <CLASES1> */	        {   1007 ,1006   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*13*/	/* 1008. <HERENCIA> */	        {   -201 ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*14*/	/* 1008. <HERENCIA> */	        {   -1   ,-89    ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*15*/	/* 1009. <MIEMBROS> */	        {   -201 ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*16*/	/* 1009. <MIEMBROS> */	        {   1011 ,1010   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*17*/	/* 1010. <MIEMBRO> */	        {   1012 ,-1     ,1013   ,-200   ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*18*/	/* 1011. <MIEMBRO1> */	        {   -201 ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*19*/	/* 1011. <MIEMBRO1> */	        {   1011 ,1010   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*20*/	/* 1012. <METATRIPCON> */ 	    {   1018 ,1014   ,-69    ,-200   ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*21*/	/* 1012. <METATRIPCON> */ 	    {   -65  ,1024   ,-64    ,1017   ,-69    ,-63    ,1015  ,-62    ,-200   ,0  ,0  ,0  },
            /*22*/	/* 1013. <ACCESO> */	        {   -76  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*23*/	/* 1013. <ACCESO> */	        {   -77  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*24*/	/* 1013. <ACCESO> */	        {   -78  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*25*/	/* 1013. <ACCESO> */	        {   -79  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*26*/	/* 1014. <TIPO_DATO */	        {   -72  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*27*/	/* 1014. <TIPO_DATO */	        {   -73  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*28*/	/* 1014. <TIPO_DATO */	        {   -74  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*29*/	/* 1015. <PARAM> */	            {   -201 ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*30*/	/* 1015. <PARAM> */	            {   1016 ,1014   ,-69    ,-1     ,-200   ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*31*/	/* 1016. <PARAMS> */	        {   -201 ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*32*/	/* 1016. <PARAMS> */	        {   1016 ,1014   ,-69    ,-1     ,-70    ,-200   ,0  ,0  ,0  ,0  ,0  ,0  },
            /*33*/	/* 1017. <TIPO_METODO> */	    {   -72  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*34*/	/* 1017. <TIPO_METODO> */	    {   -73  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*35*/	/* 1017. <TIPO_METODO> */	    {   -74  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*36*/	/* 1017. <TIPO_METODO> */	    {   -84  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*37*/	/* 1018. <DECLARACION_PROPI> */	{   -68  ,-200   ,0      ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*38*/	/* 1018. <DECLARACION_PROPI> */	{   -68  ,1023   ,-14    ,-200   ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*39*/	/* 1019. <EXP> */	            {   1021 ,1020   ,-200   ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*40*/	/* 1020. <FACTOR> */	        {   -1   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*41*/	/* 1020. <FACTOR> */	        {   -2   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*42*/	/* 1020. <FACTOR> */	        {   -3   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*43*/	/* 1020. <FACTOR> */	        {   -4   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*44*/	/* 1021. <TERMINO> */	        {   -201 ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*45*/	/* 1021. <TERMINO> */	        {   1019 ,1022   ,-200   ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*46*/	/* 1022. <OP_ARITMETICO> */ 	{   -6   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*47*/	/* 1022. <OP_ARITMETICO> */	    {   -7   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*48*/	/* 1022. <OP_ARITMETICO> */	    {   -8   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*49*/	/* 1022. <OP_ARITMETICO> */	    {   -9   ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*50*/  /* 1023. <COMPLEMENTO_DECLA> */ {  1019  ,-200   ,0      ,0 ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*51*/  /* 1023. <COMPLEMENTO_DECLA> */ {  -115  ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*52*/  /* 1023. <COMPLEMENTO_DECLA> */ {  -116  ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },
            /*53*/	/* 1024. <SENTENCIAS> */	    {  -201  ,-200   ,0      ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  ,0  },


        };
        #endregion


        #region CONSTRUCTOR
        public Sintactico(List<Token> listaTokenLexico)
        {
            // Incializando
            listaError = new List<Error>();
            listaTokens = listaTokenLexico;
            listaTokens.Add(new Token() { _Lexema = "$", _Linea = 0, _TipoToken = TipoToken.Cadena, _Token = -200 });

            listaSintactico[0] = -200; // $
            listaSintactico[1] = 1000; // S

            intentosRecuperar = 0;
            tipoRecuperacion = TipoRecuperacion.Ninguna;

            EjecutarSintactico(listaTokens);
        }
        #endregion

        #region EJECUTAR SINTACTICO
        public void EjecutarSintactico(List<Token> listaTokens)
        {
            // valores locales
            int renglon, columna = 0;
            int regla;

            do
            {
                // Validacion si el elemento de la lista es terminal
                if (listaSintactico[punteroSintactico] < 0)
                {
                    // Validacion si LAMBDA es terminal
                    if (listaSintactico[punteroSintactico] == -201)
                    {
                        listaSintactico[punteroSintactico] = 0;
                        punteroSintactico--;
                    }

                    // Los dos son iguales?? WTF
                    else if (listaSintactico[punteroSintactico] == listaTokens[punteroLexico]._Token)
                    {
                        // Ambos son $ ?? 
                        if (listaSintactico[punteroSintactico] == -200)
                        {
                            revision = true;
                            if (error)
                                MessageBox.Show(@"Analisis Sintactico Terminado, Con Errores");
                            else
                                MessageBox.Show(@"Analisis Sintactico Terminado Correctamente");
                            break;

                        }
                        else
                        {
                            // MATCH  ELEMENTO
                            listaSintactico[punteroSintactico] = 0;
                            punteroLexico++;
                            punteroSintactico--;
                            VerificarRecuperacion();
                        }
                    }
                    else
                    {
                        // Lista de tokenks y lista sintactica no son iguales
                        NuevoError(listaTokens, ref revision, 1);
                    }
                }
                else
                {
                    // NUMERO POSITIVO: REGLA DEL REPOSITORIO
                    renglon = BuscarRenglon(listaSintactico[punteroSintactico]);
                    columna = BuscarColumna(listaTokens[punteroLexico]._Token);

                    regla = MatrizSintactica[renglon, columna];
                    if (regla > 0)
                    {
                        // Llamado insersion de reglas
                        InsertarRegla(regla);
                        VerificarRecuperacion();
                    }
                    else
                    {
                        if (intentosRecuperar < 1)
                        {
                            tipoRecuperacion = TipoRecuperacion.Sobra;
                        }
                        NuevoError(listaTokens, ref revision, regla);
                    }
                }

            } while (revision != true);
        }
        #endregion

        #region INSERTAR REGLA
        private void InsertarRegla(int regla)
        {
            int i = 0;

            do
            {
                listaSintactico[punteroSintactico] = RepositorioReglas[regla - 1, i];
                punteroSintactico++;
                i++;

            } while (RepositorioReglas[regla - 1, i] != -200);
            punteroSintactico--;
        }
        #endregion

        #region BUSCAR COLUMNA
        private int BuscarColumna(int token)
        {

            switch (token)
            {
                case -1:   return 0; // ID
                case -2:   return 1; // INT
                case -3:   return 2; // DEC
                case -4:   return 3; // CAD
                case -6:   return 4; // +
                case -7:   return 5; // -
                case -8:   return 6; // *
                case -9:   return 7; // /
                case -10:  return 8;  // %
                case -12:  return 9;  // ++
                case -13:  return 10; // --
                case -14:  return 11; // =
                case -30:  return 12; // &&
                case -31:  return 13; // ||
                case -45:  return 14; // ==
                case -46:  return 15; // !=
                case -47:  return 16; // >
                case -48:  return 17; // <
                case -49:  return 19; // <
                case -50:  return 19; // <=
                case -59:  return 20; // var
                case -62:  return 21; // (
                case -63:  return 22; // )
                case -64:  return 23; // {
                case -65:  return 24; // }
                case -66:  return 25; // [
                case -67:  return 26; // ]
                case -68:  return 27; // ;
                case -69:  return 28; // :
                case -70:  return 29; // ,
                case -71:  return 30; // .
                case -72:  return 31; // string
                case -73:  return 32; // number
                case -74:  return 33; // boolean
                case -84:  return 34; // void
                case -76:  return 35; // static
                case -77:  return 36; // public
                case -78:  return 37; // private
                case -79:  return 38; // protected
                case -88:  return 39; // class
                case -89:  return 40; // extends
                case -95:  return 41; //if
                case -96:  return 42; // else
                case -98:  return 43; // switch
                case -99:  return 44; //case
                case -100: return 45; // break
                case -101: return 46; // default
                case -102: return 47; // for
                case -104: return 48; // while
                case -105: return 49; // do
                case -107: return 50; //return
                case -115: return 51; //true
                case -116: return 52; //false
                case -119: return 53; //import
                case -200: return 54; //$
                default:
                    throw new Exception("Error de logica");

            }
        }
        #endregion

        #region BUSCAR RENGLON
        private int BuscarRenglon(int regla)
        {
            switch (regla)
            {
                case 1000: return 0;  //  0. S 
                case 1001: return 1;  //  1. SCRIPT
                case 1002: return 2;  //  2. LIBRERIAS
                case 1003: return 3;  //  3. LIB 
                case 1004: return 4;  //  4. lib1
                case 1005: return 5;  //  5. CLASES 
                case 1006: return 6;  //  6. CLASE
                case 1007: return 7;  //  7. CLASES1
                case 1008: return 8;  //  8. HERENCIA
                case 1009: return 9;  //  9. MIEMBROS
                case 1010: return 10; // 10. MIEMBRO
                case 1011: return 11; // 11. MIEMBRO1
                case 1012: return 12; // 12. METATRIPCON
                case 1013: return 13; // 13. ACCESO
                case 1014: return 14; // 14. TIPO_DATO
                case 1015: return 15; // 15. PARAM
                case 1016: return 16; // 16. PARAMS
                case 1017: return 17; // 17. TIPO_METODO
                case 1018: return 18; // 18. DECLARACION_PROPIEDAD
                case 1019: return 19; // 19. EXP
                case 1020: return 20; // 20. FACTOR
                case 1021: return 21; // 21. TERMINO
                case 1022: return 22; // 22. OP.ARITMETICO
                case 1023: return 23; // 23. COMPLEMENTO_DECLARACION
                case 1024: return 24; // 24. SENTENCIAS
                //case 1024: return 24; // 24. OP.RELACIONAL
                //case 1025: return 25; // 25. SENTENCIA
                default:
                    throw new Exception("Error de logica");

            }
        }
        #endregion


        /// <summary>
        /// NUEVO ERROR
        /// </summary>
        /// <param name="listaTokens"></param>
        /// <param name="revision"></param>
        /// <param name="tipo"></param		regla	119	int

        public void NuevoError(List<Token> listaTokens, ref bool revision, int tipo)
        {
            error = true;
            var nuevoError = ManejoErrores(tipo, listaTokens[punteroLexico]._Linea);
            listaError.Add(nuevoError);

            Recuperar(ref revision, ref tipoRecuperacion);
        }


        /// <summary>
        ///  VERIFAR RECUPERACION
        /// </summary>
        private void VerificarRecuperacion()
        {
            if (tipoRecuperacion != TipoRecuperacion.Ninguna)
            {
                tipoRecuperacion = TipoRecuperacion.Ninguna;
                intentosRecuperar = 0;
            }
        }


        /// <summary>
        ///  MANEJO DE ERRORES
        /// </summary>
        /// <param name="error"></param>
        /// <param name="linea"></param>
        /// <returns></returns>
        private Error ManejoErrores(int error, int linea)
        {
            string mensajeError = "";
            switch (error)
            {
                case 1:  // cuando son terminales 
                    mensajeError = "se esperaba el simbolo: " + listaSintactico[punteroSintactico]; break;
                case -600: mensajeError = "R1000. Se esperaba iniciar script.";          break;
                case -601: mensajeError = "R1001. Se esperaba script. ";                 break;
                case -602: mensajeError = "R1002. Se esperaba librerias.";               break;
                case -603: mensajeError = "R1003. Se esperaba -119 [import]";            break;
                case -604: mensajeError = "R1004. Se esperaba -4 [Cadena] o -68 [;]";    break;
                case -605: mensajeError = "R1005. Se esperaba clases.";                  break;
                case -606: mensajeError = "R1006. Se esperaba clase.";                   break;
                case -607: mensajeError = "R1007. Se esperaba clase1";                   break;
                case -608: mensajeError = "R1008. Se esperaba estructura de herencia.";  break;
                case -609: mensajeError = "R1009. Se esperaba miembros de clase.";       break;
                case -610: mensajeError = "R1010. Se esperaba miembro.";                 break;
                case -611: mensajeError = "R1011. Se esperaba miembro1";                 break;
                case -612: mensajeError = "R1012. Se esperaba metatripcon.";             break;
                case -613: mensajeError = "R1013. Se se esperaba acceso.";               break;
                case -614: mensajeError = "R1014. Se esperaba tipo_dato.";               break;
                case -615: mensajeError = "R1015. Se esperaba parametro [param].";       break;
                case -616: mensajeError = "R1016. Se esperaba parametro [params].";      break;
                case -617: mensajeError = "R1017. Se esperaban tipo_metodo.";            break;
                case -618: mensajeError = "R1018. Se esperaba declaracion de propiedad.";break;
                case -619: mensajeError = "R1019. Formato incorrecto de expresion.";     break;
                case -620: mensajeError = "R1020. Formato incorrecto de factor.";        break;
                case -621: mensajeError = "R1021. Formato incorrecto de termino.";       break;
                case -622: mensajeError = "R1022. Formato incorrecto de op_aritmetico."; break;
                case -623: mensajeError = "R1023. Se esperaba complemento declaracion."; break;
                case -624: mensajeError = "R1024. Se esperaban sentencias.";             break;

                default:
                    break;
            }
            return new Error() { codigo = error, mensajeError = mensajeError, tipo = tipoError.Sintactico, Linea = linea };

        }



        /// <summary>
        ///  RECUPERAR
        /// </summary>
        /// <param name="revision"></param>
        /// <param name="tipo"></param>
        private void Recuperar(ref bool revision, ref TipoRecuperacion tipo)
        {
            if (intentosRecuperar > 3)
            {
                tipo = TipoRecuperacion.NoMas;
            }

            intentosRecuperar++;

            switch (tipo)
            {
                case TipoRecuperacion.Ninguna:
                    punteroSintactico--;
                    tipo++;
                    break;
                case TipoRecuperacion.Falta:
                    punteroSintactico--;  // muevo sintactio
                    if (intentosRecuperar == 3)
                    {
                        punteroSintactico += 3;
                        tipo = TipoRecuperacion.Diferentes;
                    }
                    break;
                case TipoRecuperacion.Sobra:
                    if (intentosRecuperar == 3)
                    {
                        punteroLexico -= 2;
                        tipo = TipoRecuperacion.Falta;
                    }
                    punteroLexico++;  // muevo lexico
                    break;
                case TipoRecuperacion.Diferentes:
                    punteroLexico++;   // muevo ambos
                    punteroSintactico--;
                    break;

                default:
                    revision = true;
                    MessageBox.Show(@"Analisis Sintactico no se recupero, paro fulminante");
                    break;
            }
        }
        
    }

}



