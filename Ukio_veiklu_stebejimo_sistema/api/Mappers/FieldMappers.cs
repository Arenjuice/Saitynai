using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using api.Dtos.Field;
using api.Models;

namespace api.Mappers
{
    public static class FieldMappers
    {
        public static FieldDto ToFieldDto(this Field field)
        {
            return new FieldDto
            {
                Id = field.Id,
                Number = field.Number,
                CropGroup = field.CropGroup,
                CropGroupName = field.CropGroupName,
                CropSubgroup = field.CropSubgroup,
                Perimeter = field.Perimeter,
                Area = field.Area,
                FarmId = field.FarmId,
                Records = field.Records.Select(c => c.ToRecordDto()).ToList()
            };
        }

        public static Field ToFieldFromCreate(this CreateFieldDto fieldDto, int farmId)
        {
            return new Field
            {
                Number = fieldDto.Number,
                CropGroup = fieldDto.CropGroup,
                CropGroupName = fieldDto.CropGroupName,
                CropSubgroup = fieldDto.CropSubgroup,
                Perimeter = fieldDto.Perimeter,
                Area = fieldDto.Area,
                FarmId = farmId
            };
        }

        public static Field ToFieldFromUpdate(this UpdateFieldRequestDto fieldDto)
        {
            return new Field
            {
                Number = fieldDto.Number,
                CropGroup = fieldDto.CropGroup,
                CropGroupName = fieldDto.CropGroupName,
                CropSubgroup = fieldDto.CropSubgroup,
                Perimeter = fieldDto.Perimeter,
                Area = fieldDto.Area
            };
        }
    }
}