using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonApplication.Models
{
    /// <summary>
    /// Enumeration représentant les types d'opération gérées dans le système
    /// </summary>
    enum TypeOperateur
    {
        Multiplication,
        Addition,
        Soustraction,
        Division,
        Puissance,
        Inconnu
    }
}