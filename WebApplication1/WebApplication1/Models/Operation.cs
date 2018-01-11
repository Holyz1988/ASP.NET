using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public enum TypeOperateur
    {
        Multiplication,
        Addition,
        Soustraction,
        Division,
        Puissance,
        Inconnu
    }
    public class Operation
    {
        public TypeOperateur Operateur { get; private set; }
        public double OperandeDroite { get; private set; }
        public double OperandeGauche { get; private set; }
        public int IdOperateur { get; set; }


        /// <summary>
        /// Constructeur paramétré
        /// 
        /// Permet de construire une opération à partir de son opérateur, son opérande de droite et de gauche
        /// </summary>
        /// <param name="operateur">opérateur de l'opération</param>
        /// <param name="operandeDroite">opérande de droite de l'opération</param>
        /// <param name="operandeGauche">opérande de gauche de l'opération</param>
        public Operation(TypeOperateur operateur, double operandeDroite, double operandeGauche)
        {
            this.Operateur = operateur;
            this.OperandeDroite = operandeDroite;
            this.OperandeGauche = operandeGauche;
        }

        public Operation(TypeOperateur operateur, double operandeDroite, double operandeGauche, int Id)
        {
            this.Operateur = operateur;
            this.OperandeDroite = operandeDroite;
            this.OperandeGauche = operandeGauche;
            this.IdOperateur = Id;
        }

        /// <summary>
        /// Méthode permettant de déterminer si l'opération courante est valide ou non.
        /// Si elle ne l'est pas, la détermination du résultat pourrait crasher.
        /// </summary>
        /// <returns>vrai si l'opération est valide, false sinon</returns>
        public bool IsValide()
        {
            if (this.Operateur == TypeOperateur.Division && this.OperandeDroite == 0.0)
            {
                return false;
            }
            if (this.Operateur == TypeOperateur.Puissance && this.OperandeGauche < 0 && this.OperandeDroite < 1)
            {
                return false;
            }

            return true;
        }

        public string GetRepresentationTextuelle()
        {
            return string.Format("Résultat de {0} de {1} et {2} = {3}", this.Operateur, this.OperandeGauche, this.OperandeDroite, this.GetResult());
        }

        public double GetResult()
        {
            switch (this.Operateur)
            {
                case TypeOperateur.Multiplication:
                    return this.OperandeGauche * this.OperandeDroite;
                case TypeOperateur.Addition:
                    return this.OperandeGauche + this.OperandeDroite;
                case TypeOperateur.Soustraction:
                    return this.OperandeGauche - this.OperandeDroite;
                case TypeOperateur.Division:
                    return this.OperandeGauche / this.OperandeDroite;
                case TypeOperateur.Puissance:
                    return Math.Pow(this.OperandeGauche, this.OperandeDroite);
                default:
                    return 0;
            }
        }
    }
}