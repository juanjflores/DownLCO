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
using System.Collections;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Ionic.Zip;
using Schematrix;
using System.Net;

namespace DowLCO
{
  /*
   * 09/01/2013
   * De: Descarga listas LCO del SAT del ftp 
   * descompribe las listas
   * Para almacenarlos en un txt global
   * GSM 
   */

    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {     
            //**************  Crea el directorio
            string rutaXMLSat = @"C:\DowLCO\";
            
            if (!Directory.Exists(rutaXMLSat)) {
                Directory.CreateDirectory(rutaXMLSat);
            }
            
           //*********************************Descargar de: ftp://ftp2.sat.gob.mx/agti_servicio_ftp/cfds_ftp/          
            


            //********************* Extraer archivos de .gz 
            Extrgz(@"C:\DowLCO\LCO_2014-01-06_1.XML.gz");
            Extrgz(@"C:\DowLCO\LCO_2014-01-06_2.XML.gz");
            Extrgz(@"C:\DowLCO\LCO_2014-01-06_3.XML.gz");    

            int numAr = 0;
            //Get fecha actual
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");

            //*************************Leer direcctorio donde se encuentran los xml descargados
            
            //Recorrer los archivos                       
             for (numAr = 1; numAr <= 3; numAr++)  {
                  //***********************Quitar firma   
                  Process process = new Process();
                  process.StartInfo = new System.Diagnostics.ProcessStartInfo(@"C:\Windows\System32\cmd.exe", "/C C:\\OpenSSl\\bin\\openssl.exe smime -decrypt -verify -inform DER -in \"C:\\DowLCO\\LCO_2014-01-06_"+ numAr +".xml\" -noverify -out \"C:\\DowLCO\\LCO_2014-01-06_"+ numAr +"_L.xml\"");
                  process.Start();
                  process.WaitForExit();
              }
          
            //************************* Crear directorio txt donde se almacenara info
            string pathLCO = "C:\\DowLCO\\LCO.txt";
            if (File.Exists(pathLCO))
            {
                System.IO.File.Delete(pathLCO);
            }
            //Crea directorio
            StreamWriter arch = new StreamWriter(pathLCO, true, Encoding.ASCII);

            //************************Leer file XML            
            for (numAr = 1; numAr <= 3; numAr++)
            {
                string rfc = "";
                string noCertificado = "";
                string status = "";
                string FechaIni = "";
                string FechaFin = "";
                string validezOblig = "";

                //************ Leer el xml           
                //  string pathXml = "C:\\DowLCO\\LCO_2014-01-06_2_L.xml";
                string pathXml = rutaXMLSat + "LCO_2014-01-06_" + numAr + "_L.xml";

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
                        {    //Get Datos
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
