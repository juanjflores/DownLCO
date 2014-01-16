using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Timers;

namespace DowLCO
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer timer2;            
             
        string pathDown = "ftp://ftp2.sat.gob.mx/agti_servicio_ftp/cfds_ftp/LCO_";
        //directorio guardará los archivos
        string rutaXMLSat = @"C:\DowLCO\";        
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {   
            //**************  Crea el directorio donde se descargaran los archivos
           // string rutaXMLSat = @"C:\DowLCO\";
            if (!Directory.Exists(rutaXMLSat)) {
                Directory.CreateDirectory(rutaXMLSat);
            }                   
            
            //Función download files
            DownloadFiles();
            //valida cada 2 segundos si las baderas han cambiado de estatus (descarga de archivos)
            timer2 = new System.Timers.Timer(10000);
            // Hook up the Elapsed event for the timer.
            timer2.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            // Set the Interval to 2 seconds (2000 milliseconds).
            timer2.Interval = 2000;
            timer2.Enabled = true;            
        }            
                
        public void DownloadFiles(){
            //Get fecha actual
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");            
           
            //Download file de ftp
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed1);
            webClient.DownloadFileAsync(new Uri(pathDown + fechaActs + "_1.XML.gz"), rutaXMLSat + "LCO_" + fechaActs + "_1.XML.gz");           

            WebClient webClient1 = new WebClient();
            webClient1.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed2);
            webClient1.DownloadFileAsync(new Uri(pathDown + fechaActs + "_2.XML.gz"), rutaXMLSat + "LCO_" + fechaActs + "_2.XML.gz");

            WebClient webClient2 = new WebClient();
            webClient2.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed3);
            webClient2.DownloadFileAsync(new Uri(pathDown + fechaActs + "_3.XML.gz"), rutaXMLSat + "LCO_" + fechaActs + "_3.XML.gz");
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {   //valida si las banderas han cambiado de estatus
            //para continuar 
            if ((Form1.downEnd1 == 1) && (Form1.downEnd2 == 1) && (Form1.downEnd3 == 1))
            {
                timer2.Enabled = false;
                //llama leerXml
                LeerXML();
            }
        }

        public static void LeerXML() {
            //************************Leer direcctorio donde se encuentran los xml descargados
            int numAr = 0;
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
           
            //************************* Crear directorio txt donde se almacenara info
            string pathLCO = "C:\\DowLCO\\LCO.txt";
            if (File.Exists(pathLCO))
            {
                System.IO.File.Delete(pathLCO);
            }
            //Crea directorio
            StreamWriter arch = new StreamWriter(pathLCO, true, Encoding.ASCII);

            //Recorrer los archivos                       
            for (numAr = 1; numAr <= 3; numAr++)
            {
                //***********************Quitar firma   
                Process process = new Process();                
                process.StartInfo = new System.Diagnostics.ProcessStartInfo(@"C:\Windows\System32\cmd.exe", "/C C:\\OpenSSl\\bin\\openssl.exe smime -decrypt -verify -inform DER -in \"C:\\DowLCO\\LCO_"+fechaActs+"_" + numAr + ".xml\" -noverify -out \"C:\\DowLCO\\LCO_"+fechaActs+"_" + numAr + "_L.xml\"");
                process.Start();
                process.WaitForExit();
            }

            //************************Leer file XML            
            for (numAr = 1; numAr <= 3; numAr++)
            { 
                string rfc = "";
                string noCertificado = "";
                string status = "";
                string FechaIni = "";
                string FechaFin = "";
                string validezOblig = "";
                string rutaXMLSat = @"C:\DowLCO\";
                  
                //************ Leer el xml                           
                string pathXml = rutaXMLSat + "LCO_"+fechaActs +"_" + numAr + "_L.xml";

                XmlReader xmlReader = XmlReader.Create(new StreamReader(pathXml));
                while (xmlReader.Read())
                {
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "lco:Contribuyente")) //Si es este nodo 
                    {  //Get Datos RFC
                        if (xmlReader.HasAttributes)
                            rfc = xmlReader.GetAttribute("RFC");
                    }
                    else
                    {
                        if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "lco:Certificado")) //Si es este nodo
                        {    //Get Datos restantes
                            if (xmlReader.HasAttributes)
                                validezOblig = xmlReader.GetAttribute("VlalidezObligaciones");
                            noCertificado = xmlReader.GetAttribute("noCertificado");
                            status = xmlReader.GetAttribute("EstatusCertificado");
                            FechaIni = xmlReader.GetAttribute("FechaInicio");
                            FechaFin = xmlReader.GetAttribute("FechaFinal");
                        }
                        else //Si lleva al final del nodo Contribuyente
                        {
                            if ((xmlReader.NodeType == XmlNodeType.EndElement) && (xmlReader.Name == "lco:Contribuyente"))
                            {
                                //Almaen y da formato en txt
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

        private void Completed1(object sender, AsyncCompletedEventArgs e)
        {   
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
            string rutaXMLSat = @"C:\DowLCO\";
            //Extrae arxchivo .gz
            Extrgz(rutaXMLSat + "LCO_" + fechaActs + "_1.XML.gz");
            //Una vez finalizado cambia status de bandera
            Form1.downEnd1 = 1;            
        }
        private void Completed2(object sender, AsyncCompletedEventArgs e)
        {
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
            string rutaXMLSat = @"C:\DowLCO\";
            //Extrae arxchivo .gz
            Extrgz(rutaXMLSat + "LCO_" + fechaActs + "_2.XML.gz");
            //Una vez finalizado cambia status de bandera
            Form1.downEnd2 = 1;            
        }
        private void Completed3(object sender, AsyncCompletedEventArgs e)
        {
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
            string rutaXMLSat = @"C:\DowLCO\";
            //Extrae arxchivo .gz
            Extrgz(rutaXMLSat + "LCO_" + fechaActs + "_3.XML.gz");
            //Una vez finalizado cambia status de bandera
            Form1.downEnd3 = 1;
        }         
       
        //Función extraer archivos .gz
        private string Extrgz(string infile)
        {
            string dir = Path.GetDirectoryName(infile);
            string decompressionFileName = dir +"\\"+ Path.GetFileNameWithoutExtension(infile);
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
    }
}
