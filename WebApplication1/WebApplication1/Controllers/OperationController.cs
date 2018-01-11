using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    public class OperationController : Controller
    {
        private const string SqlConnectionString = @"Server=.\SQLExpress;Initial Catalog=Operation; Trusted_Connection=Yes";
        // GET: Operation
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CalculOperation(TypeOperateur typeOperateur, double operandeGauche, double operandeDroite)
        {
            Operation monOperation = new Operation(typeOperateur, operandeDroite, operandeGauche);

            if (monOperation.IsValide())
            {
                SauvegarderBdd(monOperation);
                return View("Operation", monOperation);
            }
            else
            {
                return View("Error");
            }
        }
        public ActionResult Addition(double operandeDroite, double operandeGauche)
        {
            return CalculOperation(TypeOperateur.Addition, operandeGauche, operandeDroite);
        }

        public ActionResult Soustraction(double operandeDroite, double operandeGauche)
        {
            return CalculOperation(TypeOperateur.Soustraction, operandeGauche, operandeDroite);
        }

        public ActionResult Multiplication(double operandeDroite, double operandeGauche)
        {
            return CalculOperation(TypeOperateur.Multiplication, operandeGauche, operandeDroite);
        }

        public ActionResult Division(double operandeDroite, double operandeGauche)
        {
            return CalculOperation(TypeOperateur.Division, operandeGauche, operandeDroite);
        }

        public ActionResult Puissance(double operandeDroite, double operandeGauche)
        {
            return CalculOperation(TypeOperateur.Puissance, operandeGauche, operandeDroite);
        }

        public ActionResult Historique(int IdOperation)
        {      
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maRequete = new SqlCommand("SELECT Operateur, OperandeDroite, OperandeGauche FROM Operation WHERE numOperation=@id_operation", connexion);
            maRequete.Parameters.AddWithValue("@id_operation", IdOperation);
            SqlDataReader dataReader = maRequete.ExecuteReader();

            string operateur ="";
            float operandeDroite = 0;
            float operandeGauche = 0;

            if (dataReader.Read())
            {
                operateur = (string)dataReader["Operateur"];
                operandeDroite = (float)dataReader["OperandeDroite"];
                operandeGauche = (float)dataReader["OperandeGauche"];
                Operation monOp = new Operation(TraduireChaineDeCaractèreEnTypeOperateur(operateur), (double)operandeDroite, (double)operandeGauche);
                return View("Operation", monOp);
            }
            else
            {
                return View("Error");
            }
        }

            public ActionResult Toutes()
            {
                List<Operation> mesOperations = new List<Operation>();

                SqlConnection connexion = new SqlConnection(SqlConnectionString);
                connexion.Open();

                SqlCommand maRequete = new SqlCommand("SELECT numOperation, Operateur, OperandeDroite, OperandeGauche FROM Operation", connexion);
                SqlDataReader dataReader = maRequete.ExecuteReader();

                int numId = 0;
                string operateur = "";
                double operandeDroite = 0;
                double operandeGauche = 0;

                while (dataReader.Read())
                {
                    numId = (Int32)dataReader["numOperation"];
                    operateur = (string)dataReader["Operateur"];
                    operandeDroite = (float)dataReader["OperandeDroite"];
                    operandeGauche = (float)dataReader["OperandeGauche"];
                    Operation monOp = new Operation(TraduireChaineDeCaractèreEnTypeOperateur(operateur), operandeDroite, operandeGauche, numId);
                    mesOperations.Add(monOp);
                }

                return View("ListeOperation", mesOperations);
            }
            

            static void SauvegarderBdd(Operation operationASauvegarder)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand command = new SqlCommand(
                @"INSERT INTO Operation(Operateur, OperandeDroite, OperandeGauche) VALUES (@operateur, @droite, @gauche)"
                , connexion);
            command.Parameters.AddWithValue("@operateur", TraduireTypeOperateurEnOperateurBDD(operationASauvegarder.Operateur));
            command.Parameters.AddWithValue("@droite", operationASauvegarder.OperandeDroite);
            command.Parameters.AddWithValue("@gauche", operationASauvegarder.OperandeGauche);
            command.ExecuteNonQuery();
            connexion.Close();
        }

        static char TraduireTypeOperateurEnOperateurBDD(TypeOperateur typeOperateur)
        {
            switch (typeOperateur)
            {
                case TypeOperateur.Multiplication:
                    return '*';
                case TypeOperateur.Addition:
                    return '+';
                case TypeOperateur.Soustraction:
                    return '-';
                case TypeOperateur.Division:
                    return '/';
                case TypeOperateur.Puissance:
                    return '^';
                default:
                    return ' ';
            }
        }

        static TypeOperateur TraduireChaineDeCaractèreEnTypeOperateur(string saisieUtilisateur)
        {
            switch (saisieUtilisateur)
            {
                case "+":
                    return TypeOperateur.Addition;
                case "-":
                    return TypeOperateur.Soustraction;
                case "*":
                    return TypeOperateur.Multiplication;
                case "/":
                    return TypeOperateur.Division;
                case "^":
                    return TypeOperateur.Puissance;
                default:
                    return TypeOperateur.Inconnu;
            }
        }
    }
}