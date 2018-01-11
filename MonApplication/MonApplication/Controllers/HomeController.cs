using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public string Index(string chaineAAfficher, int? nombreFoisAAfficher)
        {
            string resultat = "Tu as tapé :";
            for(var i=0; i<nombreFoisAAfficher; i++)
            {
                resultat = resultat + chaineAAfficher + " ";
            }
            return resultat;
        }
    }
}