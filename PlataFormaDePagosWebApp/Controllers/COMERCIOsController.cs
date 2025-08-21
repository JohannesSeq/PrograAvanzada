using Newtonsoft.Json;
using PlataFormaDePagosWebApp;
using PlataFormaDePagosWebApp.Helpers;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class COMERCIOsController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        //Listado de todos los comercios
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View(db.COMERCIO.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            COMERCIO comercio = db.COMERCIO.Find(id);
            if (comercio == null)
                return HttpNotFound();

            return View(comercio);
        }
        //Crear comercio
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Identificacion,TipoIdentificacion,TipoDeComercio,Telefono,CorreoElectronico,Direccion")] COMERCIO comercio)
        {
            if (db.COMERCIO.Any(c => c.Identificacion == comercio.Identificacion))
            {
                ModelState.AddModelError("Identificacion", "Ya existe un comercio con esta identificación.");
                return View(comercio);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    comercio.FechaDeRegistro = DateTime.Now;
                    comercio.Estado = true;
                    db.COMERCIO.Add(comercio);
                    db.SaveChanges();

                    BitacoraHelper.RegistrarEvento(
                        tabla: "COMERCIO",
                        tipoEvento: "Registrar",
                        descripcion: $"Comercio creado: {comercio.Nombre}",
                        datosPosteriores: comercio
                    );

                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    BitacoraHelper.RegistrarEvento(
                        tabla: "COMERCIO",
                        tipoEvento: "Error",
                        descripcion: ex.Message,
                        stackTrace: ex.ToString()
                    );
                }
            }

            return View(comercio);
        }

        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            COMERCIO comercio = db.COMERCIO.Find(id);
            if (comercio == null)
                return HttpNotFound();

            return View(comercio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdComercio,Nombre,TipoDeComercio,Telefono,CorreoElectronico,Direccion,Estado")] COMERCIO comercio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var comercioOriginal = db.COMERCIO.AsNoTracking().FirstOrDefault(c => c.IdComercio == comercio.IdComercio);
                    if (comercioOriginal == null)
                    {
                        return HttpNotFound();
                    }

                    // Mantener valores que no deben cambiar
                    comercio.Identificacion = comercioOriginal.Identificacion;
                    comercio.TipoIdentificacion = comercioOriginal.TipoIdentificacion;
                    comercio.FechaDeRegistro = comercioOriginal.FechaDeRegistro;

                    // Actualizar automáticamente la fecha de modificación
                    comercio.FechaDeModificacion = DateTime.Now;

                    db.Entry(comercio).State = EntityState.Modified;
                    db.SaveChanges();

                    BitacoraHelper.RegistrarEvento(
                        tabla: "COMERCIO",
                        tipoEvento: "Editar",
                        descripcion: $"Comercio editado: {comercio.IdComercio}",
                        datosAnteriores: comercioOriginal,
                        datosPosteriores: comercio
                    );

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "COMERCIO",
                        tipoEvento: "Error",
                        descripcion: ex.Message,
                        stackTrace: ex.StackTrace
                    );
                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }
            }

            return View(comercio);
        }

        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            COMERCIO comercio = db.COMERCIO.Find(id);
            if (comercio == null)
                return HttpNotFound();

            return View(comercio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COMERCIO comercio = db.COMERCIO.Find(id);
            try
            {
                var datosAnteriores = JsonConvert.SerializeObject(comercio);
                db.COMERCIO.Remove(comercio);
                db.SaveChanges();

                BitacoraHelper.RegistrarEvento(
                    tabla: "COMERCIO",
                    tipoEvento: "Eliminar",
                    descripcion: $"Comercio eliminado: {comercio.IdComercio}",
                    datosAnteriores: comercio
                );
            }
            catch (Exception ex)
            {
                BitacoraHelper.RegistrarEvento(
                    tabla: "COMERCIO",
                    tipoEvento: "Error",
                    descripcion: ex.Message,
                    stackTrace: ex.StackTrace
                );
                ModelState.AddModelError("", "Error al eliminar: " + ex.Message);
                return View(comercio);
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
