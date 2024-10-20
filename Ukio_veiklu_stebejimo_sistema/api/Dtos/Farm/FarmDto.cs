using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Field;

namespace api.Dtos.Farm
{
    public class FarmDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HoldingNumber { get; set; } = string.Empty;
        public int YearOfFoundation { get; set; }

        public string Type { get; set; } = string.Empty;
        public List<FieldDto>? Fields { get; set; }
    }
}