using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PlataFormaDePagosWebApp.Models;
using PlataFormaDePagosWebApp.Helpers;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class COMERCIOsController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        // GET: COMERCIOs
        public ActionResult Index()
        {
            return View(db.COMERCIO.ToList());
        }

        // GET: COMERCIOs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            COMERCIO cOMERCIO = db.COMERCIO.Find(id);
            if (cOMERCIO == null)
                return HttpNotFound();

            return View(cOMERCIO);
        }

        // GET: COMERCIOs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COMERCIOs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdComercio,Nombre,Identificacion,CorreoElectronico")] COMERCIO cOMERCIO)
        {
            // Validación de identificación única
            if (db.COMERCIO.Any(c => c.Identificacion == cOMERCIO.Identificacion))
            {
                ModelState.AddModelError("Identificacion", "Ya existe un comercio con esta identificación.");
                return View(cOMERCIO);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cOMERCIO.FechaDeRegistro = DateTime.Now;
                    db.COMERCIO.Add(cOMERCIO);
                    db.SaveChanges();

                    // Registro en bitácora
                    BitacoraHelper.Registrar("Crear comercio", "COMERCIO");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }

            return View(cOMERCIO);
        }

        // GET: COMERCIOs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            COMERCIO cOMERCIO = db.COMERCIO.Find(id);
            if (cOMERCIO == null)
                return HttpNotFound();

            return View(cOMERCIO);
        }

        // POST: COMERCIOs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdComercio,Nombre,Identificacion,CorreoElectronico")] COMERCIO cOMERCIO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cOMERCIO.FechaDeModificacion = DateTime.Now;
                    db.Entry(cOMERCIO).State = EntityState.Modified;
                    db.SaveChanges();

                    // Registro en bitácora
                    BitacoraHelper.Registrar($"Editar comercio ID {cOMERCIO.IdComercio}", "COMERCIO");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }

            return View(cOMERCIO);
        }

        // GET: COMERCIOs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            COMERCIO cOMERCIO = db.COMERCIO.Find(id);
            if (cOMERCIO == null)
                return HttpNotFound();

            return View(cOMERCIO);
        }

        // POST: COMERCIOs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COMERCIO cOMERCIO = db.COMERCIO.Find(id);
            try
            {
                db.COMERCIO.Remove(cOMERCIO);
                db.SaveChanges();

                // Registro en bitácora
                BitacoraHelper.Registrar($"Eliminar comercio ID {cOMERCIO.IdComercio}", "COMERCIO");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar: " + ex.Message);
                return View(cOMERCIO);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
