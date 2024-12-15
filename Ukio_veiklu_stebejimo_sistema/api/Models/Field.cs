using api.Auth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Field
    {
        public int Id { get; set; }
        public int? FarmId { get; set; }
        public Farm? Farm { get; set; }
        public int Number { get; set; }
        public string CropGroup { get; set; } = string.Empty;
        public string CropGroupName { get; set; } = string.Empty;
        public string CropSubgroup { get; set; } = string.Empty;
        public decimal Perimeter { get; set; }
        public decimal Area { get; set; }
        public List<Record> Records { get; set; } = new List<Record>();

        [Required]
        public required string UserId { get; set; }
        public SystemUser? User { get; set; }
    }
}