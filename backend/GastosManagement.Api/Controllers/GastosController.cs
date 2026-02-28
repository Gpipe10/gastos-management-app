using GastosManagement.Application.DTOs.Requests;
using GastosManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GastosController : ControllerBase
    {
        private readonly IGastoService _gastoService;

        public GastosController(IGastoService gastoService)
        {
            _gastoService = gastoService;
        }

        // GET: api/gastos
        // Retorna { total, gastos[] }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _gastoService.GetAllAsync();

            // Blindaje por si el service devuelve null
            if (result == null)
                return Ok(new { total = 0, gastos = Array.Empty<object>() });

            return Ok(result);
        }

        // GET: api/gastos/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var result = await _gastoService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/gastos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GastoCreateRequest request)
        {
            try
            {
                var created = await _gastoService.CreateAsync(request);
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

        // PUT: api/gastos/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] GastoUpdateRequest request)
        {
            try
            {
                var updated = await _gastoService.UpdateAsync(id, request);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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

        // DELETE: api/gastos/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _gastoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}