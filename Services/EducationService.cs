using Microsoft.EntityFrameworkCore;
using RestApiCvManager.Data;
using RestApiCvManager.Dtos.EducationDTOs;
using RestApiCvManager.Dtos.ExperienceDTOs;
using RestApiCvManager.Models;

namespace RestApiCvManager.Services
{
    public class EducationService
    {
        private readonly CvManagerDbContext context;
        public EducationService(CvManagerDbContext _context)
        {
            context = _context;
        }

        public async Task<EducationDto> GetEducationById(int id)
        {
            var education = await context.Educations.FirstOrDefaultAsync(e => e.Id == id);
            if (education == null)
            {
                throw new KeyNotFoundException("Education not found");
            }
            return new EducationDto
            {
                PersonSchool = education.School,
                SchoolDegree = education.Degree,
                SchoolStartDate = education.StartDate,
                SchoolEndDate = education.EndDate
            };

        }

        public async Task<EducationDto> DeleteEducation(int id)
        {
            var educationToDelete = await context.Educations.FirstOrDefaultAsync(e => e.Id == id);
            if (educationToDelete == null)
            {
                throw new KeyNotFoundException("Education not found");
            }
            
            context.Educations.Remove(educationToDelete);

            try
            {
                await context.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to delete.");
            }




            return new EducationDto
            {
                PersonSchool = educationToDelete.School,
                SchoolDegree = educationToDelete.Degree,
                SchoolStartDate = educationToDelete.StartDate,
                SchoolEndDate = educationToDelete.EndDate
            };
        }

        public async Task<EducationDto> ChangeEducationById(int id, EducationDto education)
        {

            var educationToChange = await context.Educations.FirstOrDefaultAsync(e => e.Id == id);

            if (educationToChange == null)
            {
                throw new KeyNotFoundException("Education not found");
            }
            var validationsResults = ValidatorHelper.ValidateInput(education);
            if (validationsResults.Count > 0)
            {
                var error = string.Join(", ", validationsResults.Select(v => v.ErrorMessage));
                throw new ArgumentException($"Invalid format in data: {error}");
            }

            educationToChange.School = education.PersonSchool;
            educationToChange.Degree = education.SchoolDegree;
            educationToChange.StartDate = education.SchoolStartDate;
            educationToChange.EndDate = education.SchoolEndDate;
            await context.SaveChangesAsync();

            return new EducationDto
            {
                PersonSchool = education.SchoolDegree,
                SchoolDegree = education.SchoolDegree,
                SchoolStartDate = education.SchoolStartDate,
                SchoolEndDate = education.SchoolEndDate
            };



        }

        public async Task<EducationDto> AddEducation(int personId, EducationDto newEducation)
        {
            var personToFind = await context.Persons
                .Include(p => p.Educations)
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (personToFind == null)
            {
                throw new KeyNotFoundException("Person not found");
            }

            var validationsResults = ValidatorHelper.ValidateInput(newEducation);
            if (validationsResults.Count > 0)
            {
                var error = string.Join(", ", validationsResults.Select(v => v.ErrorMessage));
                throw new ArgumentException($"Invalid format in data: {error}");
            }


            var education = new Education
            {
                School = newEducation.PersonSchool,
                Degree = newEducation.SchoolDegree,
                StartDate = newEducation.SchoolStartDate,
                EndDate = newEducation.SchoolEndDate,
                Person = personToFind

            };
            personToFind.Educations.Add(education);
            await context.SaveChangesAsync();

            return new EducationDto
            {
                PersonSchool = newEducation.PersonSchool,
                SchoolDegree = newEducation.SchoolDegree,
                SchoolStartDate = newEducation.SchoolStartDate,
                SchoolEndDate = newEducation.SchoolEndDate
            };
        }
    }
}
