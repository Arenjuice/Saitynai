using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Field;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class FieldRepository : IFieldRepository
    {
        private readonly ApplicationDBContext _context;
        public FieldRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Field> CreateAsync(Field field)
        {
            await _context.Fields.AddAsync(field);
            await _context.SaveChangesAsync();
            return field;
        }

        public async Task<Field?> DeleteAsync(int id, int farmId)
        {
            var field = await _context.Fields.Where(x => x.FarmId == farmId).Include(c => c.Records).FirstOrDefaultAsync(x => x.Id == id);

            if (field == null || field.Records.Count > 0)
            {
                return null;
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();

            return field;
        }

        public async Task<bool> FieldExists(int id)
        {
            return await _context.Fields.AnyAsync(s => s.Id == id);
        }

        public async Task<List<Field>> GetAllAsync(int farmId)
        {
            return await _context.Fields.Where(x => x.FarmId == farmId).Include(c => c.Records).ToListAsync();
        }

        public async Task<Field?> GetByIdAsync(int id, int farmId)
        {
            return await _context.Fields.Where(x => x.FarmId == farmId).Include(c => c.Records).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Field?> UpdateAsync(int id, Field field, int farmId)
        {
            var existingField = await _context.Fields.Where(x => x.FarmId == farmId).FirstOrDefaultAsync(i => i.Id == id);

            if(existingField == null)
            {
                return null;                
            }

            existingField.Number = field.Number;
            existingField.CropGroup = field.CropGroup;
            existingField.CropGroupName = field.CropGroupName;
            existingField.CropSubgroup = field.CropSubgroup;
            existingField.Perimeter = field.Perimeter;
            existingField.Area = field.Area;

            await _context.SaveChangesAsync();

            return existingField;
        }
    }
}