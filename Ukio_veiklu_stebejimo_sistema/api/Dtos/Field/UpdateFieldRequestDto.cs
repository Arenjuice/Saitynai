using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Field
{
    public class UpdateFieldRequestDto
    {
        [Required]
        [Column(TypeName= "int")]
        public int Number { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Pasėlių grupė negali būti ilgenė nei 10 simbolių")]
        public string CropGroup { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Pasėlių grupės pavadinimas negali būti ilgenis nei 100 simbolių")]
        public string CropGroupName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100, ErrorMessage = "Pasėlių pogrupio pavadinimas negali būti ilgenis nei 100 simbolių")]
        public string CropSubgroup { get; set; } = string.Empty;
        [Required]
        [Column(TypeName= "decimal")]
        public decimal Perimeter { get; set; }
        [Required]
        [Column(TypeName= "decimal")]
        public decimal Area { get; set; }
    }
}