using System;
using Newtonsoft.Json;
using PlataFormaDePagosWebApp;

namespace PlataFormaDePagosWebApp.Helpers
{
    public static class BitacoraHelper
    {
        public static void RegistrarEvento(
            string tabla,
            string tipoEvento,
            string descripcion,
            object datosAnteriores = null,
            object datosPosteriores = null,
            string stackTrace = "")
        {
            try
            {
                using (var db = new PROYECTO_BANCO_LOS_PATITOSEntities())
                {
                    var evento = new BITACORA
                    {
                        TablaDeEvento = tabla,
                        TipoDeEvento = tipoEvento,
                        FechaDeEvento = DateTime.Now,
                        DescripcionDeEvento = descripcion,
                        StackTrace = stackTrace ?? "",
                        DatosAnteriores = datosAnteriores != null ? JsonConvert.SerializeObject(datosAnteriores) : null,
                        DatosPosteriores = datosPosteriores != null ? JsonConvert.SerializeObject(datosPosteriores) : null
                    };

                    db.BITACORA.Add(evento);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // En caso de que falle el registro en bitácora, no lanzar excepción para no romper el flujo principal
                System.Diagnostics.Debug.WriteLine("Error al registrar en bitácora: " + ex.Message);
            }
        }
    }
}
