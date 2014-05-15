using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Collections;
using System.Diagnostics;
using System.Xml;
using System.Timers;

/*  Author:  Guadalupe Santiago Morgado / Luis Alberto Cisneros Alvarez
 *  Company: Plataformas
 *  Version: 2.0
 *  
 *  Personal Comments:
 *  Herramienta, permite la descarga de la Lista LCO
 *  ftp://ftp2.sat.gob.mx/agti_servicio_ftp/cfds_ftp/LCO_
 *  Solo definir en config No. de archivos y directorio 
 *  De descarga
 */

namespace DownLCO
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer timer2;
       
        //Declaración de variables, estas definen el estatus, si los archivos han sido descargados.
        private static int downEnd1;
        public static int DownEnd1
        {
            get { return Form1.downEnd1; }
            set { Form1.downEnd1 = value; }
        }
        private static int downEnd2;

        public static int DownEnd2
        {
            get { return Form1.downEnd2; }
            set { Form1.downEnd2 = value; }
        }
        private static int downEnd3;

        public static int DownEnd3
        {
            get { return Form1.downEnd3; }
            set { Form1.downEnd3 = value; }
        }

        private static int downEnd4;
        public static int DownEnd4
        {
            get { return Form1.downEnd4; }
            set { Form1.downEnd4 = value; }
        }

        private static int numArc;
        public static int NumArc
        {
            get { return Form1.numArc; }
            set { Form1.numArc = value; }
        }
        private static string pathDown;

        public static string PathDown
        {
            get { return Form1.pathDown; }
            set { Form1.pathDown = value; }
        }
        private static string PathArchTxt;

        public static string PathArchTxt1
        {
            get { return Form1.PathArchTxt; }
            set { Form1.PathArchTxt = value; }
        }
        private static string rutaFtpSat;

        public static string RutaFtpSat
        {
            get { return Form1.rutaFtpSat; }
            set { Form1.rutaFtpSat = value; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Cargamos las configuraciones del AppConfig.
            Form1.rutaFtpSat = ConfigurationManager.AppSettings["RUTAFTPSAT"];
            Form1.pathDown = ConfigurationManager.AppSettings["DIRECTORIO"];
            Form1.numArc = Convert.ToInt16((ConfigurationManager.AppSettings["NUMARCHIVOS"]));
            Form1.PathArchTxt = ConfigurationManager.AppSettings["NAMETXTLCO"];
            //Creamos el directorio (si no existe), donde se descargaran los archivos.
            if (!Directory.Exists(Form1.pathDown))
            {
                Directory.CreateDirectory(Form1.pathDown);
            }
            //Descargamos los archivos.
            DescargarGZ();         
            //Descomprimimos los archivos descargados.
            ExtraerXML();
            //Leemos los XML's.
            LeerXML();
        }
        //Función para descargar las partes de la LCO comprimidas en archivos en formato .GZ
        public void DescargarGZ()
        {
            //Obtenemos la fecha actual, apartir de la cual se descargara la LCO del día.
            DateTime fechaAct = DateTime.Today;
            //Formateamos la fecha actual.
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
            //Se declara la variable ciclos, la cual se ejecutara hasta cumplir con el numero de archivos.
            for (int ciclos = 1; ciclos <= Form1.numArc; ciclos++)
            {
                try
                {
                    //Abrimos una nueva conexión desde la cual se descargara la parte de la LCO.
                    WebClient webClient1 = new WebClient();
                    webClient1.DownloadFile(new Uri(Form1.rutaFtpSat + fechaActs + "_" + ciclos + ".XML.gz"), Form1.pathDown + "LCO_" + fechaActs + "_" + ciclos + ".XML.gz");
                }
                catch { 

                }
            }
        }
        //Función para extraer los archivos XML del formatoGZ
        public void ExtraerXML()
        {
            IList listaArvchivos = Directory.GetFiles(Form1.pathDown).ToList();
            foreach (string archivo in listaArvchivos) {
                string nameFile = archivo;
                Extrgz(nameFile);
            }

        }
        //Clase ppara extraer los GZ
        public string Extrgz(string infile)
        {
            string dir = Path.GetDirectoryName(infile);
            string decompressionFileName = dir + "\\" + Path.GetFileNameWithoutExtension(infile);
            using (GZipStream instream = new GZipStream(File.OpenRead(infile), CompressionMode.Decompress))
            {
                using (FileStream outputStream = new FileStream(decompressionFileName, FileMode.Append, FileAccess.Write))
                {
                    int bufferSize = 8192, bytesRead = 0;
                    byte[] buffer = new byte[bufferSize];
                    while ((bytesRead = instream.Read(buffer, 0, bufferSize)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            return decompressionFileName;
        }
        //Función para Leer los archivos XML descomprimidos.
        public static void LeerXML()
        {
            //Lectura de la carpeta donde se toman los archivos XML's
            int numAr = 0;
            //Obtenemos la fecha actual
            DateTime fechaAct = DateTime.Today;
            //Formateamos la fecha
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
            //Verificamos si existe el directorio donde se guardara el TXT.
            string pathLCO = Form1.PathArchTxt;
            if (File.Exists(pathLCO))
            {
                System.IO.File.Delete(pathLCO);
            }
            //Creamos el archivo.
            StreamWriter arch = new StreamWriter(pathLCO, true);
            //Leemos cada uno de los archivos con sus firmas.
            for (numAr = 1; numAr <= Form1.numArc; numAr++)
            {
                //Verificamos la firma de cada uno de los archivos, y si es correcta, la eliminamos del archivo XML para poder leerla.  
                Process process = new Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo(@"C:\Windows\System32\cmd.exe", "/C C:\\OpenSSl\\bin\\openssl.exe smime -decrypt -verify -inform DER -in \""+Form1.pathDown + "LCO_" + fechaActs + "_" + numAr + ".xml\" -noverify -out \""+Form1.pathDown + "LCO_" + fechaActs + "_" + numAr + "_L.xml\"");
                process.Start();
                process.WaitForExit();
            }
            //Lectura de los archivos XML, que ya no contienen firma.          
            for (numAr = 1; numAr <= Form1.numArc; numAr++)
            {
                //Inicializacion de las variables
                string rfc = "";
                string noCertificado = "";
                string estatus = "";
                string FechaIni = "";
                string FechaFin = "";
                string validezOblig = "";
                //Declaramos el Path de los archivos XML limpios        
                string pathXml = pathDown + "LCO_" + fechaActs + "_" + numAr + "_L.xml";
                //Declaración del Reader.
                XmlReader xmlReader = XmlReader.Create(new StreamReader(pathXml));
                while (xmlReader.Read())
                {
                    //Lectura de cada uno de los Nodos de Contibuyentes
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "lco:Contribuyente")) //Si es este nodo 
                    {  //Obtenemos el RFC.
                        if (xmlReader.HasAttributes)
                           rfc = xmlReader.GetAttribute("RFC");
                           rfc = Convert.ToString(rfc);
                    }
                    else
                    {
                        //Lectura de los Nodos de Certificado para cada uno de los Contribuyentes.
                        if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "lco:Certificado")) //Si es este nodo
                        {
                            //Obtenemos los datos.
                            if (xmlReader.HasAttributes)
                            //Validez de Obligaciones
                            validezOblig = xmlReader.GetAttribute("ValidezObligaciones");
                            //Número de Certificado
                            noCertificado = xmlReader.GetAttribute("noCertificado");
                            //Estatus del Certificado
                            estatus = xmlReader.GetAttribute("EstatusCertificado");
                            //Fecha de Inicio de la Vigencia del Certificado.
                            FechaIni = xmlReader.GetAttribute("FechaInicio");
                            //Fecha Final de la Vigencia del Certificado
                            FechaFin = xmlReader.GetAttribute("FechaFinal");
                        }
                        else //Si lleva al final del nodo Contribuyente.
                        {
                            if ((xmlReader.NodeType == XmlNodeType.EndElement) && (xmlReader.Name == "lco:Certificado"))
                            {
                                //Generación línea por línea TXT (importante el Encoding en ASCII).
                                arch.WriteLine(noCertificado + "|" + FechaIni + "|" + FechaFin + "|" + rfc + "|" + estatus + "|" + validezOblig, true, Encoding.ASCII);
                            }
                        }
                    }
                }
                //Se borra el archivo GZ.
                System.IO.File.Delete(@Form1.pathDown + "LCO_" + fechaActs + "_" + numAr + ".XML.gz");
                //Se borra el archivo XML con la firma.
                System.IO.File.Delete(@Form1.pathDown + "LCO_" + fechaActs + "_" + numAr + ".xml");
                //Se borra el archivo XML limpio.
                System.IO.File.Delete(@pathXml);
            }
            //Cerramos el TXT
            arch.Close();
            //LCO Descargada correctamente
            //MessageBox.Show("Descargado Correctamente");
        }
    }
}
