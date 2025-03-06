using Microsoft.EntityFrameworkCore;
using RestApiCvManager.Data;
using RestApiCvManager.Dtos.EducationDTOs;
using RestApiCvManager.Dtos.ExperienceDTOs;
using RestApiCvManager.Models;
using RestApiCvManager.Services;

namespace RestApiCvManager.Endpoints.EducationEndpoints.cs
{
    public class EducationEndpoints
    {
        public static void RegisterEducationEndpoints(WebApplication app)
        {

            app.MapPost("/education/{personId}", async (CvManagerDbContext context, int personId, EducationDto newEducation) =>
            {

                var personToFind = await context.Persons
                .Include(p => p.Educations)
                .FirstOrDefaultAsync(p => p.Id == personId);

                if (personToFind == null)
                {
                    return Results.NotFound();
                }

               var validationsResults = ValidatorHelper.ValidateInput(newEducation);
                if (validationsResults.Count > 0)
                {
                    return Results.BadRequest(validationsResults);
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
                return Results.Ok(newEducation);

            });

            app.MapPut("education/{id}", async (CvManagerDbContext context, int id, EducationDto education) =>
            {
                var validationsResults = ValidatorHelper.ValidateInput(education);
                if (validationsResults.Count > 0)
                {
                    return Results.BadRequest(validationsResults);
                }


                var educationToChange = await context.Educations.FirstOrDefaultAsync(e => e.Id == id);

                if (educationToChange == null)
                {
                    return Results.NotFound();
                }
                educationToChange.School = education.SchoolDegree;
                educationToChange.Degree = education.SchoolDegree;
                educationToChange.StartDate = education.SchoolStartDate;
                educationToChange.EndDate = education.SchoolEndDate;

                await context.SaveChangesAsync();

                return Results.Ok(education);

            });

            app.MapDelete("education/{id}", async (CvManagerDbContext context, int id) =>
            {
                var educationToDelete = await context.Educations.FirstOrDefaultAsync(e => e.Id == id);
                if (educationToDelete == null)
                {
                    return Results.NotFound();
                }
                context.Educations.Remove(educationToDelete);
                await context.SaveChangesAsync();
                return Results.Ok($"You have deleted education with id {id}");
            });

            app.MapGet("education/{id}", async (CvManagerDbContext context, int id) =>
            {
                var education = await context.Educations.FirstOrDefaultAsync(e => e.Id == id);
                if (education == null)
                {
                    return Results.NotFound();
                }
                var educationFound = new EducationDto
                {
                    PersonSchool = education.School,
                    SchoolDegree = education.Degree,
                    SchoolStartDate = education.StartDate,
                    SchoolEndDate = education.EndDate
                };
                return Results.Ok(educationFound);
            });
        }
    }
}
