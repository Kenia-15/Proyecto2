using aplicacion_proyecto2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace aplicacion_proyecto2.Controllers
{
    public class IniciarSesionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly db_carritoContext _context;
        public IConfiguration Configuration { get; }

        public IniciarSesionController(IConfiguration configuration, db_carritoContext context)
        {
            Configuration = configuration;
            _context = context;
        }

        public IActionResult IniciarSesion(int idU)
        {
            TempData["Usuario"] = idU;
            TempData["Header"] = "N";
            return View();
        }


        [HttpPost]
        public IActionResult IniciarSesion(IniciarSesion usuario, int idU)
        {
            var validaUsuario = 0;
            var query = "";
            TempData["Header"] = "N";
            TempData["Usuario"] = idU;

            if (usuario.email == null || usuario.contrasena == null)
            {
                TempData["Mensaje"] = "Debe ingresar el usuario y la contraseña";
            } 
            else {
                //Se consulta si existe un usuario con las credenciales suministradas
                try
                {
                    query = "Select 1 from db_carrito.dbo.tbl_usuarios t where t.email = '" + usuario.email + "' and t.contrasena = '" + usuario.contrasena + "';";

                    using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                    {
                        using (SqlCommand com = new SqlCommand(query, sqlConn))
                        {
                            sqlConn.Open();

                            using (SqlDataReader read = com.ExecuteReader())
                            {
                                while (read.Read())
                                {
                                    validaUsuario = read.GetInt32(0);
                                }
                            }
                        }
                    }
                }catch (Exception ex)
                {
                    validaUsuario = 0;
                    TempData["Mensaje"] = ex;
                }
            }

            if (validaUsuario == 0 && (usuario.email != null || usuario.contrasena != null))
            {
                TempData["Mensaje"] = "El usuario o la contraseña son incorrectos";
            } 
            else if (usuario.email == null || usuario.contrasena == null)
            {
                TempData["Mensaje"] = "Debe ingresar un usuario y contraseña";
            }
            else
            {
                //Se obtiene el identificador del usuario para pasarlo al controlador PaginaPrincipalController
                var idUsuario = (from i in _context.TblUsuarios where i.Email == usuario.email && i.Contrasena == usuario.contrasena select i.IdUsuario);
                
                TempData["Usuario"] = idUsuario.FirstOrDefault();
            }

            var idPersona = (from i in _context.TblUsuarios where i.Email == usuario.email && i.Contrasena == usuario.contrasena select i.IdPersona);

            TempData["Persona"] = idPersona.FirstOrDefault();

            if (TempData["Usuario"] == null)
            {
                TempData["Mensaje"] = "Debe ingresar un usuario y contraseña";
                return View();
            } else
            {
                return RedirectToAction("Index", "TblCategorias", new { idU = idU });
            }            
        }

    }
}
