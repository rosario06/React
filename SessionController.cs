using CoreVentas.Models;
using CoreVentas.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreVentas.Controllers
{
    
    [ApiController]
    [Route("session")]
    public class SessionController : ControllerBase
    {
        private readonly DbreactVentaContext _context;

        public SessionController(DbreactVentaContext context) 
        {
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Dtosesion request) 
        {
            Usuario usuario = new Usuario();
            try {
                usuario = _context.Usuarios.Include(c => c.IdRolNavigation).Where(u => u.Correo == request.correo && u.Clave == request.clave).FirstOrDefault();
                if (usuario == null)
                    usuario = new Usuario();

                return StatusCode(StatusCodes.Status200OK,usuario);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
