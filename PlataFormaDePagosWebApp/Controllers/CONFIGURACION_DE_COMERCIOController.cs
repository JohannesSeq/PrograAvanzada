using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlataFormaDePagosWebApp;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class CONFIGURACION_DE_COMERCIOController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        // GET: CONFIGURACION_DE_COMERCIO
        public ActionResult Index()
        {
            var cONFIGURACION_DE_COMERCIO = db.CONFIGURACION_DE_COMERCIO.Include(c => c.COMERCIO);
            return View(cONFIGURACION_DE_COMERCIO.ToList());
        }

        // GET: CONFIGURACION_DE_COMERCIO/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONFIGURACION_DE_COMERCIO cONFIGURACION_DE_COMERCIO = db.CONFIGURACION_DE_COMERCIO.Find(id);
            if (cONFIGURACION_DE_COMERCIO == null)
            {
                return HttpNotFound();
            }
            return View(cONFIGURACION_DE_COMERCIO);
        }

        // GET: CONFIGURACION_DE_COMERCIO/Create
        public ActionResult Create()
        {
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion");
            return View();
        }

        // POST: CONFIGURACION_DE_COMERCIO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idConfiguracion,IdComercio,TipoConfiguracion,Comision,FechaDeRegistro,FechaDeModificacion,Estado")] CONFIGURACION_DE_COMERCIO cONFIGURACION_DE_COMERCIO)
        {
            if (ModelState.IsValid)
            {
                db.CONFIGURACION_DE_COMERCIO.Add(cONFIGURACION_DE_COMERCIO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cONFIGURACION_DE_COMERCIO.IdComercio);
            return View(cONFIGURACION_DE_COMERCIO);
        }

        // GET: CONFIGURACION_DE_COMERCIO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONFIGURACION_DE_COMERCIO cONFIGURACION_DE_COMERCIO = db.CONFIGURACION_DE_COMERCIO.Find(id);
            if (cONFIGURACION_DE_COMERCIO == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cONFIGURACION_DE_COMERCIO.IdComercio);
            return View(cONFIGURACION_DE_COMERCIO);
        }

        // POST: CONFIGURACION_DE_COMERCIO/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idConfiguracion,IdComercio,TipoConfiguracion,Comision,FechaDeRegistro,FechaDeModificacion,Estado")] CONFIGURACION_DE_COMERCIO cONFIGURACION_DE_COMERCIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cONFIGURACION_DE_COMERCIO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cONFIGURACION_DE_COMERCIO.IdComercio);
            return View(cONFIGURACION_DE_COMERCIO);
        }

        // GET: CONFIGURACION_DE_COMERCIO/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONFIGURACION_DE_COMERCIO cONFIGURACION_DE_COMERCIO = db.CONFIGURACION_DE_COMERCIO.Find(id);
            if (cONFIGURACION_DE_COMERCIO == null)
            {
                return HttpNotFound();
            }
            return View(cONFIGURACION_DE_COMERCIO);
        }

        // POST: CONFIGURACION_DE_COMERCIO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CONFIGURACION_DE_COMERCIO cONFIGURACION_DE_COMERCIO = db.CONFIGURACION_DE_COMERCIO.Find(id);
            db.CONFIGURACION_DE_COMERCIO.Remove(cONFIGURACION_DE_COMERCIO);
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
