using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    public interface IPaginacion
    {
        Task<PaginacionModel> DevolverPaginacion(
            string storeProcedure, 
            int numPagina, 
            int cantElementos, 
            IDictionary<string,object> parametrosFiltro,
            string ordenamientoColumna);
    }
}
