using CoreVentas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreVentas.Controllers
{
    
    [ApiController]
    [Route("rol")]
    public class RolController : ControllerBase
    {
        private readonly DbreactVentaContext _context;

        public RolController(DbreactVentaContext context) 
        {
            _context = context;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista() 
        {
            List<Rol> lista =new  List<Rol>();
            try {
                lista = await _context.Rols.ToListAsync();
                return StatusCode(StatusCodes.Status200OK,lista);
                
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
