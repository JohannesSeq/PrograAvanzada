using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PlataFormaDePagosWebApp.Helpers;
using Newtonsoft.Json;

namespace PlataFormaDePagosWebApp.Controllers
{
    public class REPORTESMENSUALESController : Controller
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        //Get
        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Index()
        {
            var reportes = db.REPORTESMENSUALES.Include(r => r.COMERCIO).ToList();
            return View(reportes);
        }

        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var reporte = db.REPORTESMENSUALES.Find(id);
            if (reporte == null) return HttpNotFound();
            return View(reporte);
        }

        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(REPORTESMENSUALES reporte)
        {
            if (ModelState.IsValid)
            {
                db.REPORTESMENSUALES.Add(reporte);
                db.SaveChanges();

                try
                {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "REPORTES_MENSUAL",
                        tipoEvento: "Registrar",
                        descripcion: $"Reporte creado para comercio ID: {reporte.IdComercio}",
                        datosPosteriores: reporte
                    );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error al registrar en bitácora: " + ex.Message);
                }

                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre", reporte.IdComercio);
            return View(reporte);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var reporte = db.REPORTESMENSUALES.Find(id);
            if (reporte == null) return HttpNotFound();

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre", reporte.IdComercio);
            return View(reporte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(REPORTESMENSUALES reporte)
        {
            if (ModelState.IsValid)
            {
                var anterior = db.REPORTESMENSUALES.AsNoTracking().FirstOrDefault(r => r.IdReporte == reporte.IdReporte);

                db.Entry(reporte).State = EntityState.Modified;
                db.SaveChanges();

                try
                {
                    BitacoraHelper.RegistrarEvento(
                        tabla: "REPORTES_MENSUAL",
                        tipoEvento: "Editar",
                        descripcion: $"Reporte editado ID: {reporte.IdReporte}",
                        datosAnteriores: anterior,
                        datosPosteriores: reporte
                    );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error al registrar en bitácora: " + ex.Message);
                }

                return RedirectToAction("Index");
            }

            ViewBag.IdComercio = new SelectList(db.COMERCIO, "IdComercio", "Nombre", reporte.IdComercio);
            return View(reporte);
        }

        [AuthzHandler(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var reporte = db.REPORTESMENSUALES.Find(id);
            if (reporte == null) return HttpNotFound();

            return View(reporte);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var reporte = db.REPORTESMENSUALES.Find(id);
            db.REPORTESMENSUALES.Remove(reporte);
            db.SaveChanges();

            try
            {
                BitacoraHelper.RegistrarEvento(
                    tabla: "REPORTES_MENSUAL",
                    tipoEvento: "Eliminar",
                    descripcion: $"Reporte eliminado ID: {id}",
                    datosAnteriores: reporte
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al registrar en bitácora: " + ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GenerarReportes()
        {
            var fechaActual = DateTime.Now;
            var primerDiaMes = new DateTime(fechaActual.Year, fechaActual.Month, 1);
            var ultimoDiaMes = primerDiaMes.AddMonths(1).AddDays(-1);

            var comercios = db.COMERCIO.Where(c => c.Estado == true).ToList();

            foreach (var comercio in comercios)
            {
                var cajas = db.CAJA.Where(c => c.IdComercio == comercio.IdComercio).ToList();
                var telefonosCaja = cajas.Select(c => c.TelefonoSINPE).ToList();

                var sinpes = db.SINPE
                    .Where(s => s.Estado == true)
                    .AsEnumerable() // cambia de LINQ-to-Entities a LINQ-to-Objects
                    .Where(s =>
                        telefonosCaja.Contains(s.TelefonoDestinatario) &&
                        s.FechaDeRegistro >= primerDiaMes &&
                        s.FechaDeRegistro <= ultimoDiaMes
                    )
                    .ToList();

                int cantidadCajas = cajas.Count;
                int cantidadSinpes = sinpes.Count;
                int montoRecaudado = (int)sinpes.Sum(s => s.Monto);
                var config = db.CONFIGURACION_DE_COMERCIO.FirstOrDefault(c => c.IdComercio == comercio.IdComercio);
                decimal porcentajeComision = config != null ? config.Comision / 100m : 0;
                decimal montoComision = montoRecaudado * porcentajeComision;

                var existente = db.REPORTESMENSUALES.FirstOrDefault(r =>
                    r.IdComercio == comercio.IdComercio &&
                    r.FechaDelReporte.Month == fechaActual.Month &&
                    r.FechaDelReporte.Year == fechaActual.Year
                );

                if (existente != null)
                {
                    var anterior = JsonConvert.SerializeObject(existente);

                    existente.CantidadDeCajas = cantidadCajas;
                    existente.CantidadDeSinpes = cantidadSinpes;
                    existente.MontoTotalRecaudado = montoRecaudado;
                    existente.MontoTotalComision = montoComision;
                    existente.FechaDelReporte = fechaActual;

                    try
                    {
                        BitacoraHelper.RegistrarEvento(
                            tabla: "REPORTES_MENSUAL",
                            tipoEvento: "Actualizar",
                            descripcion: $"Reporte mensual actualizado para comercio ID: {comercio.IdComercio}",
                            datosAnteriores: anterior,
                            datosPosteriores: existente
                        );
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error al registrar bitácora: " + ex.Message);
                    }
                }
                else
                {
                    var nuevo = new REPORTESMENSUALES
                    {
                        IdComercio = comercio.IdComercio,
                        CantidadDeCajas = cantidadCajas,
                        CantidadDeSinpes = cantidadSinpes,
                        MontoTotalRecaudado = montoRecaudado,
                        MontoTotalComision = montoComision,
                        FechaDelReporte = fechaActual
                    };

                    db.REPORTESMENSUALES.Add(nuevo);

                    try
                    {
                        BitacoraHelper.RegistrarEvento(
                            tabla: "REPORTES_MENSUAL",
                            tipoEvento: "Generar",
                            descripcion: $"Reporte mensual generado para comercio ID: {comercio.IdComercio}",
                            datosPosteriores: nuevo
                        );
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error al registrar bitácora: " + ex.Message);
                    }
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
