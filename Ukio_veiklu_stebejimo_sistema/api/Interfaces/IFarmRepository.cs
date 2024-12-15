using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Farm;
using api.Models;

namespace api.Interfaces
{
    public interface IFarmRepository
    {
        Task<List<Farm>> GetAllAsync();
        Task<Farm?> GetByIdAsync(int id, string userId); 
        Task<Farm> CreateAsync(Farm farm);
        Task<Farm?> UpdateAsync(int id, UpdateFarmRequestDto farmDto, string userId);
        Task<Farm?> DeleteAsync(int id, string userId);
        Task<bool> FarmExists(int id);
    }
}