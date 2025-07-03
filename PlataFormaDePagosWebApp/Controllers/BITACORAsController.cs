using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PlataFormaDePagosWebApp;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class BITACORAsController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        public ActionResult Index()
        {
            var bitacora = db.BITACORA.OrderByDescending(b => b.FechaDeEvento).ToList();
            return View(bitacora);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
