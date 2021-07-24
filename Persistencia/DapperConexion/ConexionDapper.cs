using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.DapperConexion
{
    public class ConexionDapper
    {
        public string ConexionSQL { get; set; }
        public string DefaultConnection { get; internal set; }
    }
}
