using RestApiCvManager.Models;
using System.ComponentModel.DataAnnotations;

namespace RestApiCvManager.Dtos.ExperienceDTOs
{
    public class ExperienceDto
    {
        [Required]
        [MaxLength(100)]
        public string PersonCompany { get; set; }
        [Required]
        [MaxLength(100)]
        public string PersonTitle { get; set; }
        [Required]
        [Range(0, 100)]
        public int AmountYears { get; set; }
        [Required]
        [MaxLength(600)]
        public string TitleDescription { get; set; }
    }
}
