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
                if (ex is System.Data.Entity.Validation.DbEntityValidationException validationEx)
                {
                    foreach (var failure in validationEx.EntityValidationErrors)
                    {
                        foreach (var error in failure.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine($"[BITACORA] Campo inválido: {error.PropertyName} - {error.ErrorMessage}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error al registrar en bitácora: " + ex.Message);
                }
            }


        }
    }
}
