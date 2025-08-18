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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAJA cAJA = db.CAJA.Find(id);
            if (cAJA == null)
            {
                return HttpNotFound();
            }
            return View(cAJA);
        }

        // GET: CAJAs/Create
        public ActionResult Create()
        {
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion");
            return View();
        }

        // POST: CAJAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCaja,IdComercio,Nombre,Descripcion,TelefonoSINPE,FechaDeRegistro,FechaDeModificacion,Estado")] CAJA cAJA)
        {
            if (ModelState.IsValid)
            {
                db.CAJA.Add(cAJA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cAJA.IdComercio);
            return View(cAJA);
        }

        // GET: CAJAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAJA cAJA = db.CAJA.Find(id);
            if (cAJA == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cAJA.IdComercio);
            return View(cAJA);
        }

        // POST: CAJAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCaja,IdComercio,Nombre,Descripcion,TelefonoSINPE,FechaDeRegistro,FechaDeModificacion,Estado")] CAJA cAJA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cAJA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Identificacion", cAJA.IdComercio);
            return View(cAJA);
        }

        // GET: CAJAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAJA cAJA = db.CAJA.Find(id);
            if (cAJA == null)
            {
                return HttpNotFound();
            }
            return View(cAJA);
        }

        // POST: CAJAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CAJA cAJA = db.CAJA.Find(id);
            db.CAJA.Remove(cAJA);
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
