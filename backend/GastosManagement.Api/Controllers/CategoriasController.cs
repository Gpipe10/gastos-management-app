using GastosManagement.Application.DTOs.Requests;
using GastosManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoriaService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/categorias/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null)
                return NotFound(new { message = "Categoría no encontrada." });

            return Ok(categoria);
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoriaCreateRequest request)
        {
            try
            {
                var created = await _categoriaService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/categorias/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoriaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // por ejemplo: no se puede borrar porque está asociada a gastos
                return Conflict(new { message = ex.Message });
            }
        }
    }
}