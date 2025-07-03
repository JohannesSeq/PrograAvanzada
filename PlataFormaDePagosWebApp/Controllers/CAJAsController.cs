using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PlataFormaDePagosWebApp;
using PlataFormaDePagosWebApp.Helpers;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class CAJAsController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        // GET: CAJAs
        public ActionResult Index()
        {
            var cAJA = db.CAJA.Include(c => c.COMERCIO);
            return View(cAJA.ToList());
        }

        // GET: CAJAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CAJA cAJA = db.CAJA.Find(id);
            if (cAJA == null)
                return HttpNotFound();

            return View(cAJA);
        }

        // GET: CAJAs/Create
        public ActionResult Create()
        {
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion");
            return View();
        }

        // POST: CAJAs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCaja,IdComercio,Nombre,Descripcion,TelefonoSINPE,Estado")] CAJA cAJA)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cAJA.FechaDeRegistro = DateTime.Now;
                    db.CAJA.Add(cAJA);
                    db.SaveChanges();

                    BitacoraHelper.Registrar("Crear caja", "CAJA");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cAJA.IdComercio);
            return View(cAJA);
        }

        // GET: CAJAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CAJA cAJA = db.CAJA.Find(id);
            if (cAJA == null)
                return HttpNotFound();

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cAJA.IdComercio);
            return View(cAJA);
        }

        // POST: CAJAs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCaja,IdComercio,Nombre,Descripcion,TelefonoSINPE,Estado")] CAJA cAJA)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cAJA.FechaDeModificacion = DateTime.Now;
                    db.Entry(cAJA).State = EntityState.Modified;
                    db.SaveChanges();

                    BitacoraHelper.Registrar($"Editar caja ID {cAJA.IdCaja}", "CAJA");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al editar: " + ex.Message);
                }
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cAJA.IdComercio);
            return View(cAJA);
        }

        // GET: CAJAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CAJA cAJA = db.CAJA.Find(id);
            if (cAJA == null)
                return HttpNotFound();

            return View(cAJA);
        }

        // POST: CAJAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CAJA cAJA = db.CAJA.Find(id);
            try
            {
                db.CAJA.Remove(cAJA);
                db.SaveChanges();

                BitacoraHelper.Registrar($"Eliminar caja ID {cAJA.IdCaja}", "CAJA");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar: " + ex.Message);
                return View(cAJA);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
