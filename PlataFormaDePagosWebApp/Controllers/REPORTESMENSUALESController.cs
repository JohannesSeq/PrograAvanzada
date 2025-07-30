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
    public class REPORTESMENSUALESController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        // GET: REPORTESMENSUALES
        public ActionResult Index()
        {
            var rEPORTESMENSUALES = db.REPORTESMENSUALES.Include(r => r.COMERCIO);
            return View(rEPORTESMENSUALES.ToList());
        }

        // GET: REPORTESMENSUALES/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTESMENSUALES rEPORTESMENSUALES = db.REPORTESMENSUALES.Find(id);
            if (rEPORTESMENSUALES == null)
            {
                return HttpNotFound();
            }
            return View(rEPORTESMENSUALES);
        }

        // GET: REPORTESMENSUALES/Create
        public ActionResult Create()
        {
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion");
            return View();
        }

        // POST: REPORTESMENSUALES/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdReporte,IdComercio,CantidadDeCajas,MontoTotalRecaudado,MontoTotalComision,FechaDelReporte")] REPORTESMENSUALES rEPORTESMENSUALES)
        {
            if (ModelState.IsValid)
            {
                db.REPORTESMENSUALES.Add(rEPORTESMENSUALES);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", rEPORTESMENSUALES.IdComercio);
            return View(rEPORTESMENSUALES);
        }

        // GET: REPORTESMENSUALES/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTESMENSUALES rEPORTESMENSUALES = db.REPORTESMENSUALES.Find(id);
            if (rEPORTESMENSUALES == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", rEPORTESMENSUALES.IdComercio);
            return View(rEPORTESMENSUALES);
        }

        // POST: REPORTESMENSUALES/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdReporte,IdComercio,CantidadDeCajas,MontoTotalRecaudado,MontoTotalComision,FechaDelReporte")] REPORTESMENSUALES rEPORTESMENSUALES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rEPORTESMENSUALES).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", rEPORTESMENSUALES.IdComercio);
            return View(rEPORTESMENSUALES);
        }

        // GET: REPORTESMENSUALES/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REPORTESMENSUALES rEPORTESMENSUALES = db.REPORTESMENSUALES.Find(id);
            if (rEPORTESMENSUALES == null)
            {
                return HttpNotFound();
            }
            return View(rEPORTESMENSUALES);
        }

        // POST: REPORTESMENSUALES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            REPORTESMENSUALES rEPORTESMENSUALES = db.REPORTESMENSUALES.Find(id);
            db.REPORTESMENSUALES.Remove(rEPORTESMENSUALES);
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
