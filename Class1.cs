using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace formulaire2
{
    class Class1
    {
        //déclaration des objets sql
        public SqlConnection con = new SqlConnection();  //con est l'obj de conenction
        public SqlCommand cmd = new SqlCommand();  //cmd est l'obj de commande 
        public SqlDataReader dr; 
        public DataTable dt = new DataTable();  //permt de convertir les données de dr en table 

        //déclaration de la méthode connecter 
        public void CONNECTER()  //permet la connection à l'application
        {
            if(con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)  //si la cnx est fermée ou brisée on l'ouvre
            {
                con.ConnectionString = @"Data Source=DESKTOP-A277PV0\SQLEXPRESS;Initial Catalog=STUDENT;Integrated Security=True";  //la chaîne de cnx 
                con.Open();  //ouvrir la cnx 
            }
        }

        //déclaration de la méthode déconnecter 
        public void DECONNECTER()  //permet la déco de l'app
        {
            if (con.State == ConnectionState.Open)  //si la cnx est ouverte
            {
                con.Close();  //fermer la cnx 
            }
        }
    }
}
