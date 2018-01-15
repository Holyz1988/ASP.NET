using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Net.Mail;
using System.Web.Configuration;

namespace WebApplication1.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Formulaire()
        {
            return View("page_contact");
        }

        public ActionResult Index(FormulaireUpdateOperation monFormulaire)
        {
            if (ModelState.IsValid)
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new NetworkCredential("username", "password");
            }

        }
    }
}