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
    public class SINPEsController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View(db.SINPE.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SINPE sINPE = db.SINPE.Find(id);
            if (sINPE == null)
                return HttpNotFound();

            return View(sINPE);
        }
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TelefonoOrigen,NombreOrigen,TelefonoDestinatario,NombreDestinatario,Monto,Descripcion")] SINPE sINPE)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si el teléfono de la caja existe y está activo
                    var caja = db.CAJA.FirstOrDefault(c => c.TelefonoSINPE == sINPE.TelefonoDestinatario && c.Estado == true);

                    if (caja == null)
                    {
                        ModelState.AddModelError("TelefonoDestinatario", "El teléfono ingresado no pertenece a una caja activa.");
                        return View(sINPE);
                    }

                    // Setear valores por defecto
                    sINPE.FechaDeRegistro = DateTime.Now;
                    sINPE.Estado = false;

                    db.SINPE.Add(sINPE);
                    db.SaveChanges();

                    BitacoraHelper.RegistrarEvento(
                        tabla: "SINPE",
                        tipoEvento: "Registrar",
                        descripcion: $"Transferencia registrada: {sINPE.IdSinpe}",
                        datosPosteriores: sINPE
                    );

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "SINPE",
                        tipoEvento: "Error",
                        descripcion: ex.Message,
                        stackTrace: ex.StackTrace
                    );

                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }

            return View(sINPE);
        }

        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SINPE sINPE = db.SINPE.Find(id);
            if (sINPE == null)
                return HttpNotFound();

            return View(sINPE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSinpe,TelefonoOrigen,NombreOrigen,TelefonoDestinatario,NombreDestinatario,Monto,Descripcion,Estado")] SINPE sINPE)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var anterior = db.SINPE.AsNoTracking().FirstOrDefault(x => x.IdSinpe == sINPE.IdSinpe);
                    sINPE.FechaDeModificacion = DateTime.Now;

                    db.Entry(sINPE).State = EntityState.Modified;
                    db.SaveChanges();

                    BitacoraHelper.RegistrarEvento(
                        tabla: "SINPE",
                        tipoEvento: "Editar",
                        descripcion: $"Transferencia editada: {sINPE.IdSinpe}",
                        datosAnteriores: anterior,
                        datosPosteriores: sINPE
                    );

                    return RedirectToAction("Index");
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

            return View(sINPE);
        }

        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SINPE sINPE = db.SINPE.Find(id);
            if (sINPE == null)
                return HttpNotFound();

            return View(sINPE);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SINPE sINPE = db.SINPE.Find(id);
            try
            {
                var anterior = db.SINPE.AsNoTracking().FirstOrDefault(x => x.IdSinpe == id);
                db.SINPE.Remove(sINPE);
                db.SaveChanges();

                BitacoraHelper.RegistrarEvento(
                    tabla: "SINPE",
                    tipoEvento: "Eliminar",
                    descripcion: $"Transferencia eliminada: {sINPE.IdSinpe}",
                    datosAnteriores: anterior
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

                ModelState.AddModelError("", "Error al eliminar: " + ex.Message);
                return View(sINPE);
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
