using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Models.PageIndexModel model = new Models.PageIndexModel();

            return View();
        }
        private const string SqlConnectionString = @"Server=.\SQLExpress;Initial Catalog=Operation; Trusted_Connection=Yes";
        //private Models.PageIndexModel RecupérerPageIndexDepuisBDD()
        //{


    //}

        public ActionResult AutreChose()
        {
            return View();
        }
    }
}