using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Net.Sockets;


using Org.BouncyCastle;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.X509;

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;


namespace LeerXml
{
    public partial class frm_principal : Form
    {
        conexion.conexionBD conexBD = new conexion.conexionBD();
        private readonly Encoding _encoding;
        private readonly IBlockCipher _blockCipher;
        private PaddedBufferedBlockCipher _cipher;
        private IBlockCipherPadding _padding;

        public frm_principal()
        {
            InitializeComponent();
        }

        private void btn_cargar_Click(object sender, EventArgs e)
        {
            //**************Guardar en un txt la LCO****************
            //******************************************************
            DateTime fechaAct = DateTime.Today;
            string fechaActs = fechaAct.ToString("yyy-MM-dd");
            //Nombre del archivo 
            XmlDocument xDoc = new XmlDocument();
            //Obtiene la ruta del proyecto
            string rutaAbs = Application.StartupPath;
            //archivo con fecha actual
            //string pathXml = rutaAbs + "\\LCO_"+fechaActs+".xml";
            string pathXml = rutaAbs + "\\LCO_2013-09-23.xml";
            try
            {
                xDoc.Load(pathXml);
            }
            catch (Exception exp) { }
            
            XmlElement LCO = xDoc.DocumentElement;
            string fecha = "";
            fecha = LCO.Attributes["Fecha"].Value;
            XmlNamespaceManager lco = new XmlNamespaceManager(xDoc.NameTable);
            string rutalco = Application.StartupPath;
            rutalco = rutalco + "\\LCO.xsd";
            lco.AddNamespace("lco", rutalco);
            lco.AddNamespace("lco", "Contribuyente");
            lco.AddNamespace("lco", "Certificado");
            string noCertificado = "";
            string status = "";
            string FechaIni = "";
            string FechaFin = "";
            string validezOblig = "";


            //Eliminar Archivo 
            string rutaArc = Application.StartupPath;
            string rutaArc1 = rutaArc + "\\Dlco.txt";
            if(File.Exists(rutaArc1))
            {
            System.IO.File.Delete(rutaArc1);
            }
            MessageBox.Show("msj");
            //Crea directorio
            StreamWriter arch = new StreamWriter(rutaArc1, true, Encoding.ASCII);

            MessageBox.Show("msj2");
            //Obtener contribuyentes //entonces dile
           // XmlNodeList contribuyentes = xDoc.SelectNodes(@"/lco:LCO/lco:Contribuyente", lco);
            XmlNodeList contribuyentes = xDoc.GetElementsByTagName("lco:Contribuyente");

            //*******ALMACENA EN TXT
            
            foreach (XmlNode contrb in contribuyentes)
            {
                string rfc = "";
                rfc = contrb.Attributes["RFC"].Value;
                //Lee archivo xml
               foreach (XmlNode cert in contrb.ChildNodes )
                {
                    noCertificado = cert.Attributes["noCertificado"].Value;
                    status = cert.Attributes["EstatusCertificado"].Value;
                    FechaIni = cert.Attributes["FechaInicio"].Value;
                    FechaFin = cert.Attributes["FechaFinal"].Value;
                    validezOblig = cert.Attributes["ValidezObligaciones"].Value;
                    arch.WriteLine(noCertificado + "|" + FechaIni + "|" + FechaFin + "|" + rfc + "|" + status + "|" + validezOblig);
                }
            }
            arch.Close();
            MessageBox.Show("Descargado Correctamente");


           //**********ALMACENA EN BASE DE DATOS
            /*
            //Truncate table
            string queryDele = "truncate table CatLcoP";
            try
            {
                conexBD.InsertaSql(queryDele);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema al Limpiar");
            }

            foreach (XmlNode contrb in contribuyentes)
            {
                string rfc = "";
                rfc = contrb.Attributes["RFC"].Value;
                //Lee archivo xml
                foreach (XmlNode cert in contrb.ChildNodes)
                {
                    noCertificado = cert.Attributes["noCertificado"].Value;
                    status = cert.Attributes["EstatusCertificado"].Value;
                    FechaIni = cert.Attributes["FechaInicio"].Value;
                    FechaFin = cert.Attributes["FechaFinal"].Value;
                    validezOblig = cert.Attributes["ValidezObligaciones"].Value;
                    //Save en BD
                    string query = "INSERT INTO CatLcoP(vchrfc,vchnumcer,dfechainicio,dfechafin,vchstatus,validezOblig) VALUES ('" + rfc + "', '" + noCertificado + "','" + FechaIni + "','" + FechaFin + "','" + status + "', '"+validezOblig+"')";
                    //MessageBox.Show(query);
                    try
                    {   //Ejecuta query
                        conexBD.InsertaSql(query);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Problema al Insertar");
                    }
                }
            }
            MessageBox.Show("Descargado Correctamente");
            */

            /*
                string rfcbd = "";
                rfcbd = contrb.Attributes["RFC"].Value;
                XmlNodeList certificado = xDoc.GetElementsByTagName("lco:Certificado");
                //Truncate table
                string queryDele = "truncate table Lcop";
                try
                {
                    conexBD.InsertaSql(queryDele);
                }
                catch(Exception ex) {
                    MessageBox.Show("Problema al Limpiar");
                }

                foreach ( XmlNode  cert in certificado) 
                {
                    noCertificado = cert.Attributes["noCertificado"].Value;
                    status = cert.Attributes["EstatusCertificado"].Value;
                    FechaIni = cert.Attributes["FechaInicio"].Value;
                    FechaFin = cert.Attributes["FechaFinal"].Value;

                    //MessageBox.Show(rfc + "-" + noCertificado);
                    //save
                    //Guarda en BD
                    string query = "INSERT INTO LcoP(vchRFC,vchnumcer,dfechaIni,dfechaFin,vchEstatus) VALUES ('" + rfc + "', '" + noCertificado + "','" + FechaIni + "','" +  FechaFin + "','" + status + "' )";
                    //MessageBox.Show(query);
                    try
                    {
                        conexBD.InsertaSql(query);
                       // MessageBox.Show("Guardado");
                    }
                    catch(Exception ex) {
                        MessageBox.Show("Problema al Insertar");
                    }
                    MessageBox.Show("finalizado");
                  }*/
        }

