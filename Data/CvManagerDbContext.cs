using Microsoft.EntityFrameworkCore;
using RestApiCvManager.Models;

namespace RestApiCvManager.Data
{
    public class CvManagerDbContext : DbContext
    {
        public CvManagerDbContext(DbContextOptions<CvManagerDbContext> options) : base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
            

    }
}
