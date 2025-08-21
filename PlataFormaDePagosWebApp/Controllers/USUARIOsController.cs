using PlataFormaDePagosWebApp;
using PlataFormaDePagosWebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class USUARIOsController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        // GET: USUARIOs
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Index()
        {
            var uSUARIO = db.USUARIO.Include(u => u.COMERCIO);
            return View(uSUARIO.ToList());
        }

        // GET: USUARIOs/Details/5
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIO.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            return View(uSUARIO);
        }

        // GET: USUARIOs/Create
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion");
            return View();
        }

        // POST: USUARIOs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdComercio,Nombres,PrimerApellido,SegundoApellido,Identificacion,CorreoElectronico")] USUARIO uSUARIO)
        {
            if (ModelState.IsValid)
            {
                uSUARIO.FechaDeRegistro = DateTime.Now;
                uSUARIO.FechaDeModificacion = DateTime.Now;
                uSUARIO.Estado = true;
                uSUARIO.IdNetUser = Guid.NewGuid();


                try
                {
                    db.USUARIO.Add(uSUARIO);
                    db.SaveChanges();

                    BitacoraHelper.RegistrarEvento(
                        tabla: "USUARIO",
                        tipoEvento: "Registrar",
                        descripcion: $"Usuario creado: {uSUARIO.IdUsuario}",
                        datosPosteriores: uSUARIO
                    );
                }
                catch (Exception ex)
                {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "USUARIO",
                        tipoEvento: "Error",
                        descripcion: ex.Message,
                        stackTrace: ex.StackTrace
                    );

                    ModelState.AddModelError("", "Error al guardar: " + ex.Message);
                }

                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", uSUARIO.IdComercio);
            return View(uSUARIO);
        }

        // GET: USUARIOs/Edit/5
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIO.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", uSUARIO.IdComercio);
            return View(uSUARIO);
        }

        // POST: USUARIOs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUsuario,IdComercio,IdNetUser,Nombres,PrimerApellido,SegundoApellido,Identificacion,CorreoElectronico,FechaDeRegistro,FechaDeModificacion,Estado")] USUARIO uSUARIO)
        {
            if (ModelState.IsValid)
            {
                uSUARIO.FechaDeModificacion = DateTime.Now;


                try
                {
                    db.Entry(uSUARIO).State = EntityState.Modified;
                    db.SaveChanges();
                    BitacoraHelper.RegistrarEvento(
                        tabla: "USUARIO",
                        tipoEvento: "Editar",
                        descripcion: $"Usuario editado: {uSUARIO.IdUsuario}",
                        datosPosteriores: uSUARIO
                    );

                }
                catch (Exception ex) {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "USUARIO",
                        tipoEvento: "Error",
                        descripcion: ex.Message,
                        stackTrace: ex.StackTrace
                    );

                    ModelState.AddModelError("", "Error al editar: " + ex.Message);
                }


                return RedirectToAction("Index");
            }
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", uSUARIO.IdComercio);
            return View(uSUARIO);
        }

        // GET: USUARIOs/Delete/5
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIO.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            return View(uSUARIO);
        }

        // POST: USUARIOs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {

                USUARIO uSUARIO = db.USUARIO.Find(id);
                db.USUARIO.Remove(uSUARIO);
                db.SaveChanges();

                BitacoraHelper.RegistrarEvento(
                    tabla: "USUARIO",
                    tipoEvento: "Eliminar",
                    descripcion: $"Usuario eliminado: {uSUARIO.IdUsuario}",
                    datosPosteriores: uSUARIO
            );

            }
            catch (Exception ex)
            {

                BitacoraHelper.RegistrarEvento(
                    tabla: "USUARIO",
                    tipoEvento: "Error",
                    descripcion: ex.Message,
                    stackTrace: ex.StackTrace
                );

                ModelState.AddModelError("", "Error al borrar usuario: " + ex.Message);

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
