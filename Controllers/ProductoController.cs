using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Models;

namespace Products.Controllers;

[EnableCors("ReglasCors")]
[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    public readonly productosContext _dbContext;

    public ProductoController(productosContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("lista")]
    public IActionResult Lista()
    {
        List<Producto> lista = new List<Producto>();

        try
        {
            lista = _dbContext.Productos.Include(c => c.oCategoria).ToList();

            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message, response = lista });
        }
    }
    
    [HttpGet]
    [Route("Obtener/{idProducto:int}")]
    public IActionResult Obtener(int idProducto)
    {
        Producto oProducto = _dbContext.Productos.Find(idProducto);
        
        if (oProducto == null)
        {
            return BadRequest("Producto no encontrado");
        }

        try
        {
            oProducto = _dbContext.Productos.Include(c => c.oCategoria).Where(p => p.IdProducto == idProducto).FirstOrDefault();
            
            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oProducto });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status200OK, new { mensaje = e.Message, response = oProducto });
        }
    }
    
    [HttpPost]
    [Route("Guardar")]
    public IActionResult Guardar([FromBody] Producto objeto)
    {
        try
        {
            _dbContext.Productos.Add(objeto);

            _dbContext.SaveChanges();
            
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
        }
    }
    
    [HttpPut]
    [Route("Editar")]
    public IActionResult Editar([FromBody] Producto objeto)
    {
        Producto oProducto = _dbContext.Productos.Find(objeto.IdProducto);
        
        if (oProducto == null)
        {
            return BadRequest("Producto no encontrado");
        }
        try
        {
            oProducto.CodigoBarra = objeto.CodigoBarra is null ? oProducto.CodigoBarra : objeto.CodigoBarra;
            oProducto.Descripcion = objeto.Descripcion is null ? oProducto.Descripcion : objeto.Descripcion;
            oProducto.Marca = objeto.Marca is null ? oProducto.Marca : objeto.Marca;
            oProducto.IdCategoria = objeto.IdCategoria is null ? oProducto.IdCategoria : objeto.IdCategoria;
            oProducto.Precio = objeto.Precio is null ? oProducto.Precio : objeto.Precio;
            
            _dbContext.Productos.Update(oProducto);
            _dbContext.SaveChanges();
            
            return StatusCode(StatusCodes.Status200OK);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
        }
    }
    
    [HttpDelete]
    [Route("Eliminar/{idProducto:int}")]
    public IActionResult Eliminar(int idProducto)
    {
        Producto oProducto = _dbContext.Productos.Find(idProducto);
        
        if (oProducto == null)
        {
            return BadRequest("Producto no encontrado");
        }
        try
        {
            _dbContext.Productos.Remove(oProducto);
            _dbContext.SaveChanges();
            
            return StatusCode(StatusCodes.Status200OK);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
        }
    }
}