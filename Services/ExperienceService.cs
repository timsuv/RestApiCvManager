using Microsoft.EntityFrameworkCore;
using RestApiCvManager.Data;
using RestApiCvManager.Dtos.ExperienceDTOs;
using RestApiCvManager.Models;

namespace RestApiCvManager.Services
{
    public class ExperienceService
    {
        private readonly CvManagerDbContext context;
        public ExperienceService(CvManagerDbContext _context)
        {
            context = _context;
        }

        public async Task<ExperienceDto> GetExperienceById(int id)
        {
            var experience = await context.Experiences.FirstOrDefaultAsync(e => e.Id == id);
            if (experience == null)
            {
                throw new KeyNotFoundException("Experience not found");
            }
            return new ExperienceDto
            {
                PersonCompany = experience.Company,
                PersonTitle = experience.Title,
                AmountYears = experience.Years,
                TitleDescription = experience.Description
            };
        }
        public async Task<ExperienceDto> DeleteExperience(int id)
        {
            var experienceToDelete = await context.Experiences.FirstOrDefaultAsync(e => e.Id == id);
            if (experienceToDelete == null)
            {
                throw new KeyNotFoundException("Experience not found");
            }

            context.Experiences.Remove(experienceToDelete);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to delete.");
            }

            return new ExperienceDto
            {
                PersonCompany = experienceToDelete.Company,
                PersonTitle = experienceToDelete.Title,
                AmountYears = experienceToDelete.Years,
                TitleDescription = experienceToDelete.Description
            };
        }

        public async Task<ExperienceDto> ChangeExperienceById(int id, ExperienceDto experience)
        {
            var experienceToChange = await context.Experiences.FirstOrDefaultAsync(e => e.Id == id);
            if (experienceToChange == null)
            {
                throw new KeyNotFoundException("Experience not found");
            }
            var validationsResults = ValidatorHelper.ValidateInput(experience);
            if (validationsResults.Count > 0)
            {
                var error = string.Join(", ", validationsResults.Select(v => v.ErrorMessage));
                throw new ArgumentException($"Invalid format in data: {error}");
            }


            experienceToChange.Company = experience.PersonCompany;
            experienceToChange.Title = experience.PersonTitle;
            experienceToChange.Years = experience.AmountYears;
            experienceToChange.Description = experience.TitleDescription;

            await context.SaveChangesAsync();

            return new ExperienceDto
            {
                PersonCompany = experienceToChange.Company,
                PersonTitle = experienceToChange.Title,
                AmountYears = experienceToChange.Years,
                TitleDescription = experienceToChange.Description
            };
        }

        public async Task<ExperienceDto> AddExperience(ExperienceDto experience, int id)
        {
            var personToFind = await context.Persons
                 .Include(p => p.Experiences)
                 .FirstOrDefaultAsync(p => p.Id == id);

            if (personToFind == null)
            {
                throw new KeyNotFoundException("Person not found");
            }
            var validationsResults = ValidatorHelper.ValidateInput(experience);
            if (validationsResults.Count > 0)
            {
                var error = string.Join(", ", validationsResults.Select(v => v.ErrorMessage));
                throw new ArgumentException($"Invalid format in data: {error}");
            }

            var experienceToAdd = new Experience
            {
                Company = experience.PersonCompany,
                Title = experience.PersonTitle,
                Years = experience.AmountYears,
                Description = experience.TitleDescription,
                Person = personToFind
            };
            personToFind.Experiences.Add(experienceToAdd);
            await context.SaveChangesAsync();

            return new ExperienceDto
            {
                PersonCompany = experienceToAdd.Company,
                PersonTitle = experienceToAdd.Title,
                AmountYears = experienceToAdd.Years,
                TitleDescription = experienceToAdd.Description
            };
        }
    }
}
