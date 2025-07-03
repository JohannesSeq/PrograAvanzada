using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using PlataFormaDePagosWebApp;
using PlataFormaDePagosWebApp.Helpers;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class CAJAsController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        public ActionResult Index()
        {
            var cAJA = db.CAJA.Include(c => c.COMERCIO);
            return View(cAJA.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CAJA cAJA = db.CAJA.Find(id);
            if (cAJA == null)
                return HttpNotFound();

            return View(cAJA);
        }

        public ActionResult Create()
        {
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion");
            return View();
        }

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

                    BitacoraHelper.RegistrarEvento(
                        tabla: "CAJA",
                        tipoEvento: "Registrar",
                        descripcion: $"Caja creada: {cAJA.Nombre}",
                        datosPosteriores: cAJA
                    );

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "CAJA",
                        tipoEvento: "Error",
                        descripcion: ex.Message,
                        stackTrace: ex.StackTrace
                    );

                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cAJA.IdComercio);
            return View(cAJA);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCaja,IdComercio,Nombre,Descripcion,TelefonoSINPE,Estado")] CAJA cAJA)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var anterior = db.CAJA.AsNoTracking().FirstOrDefault(x => x.IdCaja == cAJA.IdCaja);
                    cAJA.FechaDeModificacion = DateTime.Now;

                    db.Entry(cAJA).State = EntityState.Modified;
                    db.SaveChanges();

                    BitacoraHelper.RegistrarEvento(
                        tabla: "CAJA",
                        tipoEvento: "Editar",
                        descripcion: $"Caja editada: {cAJA.IdCaja}",
                        datosAnteriores: anterior,
                        datosPosteriores: cAJA
                    );

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "CAJA",
                        tipoEvento: "Error",
                        descripcion: ex.Message,
                        stackTrace: ex.StackTrace
                    );

                    ModelState.AddModelError("", "Error al editar: " + ex.Message);
                }
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cAJA.IdComercio);
            return View(cAJA);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CAJA cAJA = db.CAJA.Find(id);
            if (cAJA == null)
                return HttpNotFound();

            return View(cAJA);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CAJA cAJA = db.CAJA.Find(id);
            try
            {
                var anterior = db.CAJA.AsNoTracking().FirstOrDefault(c => c.IdCaja == id);
                db.CAJA.Remove(cAJA);
                db.SaveChanges();

                BitacoraHelper.RegistrarEvento(
                    tabla: "CAJA",
                    tipoEvento: "Eliminar",
                    descripcion: $"Caja eliminada: {cAJA.IdCaja}",
                    datosAnteriores: anterior
                );
            }
            catch (Exception ex)
            {
                BitacoraHelper.RegistrarEvento(
                    tabla: "CAJA",
                    tipoEvento: "Error",
                    descripcion: ex.Message,
                    stackTrace: ex.StackTrace
                );

                ModelState.AddModelError("", "Error al eliminar: " + ex.Message);
                return View(cAJA);
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