        public class Contr
        {
            public Contribuyente Contribuyentes;
        }

        public class Contribuyente
        {
            public string RFC;
            public string ValidezObligaciones;
            public string EstatusCertificado;
            public string noCertificado;
            public string FechaFinal;
            public string FechaInicio;
        }

        public Contr ContrObject(string xml)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Contr));
                stream = new StringReader(xml);
                reader = new XmlTextReader(stream);
                return (Contr)serializer.Deserialize(reader);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (stream != null) stream.Close();
                if (reader != null) reader.Close();
            }
        }

        private void btn_leer_Click(object sender, EventArgs e)
        {
            //leer y guardar en un archivo txt
            int[] fibarray = new int[] { 0, 1, 2, 3, 5, 8, 13 };
            string rutaArc= Application.StartupPath;
            string rutaArc1 = rutaArc+ "\\Dlco.txt";

            System.IO.File.Delete(rutaArc1);
            MessageBox.Show("Eliminado");
            StreamWriter arch = new StreamWriter(rutaArc1, true, Encoding.ASCII);
            /*
            //Eliminar archivo
            try 
            {
                File.Delete(rutaArc1);
                MessageBox.Show("Eliminado");
            }catch(Exception exp){
                MessageBox.Show("Problema al Eliminar");
            }*/

            //Creando el archivo

            foreach (int i in fibarray)
            {
                MessageBox.Show(Convert.ToString(i));
                arch.Write(Convert.ToString(i)+"|");
                //System.Console.WriteLine(i);
            }
            arch.Close();
            MessageBox.Show("Finalizado");


            XmlDocument xDoc = new XmlDocument();
            string rutaAbs = Application.StartupPath;//System.IO.Directory.GetCurrentDirectory();
            string pathXml = rutaAbs + "\\56E534B6-B5B9-4959-9102-CD9376606C5A.xml";
            MessageBox.Show(pathXml);
            try
            {
                xDoc.Load(pathXml);
            }
            catch (Exception exp) {
            }

            XmlElement comprobante = xDoc.DocumentElement;
            string folio = "";
            folio = comprobante.Attributes["folio"].Value;

            MessageBox.Show(folio);
            XmlNamespaceManager cfdi = new XmlNamespaceManager(xDoc.NameTable);
            cfdi.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            XmlNode Emisor = (XmlElement)xDoc.SelectSingleNode(@"/cfdi:Comprobante/cfdi:Emisor", cfdi);
            string rfc_emisor = Emisor.Attributes["rfc"].Value;
            MessageBox.Show(rfc_emisor);
        }

        private void btn_otro_Click(object sender, EventArgs e)
        {
            //Leer el xml
            string rutaXml = Application.StartupPath;
            string pathXml = rutaXml + "\\LCO_2010-11-29.xml";
           //tring pathTxt = rutaXml + "\\LCO_2010-11-29.txt";
            string pathTxt = rutaXml + "\\LCO_2010-11-29.txt";
           // string pathTxt = rutaXml + "\\LCO_2010-11-29_sinFirma.txt";


            //**********************************
            XmlDocument xDoc = new XmlDocument();
            //Obtiene la ruta del proyecto
            string rutaAbs = Application.StartupPath;
            pathXml = rutaAbs + "\\LCO_2010-11-29.xml";
            try
            {
                xDoc.Load(pathXml);
            }
            catch (Exception exp) { }

            //Lee el xml y se convierte a String
            StringWriter Sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(Sw);
            xDoc.WriteTo(tx);
            string xml = Sw.ToString();
            MessageBox.Show(xml);


           System.IO.StreamReader myFile = new System.IO.StreamReader(pathTxt);
            string myString = myFile.ReadToEnd();
            MessageBox.Show(myString);
            myFile.Close();

            string result = DecryptString("", 8, myString);
            MessageBox.Show(result);


            //Convertir de xml a txt
            //File.Move(@"C:\Dir1\SomeFile.txt", @"C:\Dir1\RenamedFileName.txt")
            //*********************************************************************
       /*
   var lines = File.ReadAllLines(pathTxt, Encoding.Default).ToList();
   //Una vez en memoria es muy rápido buscar lo que deseas:
   var line = lines.FirstOrDefault(p => p.StartsWith("<"));
   MessageBox.Show(line);

//**********************************
   //leer el txt y
       
   System.IO.StreamReader myFile =new System.IO.StreamReader(pathTxt);

   string myString = myFile.ReadToEnd();
   myFile.Close();

  int posic = myString.IndexOf("<");
  myString.Remove(0, 62);

   //int posic2 = 
   MessageBox.Show(Convert.ToString(posic));
   MessageBox.Show(myString);

   //Reem Linea por linea */
//****************************************************/
            //Leer txt
            StreamReader stbxml = new StreamReader(pathTxt);
            string sLine = "";
            string xmltxt = "";
            //Leer linea por linea en un streamreader
            ArrayList arrText = new ArrayList();
            while (sLine != null)
            {
                sLine = stbxml.ReadLine();
                if (sLine != null)
                   arrText.Add(sLine);
                   MessageBox.Show(sLine);
                   xmltxt = xmltxt + sLine;
            }
            stbxml.Close();
            MessageBox.Show(xmltxt);
//*******************************************
        }

        public string DecryptString(string inputString, int dwKeySize, string xmlString)
        {
            //TODO: Add Proper Exception Handlers
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize);
            rsaCryptoServiceProvider.FromXmlString(xmlString);
            int base64BlockSize = ((dwKeySize / 8) % 3 != 0) ? (((dwKeySize / 8) / 3) * 4) + 4 : ((dwKeySize / 8) / 3) * 4;
            int iterations = inputString.Length / base64BlockSize;

            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < iterations; i++)
            {
                byte[] encryptedBytes = Convert.FromBase64String(
                     inputString.Substring(base64BlockSize * i, base64BlockSize));
                Array.Reverse(encryptedBytes);
                arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
            }
            return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
        }

        private void btn_conver_Click(object sender, EventArgs e)
        {
            byte[] bytesCert;
            string pathCer = Application.StartupPath;
            pathCer = pathCer + "\\LCO_2010-11-29.xml";
            //MessageBox.Show(pathCer);
            //Pasa xml a Bytes
            bytesCert=  GetBytesFromFile(pathCer);

            //Exrae del xml la firma
            byte[] nuevo = ExtractEnvelopedData(bytesCert);
            //convert to string
            string s = Encoding.UTF8.GetString(nuevo, 0, nuevo.Length);
            MessageBox.Show(s);

            //path 
            string patnewXml = Application.StartupPath;
            patnewXml = patnewXml+"\\new.txt";

            //Guardar en un txt 
            StreamWriter fichero = new StreamWriter(patnewXml);
            fichero.Write(s);
            fichero.Close();
            MessageBox.Show("guardado");

            // Obtner el 
            X509Certificate2 x509 = new X509Certificate2();
            //Create X509Certificate2 object from .cer file. 
            byte[] rawData = ReadFile();
            x509.Import(rawData);

            //Add the certificate to a X509Store.
            X509Store store = new X509Store();
            store.Open(OpenFlags.MaxAllowed);
            store.Add(x509);
            store.Close();
            //verificar
            if(verificar(bytesCert, x509)){
                MessageBox.Show("prueba");
            }
        }

        //*****************************************
        //Validar 
        public bool verificar(byte[] signature, X509Certificate2 certificado)
        {

            if (signature == null)
                throw new ArgumentNullException("signature");
            if (certificado == null)
                throw new ArgumentNullException("certificate");

            // decode the signature 
            SignedCms  verifyCms = new SignedCms();
            verifyCms.Decode(signature);

            // verify it
            try
            {
                verifyCms.CheckSignature(new X509Certificate2Collection(certificado), false);
                return true;
            }
            catch (CryptographicException)
            {
                return false;
            }

            return false;
        }
        
        //Leer firma
        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
        
        // Leer el certificado pasarlo a bytes
        internal static byte[] ReadFile()
        {
            string pathCer = Application.StartupPath;
            pathCer = pathCer + "\\1.txt";

            string fileName = pathCer;
            FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }

        //Exrae del xml la firma
        public static byte[] ExtractEnvelopedData(byte[] signature)
        {
            if (signature == null)
                throw new ArgumentNullException("signature");

            // decode the signature
            SignedCms cms = new SignedCms();
            cms.Decode(signature);

            if (cms.Detached)
                throw new InvalidOperationException("Cannot extract enveloped content from a detached signature.");
            return cms.ContentInfo.Content;
        }

        private void btn_castle_Click(object sender, EventArgs e)
        {
            string pathCer = Application.StartupPath;
            pathCer = pathCer + "\\LCO_2013-09-23.xml";
            NetworkStream GetStreamData = new NetworkStream(pathCer);  //new GetStreamdata;

            Org.BouncyCastle.X509.X509Certificate cert = new X509CertificateParser().ReadCertificate(GetStreamData());
            byte[] key = {};
            byte[] initVector = {};
            
            KeyParameter keyParam = new KeyParameter(key);
            ICipherParameters param = new ParametersWithIV(keyParam, initVector);

            IBlockCipherPadding padding = new Pkcs7Padding();
            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), padding);
            cipher.Reset();
            cipher.Init(false, param);

            byte[] fileBytes = Convert.FromBase64String(pathCer);//encryptedDataString);
            byte[] decrypted = new byte[cipher.GetOutputSize(fileBytes.Length)];
            int l = cipher.ProcessBytes(fileBytes, 0, fileBytes.Length, decrypted, 0);
            l += cipher.DoFinal(decrypted, l);
            

        }
        //Bouncy Castle
        /*
      public string Decrypt(string cipher, string key)
      {   byte[] result = BouncyCastleCrypto(false, Convert.FromBase64String(cipher), key);
          return _encoding.GetString(result);
      }
      
      private byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input, string key)
      {
          try
          {
              _cipher = _padding == null ? new PaddedBufferedBlockCipher(_blockCipher) : new PaddedBufferedBlockCipher(_blockCipher, _padding);
              byte[] keyByte = _encoding.GetBytes(key);
              _cipher.Init(forEncrypt, new KeyParameter(keyByte));
              return _cipher.DoFinal(input);
          }
          catch (Org.BouncyCastle.Crypto.CryptoException ex)
          {
            throw new CryptoException(ex);
          }
      }*/

    }
}
