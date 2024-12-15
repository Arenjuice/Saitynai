using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ApplicationDBContext _context;
        public RecordRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Record> CreateAsync(Record record, int fieldId, int farmId, string userId)
        {
            var farm = await _context.Farms.Include(f => f.Fields).ThenInclude(f => f.Records).FirstOrDefaultAsync(f => f.Id == farmId);

            if (farm == null)
                return null;

            if (!farm.UserId.Contains(userId))
            {
                return null;
            }

            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Record?> DeleteAsync(int id, int fieldId, int farmId, string userId)
        {
            var farm = await _context.Farms.Include(f => f.Fields).ThenInclude(f => f.Records).FirstOrDefaultAsync(f => f.Id == farmId);
            
            if (farm == null)
                return null;

            if (!farm.UserId.Contains(userId))
            {
                return null;
            }

            var field = farm.Fields.FirstOrDefault(f => f.Id == fieldId);
            
            if (field == null)
                return null;

            var record = field.Records.FirstOrDefault(x => x.Id == id);

            if (record == null)
                return null;

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();

            return record;
        }

        public async Task<List<Record>> GetAllAsync(int fieldId, int farmId, string userId)
        {
            var farm = await _context.Farms.Include(f => f.Fields).ThenInclude(f => f.Records).FirstOrDefaultAsync(f => f.Id == farmId);
            
            if (farm == null)
                return null;

            if (!farm.UserId.Contains(userId))
            {
                return null;
            }

            var field = farm.Fields.FirstOrDefault(f => f.Id == fieldId);
            
            if (field == null)
                return null;

            return field.Records.ToList();
        }

        public async Task<Record?> GetByIdAsync(int id, int fieldId, int farmId, string userId)
        {
            var farm = await _context.Farms.Include(f => f.Fields).ThenInclude(f => f.Records).FirstOrDefaultAsync(f => f.Id == farmId);
            
            if (farm == null)
                return null;

            if (!farm.UserId.Contains(userId))
            {
                return null;
            }

            var field = farm.Fields.FirstOrDefault(f => f.Id == fieldId);
            
            if (field == null)
                return null;

            return field.Records.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Record?> UpdateAsync(int id, Record record, int fieldId, int farmId, string userId)
        {

            var farm = await _context.Farms.Include(f => f.Fields).ThenInclude(f => f.Records).FirstOrDefaultAsync(f => f.Id == farmId);
            
            if (farm == null)
                return null;

            if (!farm.UserId.Contains(userId))
            {
                return null;
            }

            var field = farm.Fields.FirstOrDefault(f => f.Id == fieldId);
            
            if (field == null)
                return null;

            var existingRecord = field.Records.FirstOrDefault(x => x.Id == id);

            if(existingRecord == null)
                return null;                

            existingRecord.Name = record.Name;
            existingRecord.Type = record.Type;
            existingRecord.Date = record.Date;
            existingRecord.Description = record.Description;

            await _context.SaveChangesAsync();

            return existingRecord;
        }
    }
}