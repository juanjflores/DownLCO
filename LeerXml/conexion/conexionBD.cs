using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace LeerXml.conexion
{
    class conexionBD
    {
        public static SqlConnection ConexionSqL() {
            SqlConnection cnn = null;
            string sql = "Data Source = PRUEBASPLATAFOR; " + 
                         "Initial Catalog = CFDIv2;"+
                         "Persist Security Info=False; User ID=sa;"+
                         "Password=ASDasd123*";
            cnn = new SqlConnection(sql);
            cnn.Open();
            return cnn;
        }

        public void closeConexion(SqlConnection cnn) {
            cnn.Close();
        }

        public bool InsertaSql(string Query)
        {
            SqlConnection cnn = null;
            cnn = ConexionSqL();

            try
            {

                SqlCommand comando = new SqlCommand(Query, cnn);
                comando.ExecuteNonQuery();

            }
            catch
            {
                return false;
            }
            cnn.Close();
            return true;
        }  
    }
}
