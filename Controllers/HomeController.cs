using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspinia_MVC5.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home - antes de logar
        public ActionResult Index()
        {
            return View();
        }

        // GET: Inicio - depois de logar
        public ActionResult Inicio()
        {
            return View();
        }
    }
}