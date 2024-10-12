using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Record;
using api.Models;

namespace api.Mappers
{
    public static class RecordMappers
    {
        public static RecordDto ToRecordDto(this Record record)
        {
            return new RecordDto
            {
                Id = record.Id,
                Name = record.Name,
                Type = record.Type,
                Date = record.Date,
                Description = record.Description,
                FieldId = record.FieldId
            };
        }

        public static Record ToRecordFromCreate(this CreateRecordDto recordDto, int fieldId)
        {
            return new Record
            {
                Name = recordDto.Name,
                Type = recordDto.Type,
                Description = recordDto.Description,
                Date = DateTime.Now,
                FieldId = fieldId
            };
        }

        public static Record ToRecordFromUpdate(this UpdateRecordRequestDto fieldDto)
        {
            return new Record
            {
                Name = fieldDto.Name,
                Type = fieldDto.Type,
                Date = DateTime.Now,
                Description = fieldDto.Description,
            };
        }
    }
}