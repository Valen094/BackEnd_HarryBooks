using Microsoft.AspNetCore.Mvc;
using System.Data;
using harryPotter.Modelos;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;

namespace harryPotter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LibroController : ControllerBase
    {

        private readonly string cadenaSql;
        public LibroController(IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Comprar")]
        public IActionResult Comprar([FromQuery] int id, [FromQuery] int quantity)
        {
            string q = $"execute sp_disminuir_stock {id}, {quantity}";
            var conexion = new SqlConnection(cadenaSql);
            conexion.Open();
            try
            {
                new SqlCommand(q, conexion).ExecuteNonQuery();
                return Ok("Producto rebajado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<Libro> lista = new();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_libro", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using var rd = cmd.ExecuteReader();

                    while (rd.Read())
                    {
                        lista.Add(new Libro
                        {
                            id_libro = Convert.ToInt32(rd["id_libro"]),
                            img = rd["img"].ToString(),
                            nombre = rd["nombre"].ToString(),
                            stock = Convert.ToInt32(rd["stock"]),
                            precio = Convert.ToInt32(rd["precio"])

                        });
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = lista });
            }
        }
        [HttpGet]
        [Route("Obtener/{id_libro:int}")]
        public IActionResult obtener(int id_libro)
        {
            List<Libro> lista = new List<Libro>();
            Libro libros = new Libro();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_libro", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using var rd = cmd.ExecuteReader();
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Libro
                            {
                                id_libro = Convert.ToInt32(rd["id_libro"]),
                                img = rd["img"].ToString(),
                                nombre = rd["nombre"].ToString(),
                                stock = Convert.ToInt32(rd["stock"]),
                                precio = Convert.ToInt32(rd["precio"])

                            });

                        }
                    }
                }
                libros = lista.Where(item => item.id_libro == id_libro).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = libros });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = libros });
            }
        }
        [HttpPost]
        [Route("registrar")]
        public IActionResult Registrar([FromBody] Libro objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_libro", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id_libro", objeto.id_libro);
                    cmd.Parameters.AddWithValue("img", objeto.img);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("stock", objeto.stock);
                    cmd.Parameters.AddWithValue("precio", objeto.precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El libro fue registrado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Libro objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_actualizarMunicipios", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id_libro", objeto.id_libro == 0 ? DBNull.Value : objeto.id_libro);
                    cmd.Parameters.AddWithValue("img", objeto.img is null ? DBNull.Value : objeto.img);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("stock", objeto.stock == 0 ? DBNull.Value : objeto.stock);
                    cmd.Parameters.AddWithValue("precio", objeto.stock == 0 ? DBNull.Value : objeto.precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El municipio ha sido actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("eliminar")]
        public IActionResult Editar(int id_libro)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_libro", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id_libro", id_libro);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El libro ha sido eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}
