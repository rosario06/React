using CoreVentas.Models;
using CoreVentas.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Data;
using System.Globalization;

namespace CoreVentas.Controllers
{
    
    [ApiController]
    [Route("Venta")]
    public class VentaController : ControllerBase
    {
        private readonly DbreactVentaContext _context;

        public VentaController(DbreactVentaContext context) 
        {
            _context = context;
        }

        [HttpGet]
        [Route("Productos/{busqueda}")]
        public async Task<IActionResult> Productos(string busqueda) 
        {
            List<DtoProduto> lista = new List<DtoProduto>();
            try {
                lista = await _context.Productos
                        .Where(p => string.Concat(p.Codigo.ToLower(), p.Marca.ToLower(), p.Descripcion.ToLower()).Contains(busqueda.ToLower()))
                        .Select(p => new DtoProduto()
                        {
                            IdProducto = p.IdProducto,
                            Codigo = p.Codigo,
                            Marca = p.Marca,
                            Descripcion = p.Descripcion,
                            Precio = (decimal)p.Precio

                        }).ToListAsync();

                return StatusCode(StatusCodes.Status200OK,lista);
            
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpPost]
        [Route("Registrar")]
        public IActionResult Registrar([FromBody] DtoVenta repuest)
        {
            try {
                string numeroDocumento = "";
                XElement productos = new XElement("Productos");
                foreach (DtoProduto item in repuest.listaProdutos) 
                {
                    productos.Add(new XElement("item",
                        new XElement("IdProducto", item.IdProducto),
                        new XElement("Cantidad", item.Cantidad),
                        new XElement("Precio", item.Precio),
                        new XElement("Total", item.Total)

                        ));
                
                }

                using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString())) 
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_RegistrarVenta", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("documentoCliente", SqlDbType.VarChar, 40).Value = repuest.documentoCliente;
                    cmd.Parameters.Add("nombreCliente", SqlDbType.VarChar, 40).Value = repuest.nombreCliente;
                    cmd.Parameters.Add("tipoDocumento", SqlDbType.VarChar, 50).Value = repuest.tipoDocumento;
                    cmd.Parameters.Add("idUsuario", SqlDbType.Int).Value = repuest.idUsuario;
                    cmd.Parameters.Add("subTotal", SqlDbType.Decimal).Value = repuest.subTotal;
                    cmd.Parameters.Add("impuestoTotal", SqlDbType.Decimal).Value = repuest.igv;
                    cmd.Parameters.Add("total", SqlDbType.Decimal).Value = repuest.total;
                    cmd.Parameters.Add("productos", SqlDbType.Xml).Value = productos.ToString();
                    cmd.Parameters.Add("nroDocumento", SqlDbType.VarChar,6).Value = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    numeroDocumento = cmd.Parameters["nroDocumento"].Value.ToString();
                }

