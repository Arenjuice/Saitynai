using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Farm;
using api.Models;

namespace api.Mappers
{
    public static class FarmMappers
    {
        public static FarmDto ToFarmDto(this Farm farmModel)
        {
            return new FarmDto
            {
                Id = farmModel.Id,
                Name = farmModel.Name,
                HoldingNumber = farmModel.HoldingNumber,
                YearOfFoundation = farmModel.YearOfFoundation,
                Type = farmModel.Type,
                Fields = farmModel.Fields.Select(c => c.ToFieldDto()).ToList()
            };
        }

        public static Farm ToFarmFromCreateDto(this CreateFarmRequestDto farmDto)
        {
            return new Farm
            {
                Name = farmDto.Name,
                HoldingNumber = farmDto.HoldingNumber,
                YearOfFoundation = farmDto.YearOfFoundation,
                Type = farmDto.Type
            };
        }
    }
}