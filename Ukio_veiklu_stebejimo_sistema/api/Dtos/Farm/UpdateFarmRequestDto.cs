using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Farm
{
    public class UpdateFarmRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Pavadinimas negali būti trumpesnis nei 5 simboliai")]
        [MaxLength(280, ErrorMessage = "Pavadinimas negali būti ilgenis nei 280 simbolių")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Column(TypeName= "int")]
        public int HoldingNumber { get; set; }
        [Required]
        [Column(TypeName = "date")] 
        public DateTime YearOfFoundation { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Tipas negali būti trumpesnis nei 5 simboliai")]
        [MaxLength(280, ErrorMessage = "Tipas negali būti ilgenis nei 280 simbolių")]
        public string Type { get; set; } = string.Empty;
    }
}