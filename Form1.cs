using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace formulaire2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Class1 c = new Class1();  //la déclaration globale de la classe 

        //déclaration de la méthode nombre
        public int nombre()  //calcule le nombre d'ID entré par l'utilisateur
        {
            int cpt;  //une variable à laquelle on va affecter le nombre d'ID 
            c.cmd.CommandText = " select count(ID) from GI where ID ='" + ID.Text + "'";  //permet de prendre l'obj cmd et le manipuler
            c.cmd.Connection = c.con;
            cpt = (int) c.cmd.ExecuteScalar();  //executescalar car la requêtre va retourner une seule donnée qui va être stockée dans cpt
            return cpt;
        }

        //décalaration de la méthode ajouter 
        public bool ajouter()
        {
            if(nombre() == 0)  //si le ID n'existe pas 
            {
                c.cmd.CommandText = " insert into GI values ('" + ID.Text + "','" + Nom.Text + "','" + Prénom.Text + "','" + Ville.Text +"')";
                c.cmd.Connection = c.con;
                c.cmd.ExecuteNonQuery();  //car on a la mise à jour (ajouter)
                return true;  //l'ajout est effectué 
            }
            return false;  //l'ajout n'est pas effectué 
        }

        //décalaration de la méthode supprimer 
        public bool supprimer()
        {
            if (nombre() != 0)  //si le ID existe  
            {
                c.cmd.CommandText = " delete from GI where ID ='" + ID.Text + "'";
                c.cmd.Connection = c.con;
                c.cmd.ExecuteNonQuery();  //car on a la mise à jour (ajouter)
                return true;  //l'ajout est effectué 
            }
            return false;  //l'ajout n'est pas effectué 
        }

        //décalaration de la méthode modifier 
        public bool modifier()
        {
            if (nombre() != 0)  //si le ID existe  
            {
                c.cmd.CommandText = "update GI set Nom = '" + Nom.Text + "',Prénom ='" + Prénom.Text + "',Ville='" + Ville.Text + "'where ID ='" + ID.Text + "'";  //requête de la mise à jour
                c.cmd.Connection = c.con;
                c.cmd.ExecuteNonQuery();  //car on a la mise à jour (ajouter)
                return true;  //la modification est effectué 
            }
            return false;  //la modif n'est pas effectué 
        }

        //remplissage de GridView
        public void remplirGrid()  //pour ajouter les données entrées dans la gridview
        {
            if (c.dt.Rows != null)  //si la ligne n'est pas vide  
            {
                c.dt.Clear();  //on doit la vider 
            }
            c.cmd.CommandText = " select * from GI";  
            c.cmd.Connection = c.con;
            c.dr = c.cmd.ExecuteReader();
            c.dt.Load(c.dr);  //convertir les données en une table de données 
            dataGridView1.DataSource = c.dt;
            c.dr.Close();
        }
        
        //méthode annuler 
        public void annuler (Control f)  //control est la form
        {
            foreach (Control ct in f.Controls)  //chaque controle ct de la form f
            {
                if(ct.GetType() == typeof(TextBox) || ct.GetType() == typeof(ComboBox))  //si le type est TextBox
                {
                    ct.Text = "";  //vider les TextBox
                }
                if(ct.Controls.Count != 0)  //si les champs ne sont pas vides  (vider le groupe box)
                {
                    annuler(ct);  //appel de la méthode annuler 
                }
            }
        }

        private void Modifier_Click(object sender, EventArgs e)
        {
                if (ID.Text == "" || Nom.Text == "" || Prénom.Text == "" || Ville.Text == "")  //si l'un des champs est vide
                {
                    MessageBox.Show("Merci de remplir tous les champs");  //afficher ce msg
                    return; //pour arrêter le programme pour ne pas avoir des erreurs 
                }
                if (modifier() == true)  //si la modif peut être effectué
                {
                    MessageBox.Show("Cet étudiant a été modifié");  //si l'étudiant est modifié 
                    remplirGrid();
                }
                else
                {
                    MessageBox.Show("Cet étudiant n'existe pas");  //afficher ce msg si l'étudiant n'existe pas

                }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)  //l'affichage des données dans la form
        {
            c.CONNECTER();  //la cnx à l'app
            remplirGrid();
            c.cmd.CommandText = "select * from GI";  //permet d'écrire la requête sql
            c.cmd.Connection = c.con;  //obtenir les données à partir de la classe 
            c.dr = c.cmd.ExecuteReader();  //un flux de données qui retourne un dr 
            c.dt.Load(c.dr);  //permet de convertir ces données en une table de données 
            dataGridView1.DataSource = c.dt;  //pour les afficher dans dataGridView
            c.dr.Close();  //fermer la cnx avec dr (pour ne pas manipuler ces données)
        }

        private void Fermer_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Voulez-vous fermer cet onglet?", "Confirmation", MessageBoxButtons.YesNo)==DialogResult.Yes)  //afficher le msg de confirmation ( si oui)
            {
                c.DECONNECTER();  //appel de la méthode déco
                this.Close();  //on ferme la cnx 
            }
        }

        private void Annuler_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Voulez-vous vider les champs?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                annuler(this);
            }
        }

        private void Ajouter_Click(object sender, EventArgs e)
        {
            if(ID.Text == "" || Nom.Text == "" || Prénom.Text == "" || Ville.Text == "")  //si l'un des champs est vide
            {
                MessageBox.Show("Merci de remplir tous les champs");  //afficher ce msg
                return; //pour arrêter le programme pour ne pas avoir des erreurs 
            }
            if(ajouter() == true)  //si l'ajout peut être effectué
            {
                MessageBox.Show("Cet étudiant a été ajouté");  //si l'étudiant est ajouté 
                remplirGrid();
            }
            else
            {
                MessageBox.Show("Cet étudiant existe déjà");  //afficher ce msg si l'étudiant existe déjà 

            }
        }

        private void Supprimer_Click(object sender, EventArgs e)
        {
                if (ID.Text == "")  
                {
                    MessageBox.Show("Merci de remplir le champs");  //afficher ce msg
                    return; //pour arrêter le programme pour ne pas avoir des erreurs 
                }
                if (supprimer() == true)  //si la suppression peut être effectuée 
                {
                    MessageBox.Show("Cet étudiant a été supprimé");
                    remplirGrid();
                }
                else
                {
                    MessageBox.Show("Cet étudiant n'existe pas");

                } 
        }
    }
}
