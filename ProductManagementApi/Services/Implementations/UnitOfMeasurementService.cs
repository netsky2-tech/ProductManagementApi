using Microsoft.EntityFrameworkCore;
using ProductManagementApi.DTOs;
using ProductManagementApi.DTOs.UnitOfMeasurement;
using ProductManagementApi.Models;
using ProductManagementApi.Services.Interfaces;
using ProductManagementApi.Utilities;

namespace ProductManagementApi.Services.Implementations
{
    public class UnitOfMeasurementService : IUnitOfMeasurementService
    {
        private readonly AppDbContext _context;

        public UnitOfMeasurementService(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<UnitOfMeasurementDto> CreateUnitOfMeasurementAsync(UnitOfMeasurementCreateDto unitOfMeasurementDto)
        {
            var unitOfMeasurement = new UnitOfMeasurement
            {
                Name = unitOfMeasurementDto.Name,
                Abbreviation = unitOfMeasurementDto.Abbreviation
            };

            _context.UnitsOfMeasurement.Add(unitOfMeasurement);
            await _context.SaveChangesAsync();

            return new UnitOfMeasurementDto
            {
                UnitOfMeasurementId = unitOfMeasurement.UnitOfMeasurementId,
                Name = unitOfMeasurement.Name,
                Abbreviation = unitOfMeasurement.Abbreviation
            };
        }

        public async Task DeleteUnitOfMeasurementAsync(int unitOfMeasurementId)
        {
            var unit = await _context.UnitsOfMeasurement.FindAsync(unitOfMeasurementId);
            if(unit == null)
            {
                throw new KeyNotFoundException();
            }

            _context.UnitsOfMeasurement.Remove(unit);
            await _context.SaveChangesAsync();
        }

        public async Task<UnitOfMeasurement?> GetUnitOfMeasurementByNameAsync(string name)
        {
            return await _context.UnitsOfMeasurement.FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<List<ComboDTO>> GetUnitOfMeasurementOptions()
        {
            var query = _context.UnitsOfMeasurement.AsQueryable();

            var options = await query.OrderBy(c => c.Name)
                .Select(c => new ComboDTO
                {
                    Id = c.UnitOfMeasurementId,
                    Name = c.Name
                }).ToListAsync();

            return options;
        }

        public async Task<PaginatedResponse<UnitOfMeasurementDto>> GetUnitOfMeasurementsAsync(string name, int pageNumber, int pageSize)
        {
            var query = _context.UnitsOfMeasurement.AsQueryable();

            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.Name.Contains(name));
            }

            var totalItems = await query.CountAsync();

            var units = await query
                .OrderBy(u => u.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UnitOfMeasurementDto
                {
                    UnitOfMeasurementId = u.UnitOfMeasurementId,
                    Name = u.Name,
                    Abbreviation = u.Abbreviation
                })
                .ToListAsync();

            return new PaginatedResponse<UnitOfMeasurementDto>
            {
                Items = units,
                TotalCount = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task UpdateUnitOfMeasurementAsync(UnitOfMeasurementUpdateDto unitOfMeasurementDto)
        {
            var unit = await _context.UnitsOfMeasurement.FindAsync(unitOfMeasurementDto.UnitOfMeasurementId);

            if (unit == null) {

                throw new KeyNotFoundException();
            }

            unit.Name = unitOfMeasurementDto.Name;
            unit.Abbreviation = unitOfMeasurementDto.Abbreviation;

            _context.Update(unit);
            await _context.SaveChangesAsync();
        }
    }
}
