using System;
using PlataFormaDePagosWebApp.Models;

namespace PlataFormaDePagosWebApp.Helpers
{
    public static class BitacoraHelper
    {
        public static void Registrar(string accion, string modulo, string usuario = "admin")
        {
            using (var db = new PROYECTO_BANCO_LOS_PATITOSEntities())
            {
                db.BITACORA.Add(new BITACORA
                {
                    Usuario = usuario,
                    Fecha = DateTime.Now,
                    Accion = accion,
                    Modulo = modulo
                });
                db.SaveChanges();
            }
        }
    }
}
