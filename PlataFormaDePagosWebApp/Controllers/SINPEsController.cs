using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlataFormaDePagosWebApp;
using PlataFormaDePagosWebApp.Helpers;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class SINPEsController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        // GET: SINPEs
        public ActionResult Index()
        {
            return View(db.SINPE.ToList());
        }

        // GET: SINPEs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SINPE sINPE = db.SINPE.Find(id);
            if (sINPE == null)
            {
                return HttpNotFound();
            }
            return View(sINPE);
        }

        // GET: SINPEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SINPEs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSinpe,TelefonoOrigen,NombreOrigen,TelefonoDestinatario,NombreDestinatario,Monto,Descripcion,Estado")] SINPE sINPE)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sINPE.FechaDeRegistro = DateTime.Now;
                    db.SINPE.Add(sINPE);
                    db.SaveChanges();

                    BitacoraHelper.Registrar("Crear transferencia", "SINPE");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }

            return View(sINPE);
        }

        // GET: SINPEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SINPE sINPE = db.SINPE.Find(id);
            if (sINPE == null)
            {
                return HttpNotFound();
            }
            return View(sINPE);
        }

        // POST: SINPEs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSinpe,TelefonoOrigen,NombreOrigen,TelefonoDestinatario,NombreDestinatario,Monto,Descripcion,Estado")] SINPE sINPE)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sINPE.FechaDeModificacion = DateTime.Now;
                    db.Entry(sINPE).State = EntityState.Modified;
                    db.SaveChanges();

                    BitacoraHelper.Registrar($"Editar transferencia ID {sINPE.IdSinpe}", "SINPE");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al editar: " + ex.Message);
                }
            }
            return View(sINPE);
        }

        // GET: SINPEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SINPE sINPE = db.SINPE.Find(id);
            if (sINPE == null)
            {
                return HttpNotFound();
            }
            return View(sINPE);
        }

        // POST: SINPEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SINPE sINPE = db.SINPE.Find(id);
            try
            {
                db.SINPE.Remove(sINPE);
                db.SaveChanges();

                BitacoraHelper.Registrar($"Eliminar transferencia ID {sINPE.IdSinpe}", "SINPE");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar: " + ex.Message);
                return View(sINPE);
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