                return StatusCode(StatusCodes.Status200OK,new { numeroDocumento = numeroDocumento});
            
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> Listar() 
        {
            string buscarPor = HttpContext.Request.Query["buscarPor"];
            string numeroVenta = HttpContext.Request.Query["numeroVenta"];
            string fechaInicio = HttpContext.Request.Query["fechaInicio"];
            string fechaFin = HttpContext.Request.Query["fechaFin"];

            DateTime _fechainicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            DateTime _fechafin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-ES"));

            List<DtoHistoriaVenta> lista_venta = new List<DtoHistoriaVenta>();

            try {

                if (buscarPor == "fecha")
                {
                    lista_venta = await _context.Venta
                                        .Include(u => u.IdUsuarioNavigation)
                                        .Include(d => d.DetalleVenta)
                                        .ThenInclude(p => p.IdProductoNavigation)
                                        .Where(v => v.FechaRegistro.Value.Date >= _fechainicio.Date && v.FechaRegistro.Value.Date <= _fechafin.Date)
                                        .Select(v => new DtoHistoriaVenta()
                                        {
                                            FechaRegistro = v.FechaRegistro.Value.ToString("dd/MM/yyyy"),
                                            NumeroDocunto = v.NumeroDocumento,
                                            TipoDocumento = v.TipoDocumento,
                                            DocumentoCliente = v.DocumentoCliente,
                                            UsuaioRegistro = v.IdUsuarioNavigation.Nombre,
                                            SubTotalo = v.SubTotal.ToString(),
                                            Impuesto = v.ImpuestoTotal.ToString(),
                                            Total = v.Total.ToString(),
                                            Detalle = v.DetalleVenta.Select(d => new DtoDetalleVenta()
                                            {
                                                Producto = d.IdProductoNavigation.Descripcion,
                                                Cantidad = d.Cantidad.ToString(),
                                                Precio = d.Precio.ToString(),
                                                Total = d.Total.ToString()
                                            }).ToList()
                                        }).ToListAsync();
                }
                else {
                    lista_venta = await _context.Venta
                                        .Include(u => u.IdUsuarioNavigation)
                                        .Include(d => d.DetalleVenta)
                                        .ThenInclude(p => p.IdProductoNavigation)
                                        .Where(v => v.NumeroDocumento == numeroVenta)
                                        .Select(v => new DtoHistoriaVenta()
                                        {
                                            FechaRegistro = v.FechaRegistro.Value.ToString("dd/MM/yyyy"),
                                            NumeroDocunto = v.NumeroDocumento,
                                            TipoDocumento = v.TipoDocumento,
                                            DocumentoCliente = v.DocumentoCliente,
                                            NombreCliente = v.NombreCliente,
                                            UsuaioRegistro = v.IdUsuarioNavigation.Nombre,
                                            SubTotalo = v.SubTotal.ToString(),
                                            Impuesto = v.ImpuestoTotal.ToString(),
                                            Total = v.Total.ToString(),
                                            Detalle = v.DetalleVenta.Select(d => new DtoDetalleVenta()
                                            {
                                                Producto = d.IdProductoNavigation.Descripcion,
                                                Cantidad = d.Cantidad.ToString(),
                                                Precio = d.Precio.ToString(),
                                                Total = d.Total.ToString()

                                            }).ToList()
                                        }).ToListAsync();
                }

                return StatusCode(StatusCodes.Status200OK,lista_venta);
            } catch (Exception ex) {
            
                return StatusCode(StatusCodes.Status400BadRequest,ex.Message);
            }


        
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte()
        {
            string fechaInicio = HttpContext.Request.Query["fechaInicio"];
            string fechaFin = HttpContext.Request.Query["fechaFin"];

            DateTime _fechainicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-PE"));
            DateTime _fechafin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-PE"));

            List<DtoReporteVenta> lista_venta = new List<DtoReporteVenta>();
            try
            {
                lista_venta = (from v in _context.Venta
                               join d in _context.DetalleVenta on v.IdVenta equals d.IdVenta
                               join p in _context.Productos on d.IdProducto equals p.IdProducto
                               where v.FechaRegistro.Value.Date >= _fechainicio.Date && v.FechaRegistro.Value.Date <= _fechafin.Date
                               select new DtoReporteVenta()
                               {
                                   FechaRegistro = v.FechaRegistro.Value.ToString("dd/MM/yyyy"),
                                   NumeroDoumento = v.NumeroDocumento,
                                   TipoDocumento = v.TipoDocumento,
                                   DocumentoCliente = v.DocumentoCliente,
                                   NombreCliente = v.NombreCliente,
                                   SubTotalVenta = v.SubTotal.ToString(),
                                   ImpuestoTotalVenta = v.ImpuestoTotal.ToString(),
                                   TotalVenta = v.Total.ToString(),
                                   Producto = p.Descripcion,
                                   Cantidad = d.Cantidad.ToString(),
                                   Precio = d.Precio.ToString(),
                                   Total = d.Total.ToString()
                               }).ToList();



                return StatusCode(StatusCodes.Status200OK, lista_venta);
            }
            catch (Exception ex)
            {
                var str = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, lista_venta);
            }


        }
    }
}
