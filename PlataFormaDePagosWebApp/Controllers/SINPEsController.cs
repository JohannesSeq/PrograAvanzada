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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSinpe,TelefonoOrigen,NombreOrigen,TelefonoDestinatario,NombreDestinatario,Monto,Descripcion,FechaDeRegistro,FechaDeModificacion,Estado")] SINPE sINPE)
        {
            if (ModelState.IsValid)
            {
                db.SINPE.Add(sINPE);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSinpe,TelefonoOrigen,NombreOrigen,TelefonoDestinatario,NombreDestinatario,Monto,Descripcion,FechaDeRegistro,FechaDeModificacion,Estado")] SINPE sINPE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sINPE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            db.SINPE.Remove(sINPE);
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
