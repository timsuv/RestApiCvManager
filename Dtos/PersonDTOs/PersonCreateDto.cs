using RestApiCvManager.Dtos.EducationDTOs;
using RestApiCvManager.Dtos.ExperienceDTOs;
using System.ComponentModel.DataAnnotations;

namespace RestApiCvManager.Dtos.PersonDTOs
{
    public class PersonCreateDto
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [EmailAddress]

        public string PersonEmail { get; set; }
        [MaxLength(50)]
        [Required]
        public string PersonName { get; set; }
        [Required]
        [MaxLength(600)]
        public string PersonDescription { get; set; }
        [Required]
        [MaxLength(11)] //swedish number
        public string PersonPhone { get; set; }

        public List<EducationDto>? Educations { get; set; }
        public List<ExperienceDto>? Experiences { get; set; }
    }
}
