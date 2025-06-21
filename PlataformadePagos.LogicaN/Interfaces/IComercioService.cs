using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlataformadePagos.Models;

namespace PlataformadePagos.LogicaN.Interfaces
{
    public interface IComercioService
    {
        List<ComercioDto> ObtenerTodos();
        ComercioDto ObtenerPorId(int id);
        void Registrar(ComercioDto comercio);
        void Editar(ComercioDto comercio);
    }
}
