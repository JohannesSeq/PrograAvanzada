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
            var configuraciones = db.CONFIGURACION_DE_COMERCIO.Include(c => c.COMERCIO).ToList();
            return View(configuraciones);
        }

        // GET: CONFIGURACION_DE_COMERCIO/Create
        public ActionResult Create()
        {
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre");
            return View();
        }

        // POST: CONFIGURACION_DE_COMERCIO/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CONFIGURACION_DE_COMERCIO configuracion)
        {
            if (db.CONFIGURACION_DE_COMERCIO.Any(c => c.IdComercio == configuracion.IdComercio))
            {
                ModelState.AddModelError("", "Ya existe una configuración para este comercio.");
            }

            if (ModelState.IsValid)
            {
                configuracion.FechaDeRegistro = DateTime.Now;
                configuracion.FechaDeModificacion = DateTime.Now;
                configuracion.Estado = true;
                db.CONFIGURACION_DE_COMERCIO.Add(configuracion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre", configuracion.IdComercio);
            return View(configuracion);
        }

        // GET: CONFIGURACION_DE_COMERCIO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();

            CONFIGURACION_DE_COMERCIO config = db.CONFIGURACION_DE_COMERCIO.Find(id);
            if (config == null) return HttpNotFound();

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre", config.IdComercio);
            return View(config);
        }

        // POST: CONFIGURACION_DE_COMERCIO/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CONFIGURACION_DE_COMERCIO configuracion)
        {
            if (ModelState.IsValid)
            {
                var original = db.CONFIGURACION_DE_COMERCIO.Find(configuracion.idConfiguracion);
                original.TipoConfiguracion = configuracion.TipoConfiguracion;
                original.Comision = configuracion.Comision;
                original.Estado = configuracion.Estado;
                original.FechaDeModificacion = DateTime.Now;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre", configuracion.IdComercio);
            return View(configuracion);
        }
    }
}
