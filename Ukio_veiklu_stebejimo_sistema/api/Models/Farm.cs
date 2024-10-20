using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Farm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HoldingNumber { get; set; } = string.Empty;

        public int YearOfFoundation { get; set; }
        public string Type { get; set; } = string.Empty;
        public List<Field> Fields { get; set; } = new List<Field>();
    }
}