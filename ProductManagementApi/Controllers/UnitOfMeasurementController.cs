using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.DTOs.UnitOfMeasurement;
using ProductManagementApi.Models;
using ProductManagementApi.Services.Interfaces;
using ProductManagementApi.Utilities;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfMeasurementController : ControllerBase
    {
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;
        private readonly AppDbContext _context;

        public UnitOfMeasurementController(IUnitOfMeasurementService unitOfMeasurementService, AppDbContext context)
        {
            _unitOfMeasurementService = unitOfMeasurementService;
            _context = context;
        }

        //Get unit of measurements with filters
        [HttpGet]
        public async Task<IActionResult> GetUnitOfMeasurements([FromQuery] string? name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new ApiResponse<object>(false, "Parámetros de paginación inválidos."));
            }

            try
            {
                var result = await _unitOfMeasurementService.GetUnitOfMeasurementsAsync(name, pageNumber, pageSize);
                return Ok(new ApiResponse<PaginatedResponse<UnitOfMeasurementDto>>(true, "Unidades de medida obtenidas exitosamente", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }


        // Create new unit of measurement
        [HttpPost]
        public async Task<IActionResult> CreateUnitOfMeasurement([FromBody] UnitOfMeasurementCreateDto unitOfMeasurementDto)
        {
            if (unitOfMeasurementDto == null)
            {
                return BadRequest(new ApiResponse<object>(false, "Los datos de la unidad de medida no pueden estar vacios."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(false, "Datos inválidos.", null, ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            try
            {
                var existingUnitOfMeasurement = await _unitOfMeasurementService.GetUnitOfMeasurementByNameAsync(unitOfMeasurementDto.Name);
                if (existingUnitOfMeasurement != null)
                {
                    return BadRequest(new ApiResponse<object>(false, "Ya existe una unidad de medida con el mismo nombre."));
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                var result = await _unitOfMeasurementService.CreateUnitOfMeasurementAsync(unitOfMeasurementDto);

                await transaction.CommitAsync();

                return Ok(new ApiResponse<UnitOfMeasurementDto>(true, "Unidad de medida creada exitosamente", result));
            }
            catch (DbUpdateException ex)
            { 
                return StatusCode(500, new ApiResponse<object>(false, "Error de base de datos.", null, new List<string> { ex.InnerException?.Message ?? ex.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }

        // Update a unit of measurement
        [HttpPut("{unitOfMeasurementId}")]
        public async Task<IActionResult> UpdateUnitOfMeasurement([FromBody] UnitOfMeasurementUpdateDto unitOfMeasurementDto)
        {
            if (unitOfMeasurementDto == null)
            {
                return BadRequest(new ApiResponse<object>(false, "Los datos de la unidad de medida no pueden estar vacios."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(false, "Datos inválidos.", null, ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            try
            {

                await _unitOfMeasurementService.UpdateUnitOfMeasurementAsync(unitOfMeasurementDto);

                return Ok(new ApiResponse<object>(true, "Unidad de medida actualizada exitosamente"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>(false, "Unidad de medida no encontrada."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }

        //Detele a category
        [HttpDelete("{unitOfMeasurementId}")]
        public async Task<IActionResult> DeleteUnitOfMeasuremente(int unitOfMeasurementId)
        {
            try
            {
                await _unitOfMeasurementService.DeleteUnitOfMeasurementAsync(unitOfMeasurementId);
                return Ok(new ApiResponse<object>(true, "Unidad de medida eliminada exitosamente."));

            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>(false, "Unidad de medida no encontrada."));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }

        //Get unit of measurement options
        [HttpGet("options")]
        public async Task<IActionResult> GetOptions()
        {
            try
            {
                var result = await _unitOfMeasurementService.GetUnitOfMeasurementOptions();

                return Ok(new ApiResponse<object>(true, "Unidad de medidas obtenidas correctamente", result));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }
    }
}
