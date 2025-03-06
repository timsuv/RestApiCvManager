using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiCvManager.Data;
using RestApiCvManager.Dtos.EducationDTOs;
using RestApiCvManager.Dtos.ExperienceDTOs;
using RestApiCvManager.Dtos.PersonDTOs;
using RestApiCvManager.Models;
using RestApiCvManager.Services;
using System;

namespace RestApiCvManager.Endpoints.PersonEndpoints
{
    public class PersonEndpoints
    {
        public static void RegisterPersonEndpoints(WebApplication app)
        {
            app.MapGet("/persons", async (CvManagerDbContext context) =>
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

                return Results.Ok(persons);
            });

            app.MapGet("/person/{id}", async (CvManagerDbContext context, int id) =>
            {
                var person = await context.Persons
               .Include(p => p.Educations)
               .Include(p => p.Experiences)
               .FirstOrDefaultAsync(p => p.Id == id);


                if (person == null)
                {
                    return Results.NotFound();
                }
                var personDTO = new PersonDto
                {
                    PersonName = person.Name,
                    PersonEmail = person.Email,
                    PersonPhone = person.Phone,
                    PersonDescription = person.Description,
                    Educations = person.Educations.Select(
                        e => new EducationDto
                        {
                            PersonSchool = e.School,
                            SchoolDegree = e.Degree,
                            SchoolStartDate = e.StartDate,
                            SchoolEndDate = e.EndDate
                        }).ToList(),
                    Experiences = person.Experiences.Select(
                        e => new ExperienceDto
                        {
                            PersonCompany = e.Company,
                            PersonTitle = e.Title,
                            AmountYears = e.Years,
                            TitleDescription = e.Description
                        }).ToList()
                };

                return Results.Ok(personDTO);

            });

            app.MapPut("person/{id}", async (CvManagerDbContext context, int id, PersonDto person) =>
            {
                var validationsResults = ValidatorHelper.ValidateInput(person);
                if (validationsResults.Count > 0)
                {
                    return Results.BadRequest(validationsResults);
                }


                var personToChange = await context.Persons.FirstOrDefaultAsync(e => e.Id == id);

                if (personToChange == null)
                {
                    return Results.NotFound();
                }
                personToChange.Name = person.PersonName;
                personToChange.Phone = person.PersonPhone;
                personToChange.Email = person.PersonEmail;
                personToChange.Description = person.PersonDescription;

                await context.SaveChangesAsync();

                return Results.Ok(person);

            });


        }


    }
}
