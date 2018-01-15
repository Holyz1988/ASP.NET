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

        public ActionResult FormulaireSaisieOperation()
        {
            return View("LesOperations");
        }

        public ActionResult FormulaireModOperation(int IdOperation)
        {
            Operation monOperation = ChercherOperationBdd(IdOperation);
            return View("Modification", monOperation);
        }

        public ActionResult EnvoiFormulaireOperation(TypeOperateur operateur, double operandeGauche, double operandeDroite)
        {
            Operation monOperation = new Operation(operateur, operandeDroite, operandeGauche);
            monOperation.IdOperateur = SauvegarderBdd(monOperation);

            return Redirect("Resultat?IdOperation=" + monOperation.IdOperateur);
        }

        public ActionResult EnvoiFormulaireModOperation(TypeOperateur operateur, double operandeGauche, double operandeDroite, int IdOperation)
        {
            ModificationFormulaire(IdOperation, operateur, operandeGauche, operandeDroite);

            return Redirect("Resultat?IdOperation=" + IdOperation);
        }

        public ActionResult SupprimerOperation(int IdOperation)
        {
            SupprimerBdd(IdOperation);
            return Redirect("Toutes");
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

        public ActionResult Resultat(int IdOperation)
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

        public Operation ChercherOperationBdd(int IdOperation)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand(@"SELECT * FROM Operation WHERE numOperation=@id_operation", connexion);
            maCommande.Parameters.AddWithValue("@id_operation", IdOperation);
            SqlDataReader monReader = maCommande.ExecuteReader();

            monReader.Read();

            var operateur = (string)monReader["Operateur"];
            var operandeDroite = (float)monReader["OperandeDroite"];
            var operandeGauche = (float)monReader["OperandeGauche"];

            Operation operationAModifier = new Operation(TraduireChaineDeCaractèreEnTypeOperateur(operateur), operandeDroite, operandeGauche);
            operationAModifier.IdOperateur = (Int32)monReader["numOperation"];

            return operationAModifier;
        }

        public void ModificationFormulaire(int IdOperateur, TypeOperateur typeOperateur, double operandeGauche, double operandeDroite)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand command = new SqlCommand(
                @"UPDATE Operation SET Operateur=@operateur, OperandeDroite=@operande_droite, OperandeGauche=@operande_gauche WHERE numOperation=@id_operation", connexion);
            command.Parameters.AddWithValue("@operateur", TraduireTypeOperateurEnOperateurBDD(typeOperateur));
            command.Parameters.AddWithValue("@operande_droite", operandeDroite);
            command.Parameters.AddWithValue("@operande_gauche", operandeGauche);
            command.Parameters.AddWithValue("@id_operation", IdOperateur);
            command.ExecuteNonQuery();

            connexion.Close();
        }

        static int SauvegarderBdd(Operation operationASauvegarder)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand command = new SqlCommand(
                @"INSERT INTO Operation(Operateur, OperandeDroite, OperandeGauche) VALUES (@operateur, @droite, @gauche); SELECT SCOPE_IDENTITY()"
                , connexion);
            command.Parameters.AddWithValue("@operateur", TraduireTypeOperateurEnOperateurBDD(operationASauvegarder.Operateur));
            command.Parameters.AddWithValue("@droite", operationASauvegarder.OperandeDroite);
            command.Parameters.AddWithValue("@gauche", operationASauvegarder.OperandeGauche);

            int numOperation = Convert.ToInt32(command.ExecuteScalar());

            connexion.Close();

            return numOperation;
        }

        static void SupprimerBdd(int IdOperation)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand suppression = new SqlCommand(@"DELETE FROM Operation WHERE numOperation=@id_operation", connexion);
            suppression.Parameters.AddWithValue("@id_operation", IdOperation);
            suppression.ExecuteNonQuery();

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