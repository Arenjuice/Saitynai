using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Record;

namespace api.Dtos.Field
{
    public class FieldDto
    {
        public int Id { get; set; }
        public int? FarmId { get; set; }
        public int Number { get; set; }
        public string CropGroup { get; set; } = string.Empty;
        public string CropGroupName { get; set; } = string.Empty;
        public string CropSubgroup { get; set; } = string.Empty;
        public decimal Perimeter { get; set; }
        public decimal Area { get; set; }
        public List<RecordDto>? Records { get; set; }
    }
}