using RestApiCvManager.Dtos.EducationDTOs;
using RestApiCvManager.Dtos.ExperienceDTOs;
using System.ComponentModel.DataAnnotations;

namespace RestApiCvManager.Dtos.PersonDTOs
{
    public class PersonCreateDto
    {
       
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

       
    }
}
