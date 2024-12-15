using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Farm;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class FarmRepository : IFarmRepository
    {
        private readonly ApplicationDBContext _context;
        public FarmRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Farm> CreateAsync(Farm farm)
        {
            await _context.Farms.AddAsync(farm);
            await _context.SaveChangesAsync();
            return farm;
        }

        public async Task<Farm?> DeleteAsync(int id, string userId)
        {
            var farm = await _context.Farms.Include(f => f.Fields).FirstOrDefaultAsync(x => x.Id == id);

            if (farm == null || farm.Fields.Count > 0)
            {
                return null;
            }

            if (!farm.UserId.Contains(userId))
            {
                return null;
            }

            _context.Farms.Remove(farm);
            await _context.SaveChangesAsync();
            return farm;
        }

        public async Task<bool> FarmExists(int id)
        {
            return await _context.Farms.AnyAsync(s => s.Id == id);
        }

        public async Task<List<Farm>> GetAllAsync()
        {
            return await _context.Farms.Include(c => c.Fields).ToListAsync();
        }

        public async Task<Farm?> GetByIdAsync(int id, string userId)
        {
            var farm = await _context.Farms.Include(c => c.Fields).FirstOrDefaultAsync(i => i.Id == id);

            if (farm == null)
            {
                return null;
            }

            if (!farm.UserId.Contains(userId))
            {
                return null;
            }

            return farm;
        }

        public async Task<Farm?> UpdateAsync(int id, UpdateFarmRequestDto farmDto, string userId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(x => x.Id == id);

            if (farm == null)
            {
                return null;
            }

            if (!farm.UserId.Contains(userId))
            {
                return null;
            }

            farm.Name = farmDto.Name;
            farm.HoldingNumber = farmDto.HoldingNumber;
            farm.YearOfFoundation = farmDto.YearOfFoundation;
            farm.Type = farmDto.Type;

            await _context.SaveChangesAsync();

            return farm;
        }
    }
}