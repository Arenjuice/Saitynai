using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IRecordRepository
    {
        Task<List<Record>> GetAllAsync(int fieldId, int farmId, string userId);
        Task<Record?> GetByIdAsync(int id, int fieldId, int farmId, string userId);
        Task<Record> CreateAsync(Record record, int fieldId, int farmId, string userId);
        Task<Record?> UpdateAsync(int id, Record record, int fieldId, int farmId, string userId);
        Task<Record?> DeleteAsync(int id, int fieldId, int farmId, string userId);
    }
}