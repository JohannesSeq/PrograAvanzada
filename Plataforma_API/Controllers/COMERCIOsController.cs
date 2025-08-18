using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Plataforma_API;

namespace Plataforma_API.Controllers
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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMERCIO cOMERCIO = db.COMERCIO.Find(id);
            if (cOMERCIO == null)
            {
                return HttpNotFound();
            }
            return View(cOMERCIO);
        }

        // GET: COMERCIOs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: COMERCIOs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdComercio,Identificacion,TipoIdentificacion,Nombre,TipoDeComercio,Telefono,CorreoElectronico,Direccion,FechaDeRegistro,FechaDeModificacion,Estado")] COMERCIO cOMERCIO)
        {
            if (ModelState.IsValid)
            {
                db.COMERCIO.Add(cOMERCIO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cOMERCIO);
        }

        // GET: COMERCIOs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMERCIO cOMERCIO = db.COMERCIO.Find(id);
            if (cOMERCIO == null)
            {
                return HttpNotFound();
            }
            return View(cOMERCIO);
        }

        // POST: COMERCIOs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdComercio,Identificacion,TipoIdentificacion,Nombre,TipoDeComercio,Telefono,CorreoElectronico,Direccion,FechaDeRegistro,FechaDeModificacion,Estado")] COMERCIO cOMERCIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOMERCIO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cOMERCIO);
        }

        // GET: COMERCIOs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMERCIO cOMERCIO = db.COMERCIO.Find(id);
            if (cOMERCIO == null)
            {
                return HttpNotFound();
            }
            return View(cOMERCIO);
        }

        // POST: COMERCIOs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COMERCIO cOMERCIO = db.COMERCIO.Find(id);
            db.COMERCIO.Remove(cOMERCIO);
            db.SaveChanges();
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
