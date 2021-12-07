﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    class Conexion
    {

        // Metodo Conectar
        private static OleDbConnection cnx = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; DATA Source = BLinkDB.accdb;");

        public static string[] Nombre;
        private static byte[] Rostro;
        /*public static string[] Apellidos;
        public static string[] FechaNacimiento;
        public static string[] Direccion;
        public static string[] Email;
        public static string[] Telefono;
        public static string[] Cedula;*/
        public static List<byte[]> ListadoRostros = new List<byte[]>();
        public static int TotalRostros;

        public static bool GuardarImagen(string Nombre, byte[] Imagen)
        {
            cnx.Open();
            OleDbCommand cmd = new OleDbCommand("INSERT INTO Rostros (Nombre, Imagen) Values ('" + Nombre + "',?);");
            OleDbParameter parImagen = new OleDbParameter("@Imagen", OleDbType.VarBinary, Imagen.Length);
            parImagen.Value = Imagen;
            cmd.Parameters.Add(Imagen);
            int Resultado = cmd.ExecuteNonQuery();
            cnx.Close();

            return Convert.ToBoolean(Resultado);
        }

        public static DataTable Consultar(DataGridView DATA)
        {
            cnx.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Rostros;", cnx);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DATA.DataSource = dt;
            int Cont = dt.Rows.Count;
            Nombre = new string[Cont];

            for (int i=0; i<Cont; i++)
            {
                Nombre[i] = dt.Rows[i]["Nombre"].ToString();
                Rostro = (byte[])dt.Rows[i]["Imagen"];
                ListadoRostros.Add(Rostro);
            }

            try
            {
                DATA.Columns[0].Width = 60;
                DATA.Columns[1].Width = 160;
                DATA.Columns[2].Width = 160;

                for (int i = 0; i < Cont; i++)
                {
                    DATA.Rows[i].Height = 110;
                }

            } catch
            {
            }
            TotalRostros = Cont;
            //cnx.Close();
            return dt;
        }

        //***************

        public static byte[] ConvertImgToBinary(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            MemoryStream Memoria = new MemoryStream();
            bmp.Save(Memoria, ImageFormat.Bmp);

            byte[] imagen = Memoria.ToArray();

            return imagen; //arreglo de binario a la imagen
        }

        public static Image ConvertBinaryToImg(int C)
        {
            Image Imagen;
            byte[] img = ListadoRostros[C];
            MemoryStream Memoria = new MemoryStream(img);
            Imagen = Image.FromStream(Memoria);
            Memoria.Close();
            return Imagen;
        }
    }
}
