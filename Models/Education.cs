using System.ComponentModel.DataAnnotations;

namespace RestApiCvManager.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string School { get; set; }
        [Required]
        [StringLength(100)]
        public string Degree { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public Person Person { get; set; }
    }
}
