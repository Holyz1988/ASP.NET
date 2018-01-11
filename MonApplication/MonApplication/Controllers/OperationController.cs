using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MonApplication.Models;
using System.Data.SqlClient;

namespace MonApplication.Controllers
{
    public class OperationController : Controller
    {
        public string Addition(double operandeGauche, double operandeDroite)
        {
            return CalculOperation(TypeOperateur.Addition, operandeGauche, operandeDroite);
        }

        private string CalculOperation(TypeOperateur typeOperateur, double operandeGauche, double operandeDroite) 
        {
            Operation operationSaisie = new Operation(typeOperateur, operandeDroite, operandeGauche);
            if (operationSaisie.IsValide())
            {
                SauvegarderBdd(operationSaisie);
                string representation = operationSaisie.GetRepresentationTextuelle();
                return representation;
            }
            else
            {
                return "Calcul invalide";
            }
        }

        public string Soustraction(double operandeGauche, double operandeDroite)
        {
            return CalculOperation(TypeOperateur.Soustraction, operandeGauche, operandeDroite);
        }


        public string Multiplication(double operandeGauche, double operandeDroite)
        {
            return CalculOperation(TypeOperateur.Multiplication, operandeGauche, operandeDroite);
        }

        public string Division(double operandeGauche, double operandeDroite)
        {
            return CalculOperation(TypeOperateur.Division, operandeGauche, operandeDroite);
        }

        public string Puissance(double operandeGauche, double operandeDroite)
        {
            return CalculOperation(TypeOperateur.Puissance, operandeGauche, operandeDroite);
        }

        private const string SqlConnectionString =
            @"Server=.\SQLExpress;Initial Catalog=MaPremiereBDD; Trusted_Connection=Yes";

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
    }
}