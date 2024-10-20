using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Record
    {
        public int Id { get; set; }
        public int? FieldId { get; set; }
        public Field? Field { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateOnly Date { get; set; } = DateOnly.MinValue;
        public string Description { get; set; } = string.Empty;

    }
}