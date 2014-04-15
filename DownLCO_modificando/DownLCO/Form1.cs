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

namespace DownLCO
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer timer2;
       
        //Declaración de variables
        //definen el estatus, si los archivos han sido descargados
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
            //directorio guardará los archivos
            Form1.rutaFtpSat = ConfigurationManager.AppSettings["RUTAFTPSAT"];
            Form1.pathDown = ConfigurationManager.AppSettings["DIRECTORIO"];
            Form1.numArc = Convert.ToInt16((ConfigurationManager.AppSettings["NUMARCHIVOS"]));
            Form1.PathArchTxt = ConfigurationManager.AppSettings["NAMETXTLCO"];

            //**************  Crea el directorio donde se descargaran los archivos           
            if (!Directory.Exists(Form1.pathDown))
            {
                Directory.CreateDirectory(Form1.pathDown);
            }

            DownloadFiles();         
            //Extrae los archivos descargados
            ExtraerFiles();
            LeerXML();

        }

        public void DownloadFiles()
        {
            //Get fecha actual
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
            // Declara y usa la variable local ciclos
            for (int ciclos = 1; ciclos <= Form1.numArc; ciclos++)
            {
                //string numb = Thread.CurrentThread.Name;
                try
                {
                    WebClient webClient1 = new WebClient();
                    webClient1.DownloadFile(new Uri(Form1.rutaFtpSat + fechaActs + "_" + ciclos + ".XML.gz"), Form1.pathDown + "LCO_" + fechaActs + "_" + ciclos + ".XML.gz");
                }
                catch { 

                }
              //  webClient1.DownloadFileAsync(new Uri(Form1.rutaFtpSat + fechaActs + "_" + ciclos + ".XML.gz"), Form1.pathDown + "LCO_" + fechaActs + "_" + ciclos + ".XML.gz");                
            }
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {   //valida si las banderas han cambiado de estatus
            //para continuar 
            if ((Form1.downEnd1 == 1) && (Form1.downEnd2 == 1) && (Form1.downEnd3 == 1) && (Form1.downEnd4 == 1))
            {
                timer2.Enabled = false;
                //llama leerXml
                LeerXML();
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
            string rutaXMLSat = @"C:\DowLCO\";
            //Extrae arxchivo .gz
            Extrgz(Form1.pathDown + "LCO_" + fechaActs + "_1.XML.gz");
            //Una vez finalizado cambia status de bandera
            Form1.downEnd1 = 1;
        }

        public void ExtraerFiles() {
            IList listaArvchivos = Directory.GetFiles(Form1.pathDown).ToList();
            foreach (string archivo in listaArvchivos) {
                string nameFile = archivo;
                Extrgz(nameFile);
            }

        }

        public string Extrgz(string infile)
        {
            string dir = Path.GetDirectoryName(infile);
            string decompressionFileName = dir + "\\" + Path.GetFileNameWithoutExtension(infile);
            using (GZipStream instream = new GZipStream(File.OpenRead(infile), CompressionMode.Decompress))// ArgumentException...
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

        //
        public static void LeerXML()
        {
            //************************Leer direcctorio donde se encuentran los xml descargados
            int numAr = 0;
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");

            //************************* Crear directorio txt donde se almacenara info
            string pathLCO = Form1.PathArchTxt; // "C:\\DowLCO\\LCO.txt";
            if (File.Exists(pathLCO))
            {
                System.IO.File.Delete(pathLCO);
            }
            //Crea directorio
            StreamWriter arch = new StreamWriter(pathLCO, true, Encoding.ASCII);

            //Recorrer los archivos                       
            for (numAr = 1; numAr <= Form1.numArc; numAr++)
            {
                //***********************Quitar firma   
                Process process = new Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo(@"C:\Windows\System32\cmd.exe", "/C C:\\OpenSSl\\bin\\openssl.exe smime -decrypt -verify -inform DER -in \"C:\\DowLCO\\LCO_" + fechaActs + "_" + numAr + ".xml\" -noverify -out \"C:\\DowLCO\\LCO_" + fechaActs + "_" + numAr + "_L.xml\"");
                process.Start();
                process.WaitForExit();
            }

            //************************Leer file XML            
            for (numAr = 1; numAr <= Form1.numArc; numAr++)
            {
                string rfc = "";
                string noCertificado = "";
                string status = "";
                string FechaIni = "";
                string FechaFin = "";
                string validezOblig = "";

                //************ Leer el xml                           
                string pathXml = pathDown + "LCO_" + fechaActs + "_" + numAr + "_L.xml";

                XmlReader xmlReader = XmlReader.Create(new StreamReader(pathXml));
                while (xmlReader.Read())
                {
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "lco:Contribuyente")) //Si es este nodo 
                    {  //Get Datos RFC
                        if (xmlReader.HasAttributes)
                           rfc = xmlReader.GetAttribute("RFC");
                            byte[] bytes = Encoding.Default.GetBytes(rfc);
                            rfc = Encoding.UTF8.GetString(bytes);

                    }
                    else
                    {
                        if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "lco:Certificado")) //Si es este nodo
                        {    //Get Datos restantes
                            if (xmlReader.HasAttributes)
                                validezOblig = xmlReader.GetAttribute("ValidezObligaciones");
                                noCertificado = xmlReader.GetAttribute("noCertificado");
                                status = xmlReader.GetAttribute("EstatusCertificado");
                                FechaIni = xmlReader.GetAttribute("FechaInicio");
                                FechaFin = xmlReader.GetAttribute("FechaFinal");
                        }
                        else //Si lleva al final del nodo Contribuyente
                        {
                            if ((xmlReader.NodeType == XmlNodeType.EndElement) && (xmlReader.Name == "lco:Certificado"))
                            {
                                //Almacen y da formato en txt
                                arch.WriteLine(noCertificado + "|" + FechaIni + "|" + FechaFin + "|" + rfc + "|" + status + "|" + validezOblig);
                            }
                        }
                    }
                }
            }
            //Close file txt
            arch.Close();
            MessageBox.Show("Descargado Correctamente");
        }
    }
}
