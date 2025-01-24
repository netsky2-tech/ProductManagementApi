using ProductManagementApi.DTOs;
using ProductManagementApi.DTOs.UnitOfMeasurement;
using ProductManagementApi.Models;
using ProductManagementApi.Utilities;

namespace ProductManagementApi.Services.Interfaces
{
    public interface IUnitOfMeasurementService
    {
        Task<PaginatedResponse<UnitOfMeasurementDto>> GetUnitOfMeasurementsAsync(string name, int pageNumber, int pageSize);
        Task<UnitOfMeasurementDto> CreateUnitOfMeasurementAsync(UnitOfMeasurementCreateDto unitOfMeasurementDto);
        Task UpdateUnitOfMeasurementAsync(UnitOfMeasurementUpdateDto unitOfMeasurementDto);
        Task DeleteUnitOfMeasurementAsync(int unitOfMeasurementId);
        Task<UnitOfMeasurement?> GetUnitOfMeasurementByNameAsync(string name);
        Task<List<ComboDTO>> GetUnitOfMeasurementOptions();
    }
}
