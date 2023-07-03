using CoreVentas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreVentas.Controllers
{
    
    [ApiController]
    [Route("producto")]
    public class ProductoController : ControllerBase
    {
        private readonly DbreactVentaContext _context;

        public ProductoController(DbreactVentaContext context) 
        {
            _context = context;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista() 
        {
            List<Producto> lista = new List<Producto>();
            try {
                lista = await _context.Productos.Include(c => c.IdCategoriaNavigation).OrderByDescending(c => c.IdProducto).ToListAsync();
                
                return StatusCode(StatusCodes.Status200OK,lista);
            
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] Producto producto) 
        {
            try
            {
                await _context.Productos.AddAsync(producto);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK,"ok");
            }catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] Producto producto)
        {
            try {
                _context.Productos.Update(producto);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK,"ok");
            
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id) 
        {
            try {
                Producto producto = _context.Productos.Find(id);
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK,"ok");
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
