using RestApiCvManager.Models;
using System.ComponentModel.DataAnnotations;

namespace RestApiCvManager.Dtos.EducationDTOs
{
    public class EducationDto
    {
        [Required]
        [MaxLength(100)]
        public string PersonSchool { get; set; }
        [Required]
        [StringLength(100)]
        public string SchoolDegree { get; set; }
        [Required]
        public DateTime SchoolStartDate { get; set; }
        [Required]
        public DateTime SchoolEndDate { get; set; }

    }
}
