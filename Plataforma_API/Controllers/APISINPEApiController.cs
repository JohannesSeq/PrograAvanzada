using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Plataforma_API;

namespace Plataforma_API.Controllers
{
    public class APISINPEApiController : ApiController
    {
        private PROYECTO_BANCO_LOS_PATITOSEntities db = new PROYECTO_BANCO_LOS_PATITOSEntities();

        // GET: api/sinpe/consultar?telefono=88885555
        [HttpGet]
        [Route("api/sinpe/consultar")]
        public IHttpActionResult ConsultarSinpes(string telefono)
        {
            var sinpes = db.SINPE
                .Where(s => s.TelefonoDestinatario == telefono)
                .Select(s => new
                {
                    s.IdSinpe,
                    s.TelefonoOrigen,
                    s.NombreOrigen,
                    s.TelefonoDestinatario,
                    s.NombreDestinatario,
                    s.Monto,
                    s.Descripcion,
                    Fecha = s.FechaDeRegistro,
                    Estado = s.Estado
                })
                .ToList();

            return Ok(sinpes);
        }

        // POST: api/sinpe/sincronizar
        [HttpPost]
        [Route("api/sinpe/sincronizar")]
        public IHttpActionResult SincronizarSinpe([FromBody] SincronizarRequest request)
        {
            var sinpe = db.SINPE.Find(request.IdSinpe);
            if (sinpe == null)
            {
                return Ok(new { EsValido = false, Mensaje = "SINPE no encontrado" });
            }

            if (sinpe.Estado)
            {
                return Ok(new { EsValido = false, Mensaje = "Este SINPE ya fue sincronizado" });
            }

            sinpe.Estado = true;
            sinpe.FechaDeModificacion = DateTime.Now;
            db.SaveChanges();

            return Ok(new { EsValido = true, Mensaje = "SINPE sincronizado correctamente" });
        }

        // POST: api/sinpe/recibir
        [HttpPost]
        [Route("api/sinpe/recibir")]
        public IHttpActionResult RecibirSinpe([FromBody] RecibirSinpeRequest request)
        {
            try
            {
                SINPE nuevoSinpe = new SINPE
                {
                    TelefonoOrigen = request.TelefonoOrigen,
                    NombreOrigen = request.NombreOrigen,
                    TelefonoDestinatario = request.TelefonoDestinatario,
                    NombreDestinatario = request.NombreDestinatario,
                    Monto = request.Monto,
                    Descripcion = request.Descripcion,
                    FechaDeRegistro = DateTime.Now,
                    Estado = false
                };

                db.SINPE.Add(nuevoSinpe);
                db.SaveChanges();

                return Ok(new { EsValido = true, Mensaje = "SINPE recibido correctamente" });
            }
            catch (Exception ex)
            {
                return Ok(new { EsValido = false, Mensaje = "Error al registrar el SINPE: " + ex.Message });
            }
        }

        public class SincronizarRequest
        {
            public int IdSinpe { get; set; }
        }

        public class RecibirSinpeRequest
        {
            public string TelefonoOrigen { get; set; }
            public string NombreOrigen { get; set; }
            public string TelefonoDestinatario { get; set; }
            public string NombreDestinatario { get; set; }
            public decimal Monto { get; set; }
            public string Descripcion { get; set; }
        }
    }
}