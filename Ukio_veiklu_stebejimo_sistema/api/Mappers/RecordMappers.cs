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

        public static Record ToRecordFromCreate(this CreateRecordDto recordDto, int fieldId, string userId)
        {
            return new Record
            {
                Name = recordDto.Name,
                Type = recordDto.Type,
                Description = recordDto.Description,
                Date = recordDto.Date,
                FieldId = fieldId,
                UserId = userId
            };
        }

        public static Record ToRecordFromUpdate(this UpdateRecordRequestDto recordDto, string userId)
        {
            return new Record
            {
                Name = recordDto.Name,
                Type = recordDto.Type,
                Date = recordDto.Date,
                Description = recordDto.Description,
                UserId = userId
            };
        }
    }
}