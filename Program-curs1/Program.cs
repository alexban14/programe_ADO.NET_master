using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

namespace Program_curs1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string conString = "Server=AN515-45\\MSSQLSERVER01;Database=BPersonal;Integrated Security=SSPI";
            // string conString = GetConnectionString(strconBPersonal)

            // Crearea obiectului conexiune
            SqlConnection con = new SqlConnection(conString);

            // Deschidere/inchidere si acces la baza
            try
            {
                // Deschide baza si pregateste comanda de interogare
                con.Open();

                string strSql = "SELECT * FROM Salariati WHERE Profesia = 'Inginer'";
                SqlCommand cmd = new SqlCommand(strSql, con);

                // Deschide fisierul lista si scrie capul de tabel
                FileStream strm = new FileStream("ListaPersonal.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(strm);

                string strTitlu = "Lista salariatilor de profesie strungar, la data de " + DateTime.Now;
                sw.WriteLine(strTitlu);
                sw.WriteLine();

                string strCapTab = "Marca Nume si prenume Profesia Salariu";
                sw.WriteLine(strCapTab);

                // Executa comanda si preia articolele in lista
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    StringBuilder txt = new StringBuilder("");
                    txt.Append(rd["Marca"].ToString() + " ");
                    txt.Append(rd["Nume"].ToString() + " ");
                    txt.Append(rd["Prenume"].ToString() + " ");
                    txt.Append(rd["Profesia"].ToString() + " ");
                    txt.Append(rd["Salariu"].ToString());
                    sw.WriteLine(txt);
                }
                rd.Close();
                sw.Close();
                strm.Close();


                Console.WriteLine("Datele au fost preluate si introduse in fisier");
                Console.ReadKey();
            }
            catch (SqlException err)
            {
                Console.WriteLine("Conexiunea la baza de date nu s-a putut realiza: " + err.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private static string GetConnectionString(string name)
        {
            string returnString = null;

            ConnectionStringSettings connString = ConfigurationManager.ConnectionStrings[name];

            if (connString != null)
                returnString = connString.ConnectionString;

            return returnString;
        }
    }
}