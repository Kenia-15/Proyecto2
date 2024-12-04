using aplicacion_proyecto2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace aplicacion_proyecto2.Controllers
{
    public class RegistroController : Controller
    {
        private readonly db_carritoContext _context;

        public IConfiguration Configuration { get; }

        public RegistroController(db_carritoContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registrar()
        {
            ViewData["MetodosPago"] = new SelectList(_context.TblMetodosPagos, "IdMetodoPago", "MetodoPago");
            TempData["Header"] = "N";
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(MdlRegistro registro)
        {
            int idPersona = 0;
            var valido = false;

            TempData["Header"] = "N";

            idPersona = insertarPersona(registro);
            valido = insertarUsuario(registro, idPersona);

            //Si se insertó correctamente el usuario se redirecciona a la página de inicio de sesión
            if (valido == true)
            {
                return RedirectToAction("IniciarSesion", "IniciarSesion");
            }
            //de lo contrario se mantiene en la misma vista
            else
            {
                return View();
            }
        }

        public int insertarPersona(MdlRegistro persona)
        {
            int secuenciaIdPer = 0;
            var query = "";

            if (!validarPersona(persona))
            {
                TempData["Mensaje"] = "Debe completar los campos requeridos (*)";
            }
            else
            {
                //Se inserta a la persona en la tabla tbl_personas
                try
                {
                    query = "insert into db_carrito.dbo.tbl_personas(id_metodo_pago, numero_identificacion, primer_nombre, segundo_nombre, primer_apellido, segundo_apellido) values('" + persona.personas.IdMetodoPago + "', '" + persona.personas.NumeroIdentificacion + "', '" + persona.personas.PrimerNombre + "', '" + persona.personas.SegundoNombre + "', '" + persona.personas.PrimerApellido + "', '" + persona.personas.SegundoApellido + "');";

                    using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                    {
                        using (SqlCommand com = new SqlCommand(query, sqlConn))
                        {
                            sqlConn.Open();
                            com.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = ex.ToString();
                }
            }

            //Se obtiene el último índice de la tabla
            secuenciaIdPer = _context.TblPersonas.Max(p => p.IdPersona);

            //Se setea el id de la persona para crear el usuario
            return secuenciaIdPer;
        }


        /*Función que devuelve un boleano para validar que los datos de la persona no vayan vacíos*/
        public bool validarPersona(MdlRegistro persona)
        {
            if (persona.personas.NumeroIdentificacion == null || persona.personas.PrimerNombre == null || persona.personas.PrimerApellido == null || persona.personas.SegundoApellido == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Función que devuelve un booleano para validar que las credenciales del usuario no vayan vacías*/
        public bool validarUsuario(MdlRegistro usuario)
        {
            if (usuario.usuarios.Email == null || usuario.usuarios.Contrasena == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*Función encargada de insertar las credenciales del usuario en la tabla tbl_usuarios*/
        public bool insertarUsuario(MdlRegistro usuario, int IdPersona)
        {
            int identificadorPersona = 0;
            var flag = false;
            var query = "";
            var correo = 0;

            //Se obtiene el id de la persona
            identificadorPersona = IdPersona;

            if (!validarUsuario(usuario))
            {
                ViewData["MetodosPago"] = new SelectList(_context.TblMetodosPagos, "IdMetodoPago", "MetodoPago");
                TempData["Mensaje"] = "Debe completar el usuario y la contraseña";
            }
            else
            {
                //Se inserta el registro de las credenciales de la persona en la tabla tbl_usuarios
                try
                {
                    query = "insert into db_carrito.dbo.tbl_usuarios(id_persona, email, contrasena, estado) values('" + identificadorPersona + "', '" + usuario.usuarios.Email + "', '" + usuario.usuarios.Contrasena + "', 'A');";

                    using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                    {
                        using (SqlCommand com = new SqlCommand(query, sqlConn))
                        {
                            sqlConn.Open();
                            com.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }

                    TempData["MensajeExito"] = "Registro creado con éxito";
                    flag = true;
                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = ex.ToString();
                    flag = false;
                }
            }
            //bandera para determar si se insertó correctamente el usuario
            return flag;
        }
    }
}
