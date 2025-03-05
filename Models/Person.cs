
using System.ComponentModel.DataAnnotations;

namespace RestApiCvManager.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        
        public string Email { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(600)]
        public string Description { get; set; }
        [Required]
        [MaxLength(11)] //swedish number
        public string Phone { get; set; }

        public List<Education> Educations { get; set; }

        
    }
}
