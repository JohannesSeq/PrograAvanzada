using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Forbidden()
        {
            return View();
        }

        public ActionResult NotFound()
        { 
            return View(); 
        }
    }
}