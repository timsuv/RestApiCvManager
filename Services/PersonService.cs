using Microsoft.EntityFrameworkCore;
using RestApiCvManager.Data;
using RestApiCvManager.Dtos.EducationDTOs;
using RestApiCvManager.Dtos.ExperienceDTOs;
using RestApiCvManager.Dtos.PersonDTOs;
using RestApiCvManager.Models;
using System.ComponentModel.DataAnnotations;

namespace RestApiCvManager.Services
{
    public class PersonService
    {
        private readonly CvManagerDbContext context;
        public PersonService(CvManagerDbContext _context)
        {
            context = _context;
        }

        public async Task<List<PersonDto>> GetAllUsers()
        {


            var persons = await context.Persons
               .Include(p => p.Educations)
               .Include(p => p.Experiences)
                   .Select(p => new PersonDto
                   {
                       PersonName = p.Name,
                       PersonEmail = p.Email,
                       PersonPhone = p.Phone,
                       PersonDescription = p.Description,
                       Educations = p.Educations.Select(e => new EducationDto
                       {
                           PersonSchool = e.School,
                           SchoolDegree = e.Degree,
                           SchoolStartDate = e.StartDate,
                           SchoolEndDate = e.EndDate
                       }).ToList(),
                       Experiences = p.Experiences.Select(e => new ExperienceDto
                       {
                           PersonCompany = e.Company,
                           PersonTitle = e.Title,
                           AmountYears = e.Years,
                           TitleDescription = e.Description

                       }).ToList()
                   })
                   .ToListAsync();
            if (persons == null)
            {
                throw new KeyNotFoundException("No persons found");
            }
            return persons;
        }

        public async Task<PersonDto> GetUserById(int id)
        {
            var person = await context.Persons
              .Include(p => p.Educations)
              .Include(p => p.Experiences)
              .FirstOrDefaultAsync(p => p.Id == id);


            if (person == null)
            {
                throw new KeyNotFoundException("Person not found");
            }
            return new PersonDto
            {
                PersonName = person.Name,
                PersonEmail = person.Email,
                PersonPhone = person.Phone,
                PersonDescription = person.Description,
                Educations = person.Educations.Select(e => new EducationDto
                {
                    PersonSchool = e.School,
                    SchoolDegree = e.Degree,
                    SchoolStartDate = e.StartDate,
                    SchoolEndDate = e.EndDate
                }).ToList(),
                Experiences = person.Experiences.Select(e => new ExperienceDto
                {
                    PersonCompany = e.Company,
                    PersonTitle = e.Title,
                    AmountYears = e.Years,
                    TitleDescription = e.Description
                }).ToList()
            };


        }

        public async Task<PersonCreateDto> ChangeUserById(int id, PersonCreateDto person)
        {
            var validationsResults = ValidatorHelper.ValidateInput(person);
            if (validationsResults.Count > 0)
            {
                var error = string.Join(", ", validationsResults.Select(v => v.ErrorMessage));
                throw new ArgumentException($"Invalid format in data: {error}");
            }


            var personToChange = await context.Persons.FirstOrDefaultAsync(e => e.Id == id);

            if (personToChange == null)
            {
                throw new KeyNotFoundException("Person not found");
            }


            personToChange.Name = person.PersonName;
            personToChange.Phone = person.PersonPhone;
            personToChange.Email = person.PersonEmail;
            personToChange.Description = person.PersonDescription;

            await context.SaveChangesAsync();
            return person;

        }

        public async Task<PersonCreateDto> CreatePerson(PersonCreateDto person)
        {
            var validationsResults = ValidatorHelper.ValidateInput(person);
            if (validationsResults.Count > 0)
            {
                var error = string.Join(", ", validationsResults.Select(v => v.ErrorMessage));
                throw new ArgumentException($"Invalid format in data: {error}");
            }
            var personToCreate = new Person
            {
                Name = person.PersonName,
                Phone = person.PersonPhone,
                Email = person.PersonEmail,
                Description = person.PersonDescription
            };
            context.Persons.Add(personToCreate);
            await context.SaveChangesAsync();
            return person;
        }
    }
}
