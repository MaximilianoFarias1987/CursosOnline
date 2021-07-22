using Dominio;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia
{
    public class DataPrueba
    {
        public DataPrueba()
        {

        }
        public static async Task InsertarData(CursosContext context, UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any()) //Si no existe un usuario en la base de datos, que me Inserte un Usuario.
            {
                var usuario = new Usuario
                {
                    NombreCompleto = "Maximiliano Farias",
                    UserName = "maximilianocba07",
                    Email = "maximilianocba07@gmail.com"
                };
                await userManager.CreateAsync(usuario, "Usuario123$");
            }
        }
    }
}
