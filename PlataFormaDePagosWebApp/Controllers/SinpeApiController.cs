using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class SinpeApiController : Controller
    {
        // Vista para consultar SINPEs
        public ActionResult Consultar()
        {
            return View();
        }

        // Vista para recibir SINPEs
        public ActionResult Recibir()
        {
            return View();
        }

    }
}