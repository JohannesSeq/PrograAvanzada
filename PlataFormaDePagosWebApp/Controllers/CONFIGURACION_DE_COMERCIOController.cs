using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PlataFormaDePagosWebApp.Helpers;

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

                // Crear objeto plano sin navegación
                var logData = new
                {
                    configuracion.idConfiguracion,
                    configuracion.IdComercio,
                    configuracion.TipoConfiguracion,
                    configuracion.Comision,
                    configuracion.FechaDeRegistro,
                    configuracion.FechaDeModificacion,
                    configuracion.Estado
                };

                BitacoraHelper.RegistrarEvento(
                    tabla: "CONFIG_COMERCIO",
                    tipoEvento: "Registrar",
                    descripcion: $"Configuración creada para comercio ID: {configuracion.IdComercio}",
                    datosPosteriores: logData
                );

                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre", configuracion.IdComercio);
            return View(configuracion);
        }

        // GET: CONFIGURACION_DE_COMERCIO/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();

            CONFIGURACION_DE_COMERCIO config = db.CONFIGURACION_DE_COMERCIO.Find(id);
            if (config == null)
                return HttpNotFound();

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
                var original = db.CONFIGURACION_DE_COMERCIO.AsNoTracking().FirstOrDefault(c => c.idConfiguracion == configuracion.idConfiguracion);
                var registro = db.CONFIGURACION_DE_COMERCIO.Find(configuracion.idConfiguracion);

                if (registro == null)
                    return HttpNotFound();

                registro.TipoConfiguracion = configuracion.TipoConfiguracion;
                registro.Comision = configuracion.Comision;
                registro.Estado = configuracion.Estado;
                registro.FechaDeModificacion = DateTime.Now;

                db.SaveChanges();

                var anteriorFlat = new
                {
                    original.idConfiguracion,
                    original.IdComercio,
                    original.TipoConfiguracion,
                    original.Comision,
                    original.FechaDeRegistro,
                    original.FechaDeModificacion,
                    original.Estado
                };

                var actualizadoFlat = new
                {
                    registro.idConfiguracion,
                    registro.IdComercio,
                    registro.TipoConfiguracion,
                    registro.Comision,
                    registro.FechaDeRegistro,
                    registro.FechaDeModificacion,
                    registro.Estado
                };

                BitacoraHelper.RegistrarEvento(
                    tabla: "CONFIG_COMERCIO",
                    tipoEvento: "Editar",
                    descripcion: $"Configuración editada ID: {registro.idConfiguracion}",
                    datosAnteriores: anteriorFlat,
                    datosPosteriores: actualizadoFlat
                );

                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre", configuracion.IdComercio);
            return View(configuracion);
        }
    }
}
