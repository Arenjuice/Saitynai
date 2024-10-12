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
        public int HoldingNumber { get; set; }

        [Column(TypeName = "date")] 
        public DateTime YearOfFoundation { get; set; }

        public string Type { get; set; } = string.Empty;
        public List<FieldDto>? Fields { get; set; }
    }
}