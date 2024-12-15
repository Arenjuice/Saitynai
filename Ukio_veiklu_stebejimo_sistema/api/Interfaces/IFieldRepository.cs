using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Field;
using api.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace api.Interfaces
{
    public interface IFieldRepository
    {
        Task<List<Field>> GetAllAsync(int farmId, string userId);
        Task<Field?> GetByIdAsync(int id, int farmId, string userId);
        Task<Field> CreateAsync(Field field, string userId);
        Task<Field?> UpdateAsync(int id, Field field, int farmId, string userId);
        Task<Field?> DeleteAsync(int id, int farmId, string userId);
        Task<bool> FieldExists(int id);
    }
}