using Microsoft.EntityFrameworkCore;
using RestApiCvManager.Data;
using RestApiCvManager.Dtos.ExperienceDTOs;
using RestApiCvManager.Models;
using RestApiCvManager.Services;
using System.ComponentModel.DataAnnotations;

namespace RestApiCvManager.Endpoints.ExperienceEndpoints
{
    public class ExperienceEndpoints
    {
        public static void RegisterExperienceEndpoints(WebApplication app)
        {

            app.MapPost("/experience/{personId}", async (CvManagerDbContext context, int personId, ExperienceDto newExperience) =>
            {

                var validationsResults = ValidatorHelper.ValidateInput(newExperience);
                if (validationsResults.Count > 0)
                {
                    return Results.BadRequest(validationsResults);
                }

                var personToFind = await context.Persons
                .Include(p => p.Experiences)
                .FirstOrDefaultAsync(p => p.Id == personId);

                if (personToFind == null)
                {
                    return Results.NotFound();
                }
                var experience = new Experience
                {
                    Company = newExperience.PersonCompany,
                    Title = newExperience.PersonTitle,
                    Years = newExperience.AmountYears,
                    Description = newExperience.TitleDescription,
                    Person = personToFind

                };
                personToFind.Experiences.Add(experience);
                await context.SaveChangesAsync();
                return Results.Ok(newExperience);



            });

            app.MapPut("experience/{id}", async (CvManagerDbContext context, int id, ExperienceDto experience) =>
            {
                var validationsResults = ValidatorHelper.ValidateInput(experience);
                if (validationsResults.Count > 0)
                {
                    return Results.BadRequest(validationsResults);
                }


                var experienceToChange = await context.Experiences.FirstOrDefaultAsync(e => e.Id == id);

                if (experienceToChange == null)
                {
                    return Results.NotFound();
                }
                experienceToChange.Company = experience.PersonCompany;
                experienceToChange.Title = experience.PersonTitle;
                experienceToChange.Years = experience.AmountYears;
                experienceToChange.Description = experience.TitleDescription;

                await context.SaveChangesAsync();

                return Results.Ok(experience);

            });

            app.MapDelete("experience/{id}", async (CvManagerDbContext context, int id) =>
            {
                var experienceToDelete = await context.Experiences.FirstOrDefaultAsync(e => e.Id == id);
                if (experienceToDelete == null)
                {
                    return Results.NotFound();
                }
                context.Experiences.Remove(experienceToDelete);
                await context.SaveChangesAsync();
                return Results.Ok($"You have deleted experience with id {id}");
            });

            app.MapGet("experience/{id}", async (CvManagerDbContext context, int id) => 
            {
                var experience = context.Experiences.FindAsync(id);
                if (experience == null)
                {
                    return Results.NotFound();
                }

                var experienceFound = new ExperienceDto
                {
                    PersonCompany = experience.Result.Company,
                    PersonTitle = experience.Result.Title,
                    AmountYears = experience.Result.Years,
                    TitleDescription = experience.Result.Description

                };

                return Results.Ok(experienceFound);
            });
        }

       
    }


}
