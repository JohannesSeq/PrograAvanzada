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
        public ActionResult Create([Bind(Include = "IdComercio,Nombre,Descripcion,TelefonoSINPE,Estado")] CAJA cAJA)
        {
            // Validación: Teléfono activo único
            bool telefonoDuplicado = db.CAJA.Any(c =>
                c.TelefonoSINPE == cAJA.TelefonoSINPE &&
                c.Estado == true);

            if (telefonoDuplicado)
                ModelState.AddModelError("TelefonoSINPE", "Ya existe una caja activa con ese número de teléfono.");

            // Validación: Nombre único por comercio
            bool nombreDuplicado = db.CAJA.Any(c =>
                c.Nombre == cAJA.Nombre &&
                c.IdComercio == cAJA.IdComercio);

            if (nombreDuplicado)
                ModelState.AddModelError("Nombre", "Ya existe una caja con ese nombre para este comercio.");

            if (ModelState.IsValid)
            {
                try
                {
                    cAJA.FechaDeRegistro = DateTime.Now;
                    cAJA.FechaDeModificacion = null;

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

        public ActionResult VerSinpe(string telefono)
        {
            if (string.IsNullOrEmpty(telefono))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sinpes = db.SINPE
                .Where(s => s.TelefonoDestinatario == telefono)
                .OrderByDescending(s => s.FechaDeRegistro)
                .ToList();

            ViewBag.Telefono = telefono;
            return View(sinpes);
        }

        public ActionResult Sync(string telefono)
        {

            if (string.IsNullOrEmpty(telefono))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sinpes = db.SINPE
                .Where(s => s.TelefonoDestinatario == telefono)
                .OrderByDescending(s => s.FechaDeRegistro)
                .ToList();



            foreach (SINPE s in sinpes)
            {

                try 
                {
                    var anterior = db.SINPE.AsNoTracking().FirstOrDefault(x => x.IdSinpe == s.IdSinpe);
                    s.FechaDeModificacion = DateTime.Now;
                    s.Estado = true;
                    db.Entry(s).State = EntityState.Modified;
                    db.SaveChanges();

                    BitacoraHelper.RegistrarEvento(
                        tabla: "SINPE",
                        tipoEvento: "Editar",
                        descripcion: $"SINPE editado: {s.IdSinpe}",
                        datosAnteriores: anterior,
                        datosPosteriores: s
                    );
                } 
                
                catch (Exception ex)
                {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "SINPE",
                        tipoEvento: "Error",
                        descripcion: ex.Message,
                        stackTrace: ex.StackTrace
                    );

                    ModelState.AddModelError("", "Error al editar: " + ex.Message);
                }


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
