using CoreVentas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreVentas.Controllers
{

    [ApiController]
    [Route("Usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly DbreactVentaContext _context;

        public UsuarioController(DbreactVentaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            List<Usuario> usuario = new List<Usuario>();
            try {
                usuario = await _context.Usuarios.Include(c => c.IdRolNavigation).OrderByDescending(c => c.IdUsuario).ToListAsync();

                return StatusCode(StatusCodes.Status200OK, usuario);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] Usuario usuario)
        {
            try {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, "ok");
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Usuario usuario)
        {
            try {
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, "ok");

            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Elininar(int id) 
        {
            try {
                Usuario usuario = _context.Usuarios.Find(id);
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK,"ok");
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
