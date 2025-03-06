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

            app.MapPost("/experience/{personId}", async (ExperienceService service, int personId, ExperienceDto newExperience) =>
            {

                try
                {
                    var experience = await service.AddExperience(newExperience, personId);
                    return Results.Ok(newExperience);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }



            });

            app.MapPut("experience/{id}", async (ExperienceService service, int id, ExperienceDto experience) =>
            {
                try
                {
                    var updatedExperience = await service.ChangeExperienceById(id, experience);
                    return Results.Ok(updatedExperience);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }

            });

            app.MapDelete("experience/{id}", async (ExperienceService service, int id) =>
            {
                try
                {
                    var deletedExperience = await service.DeleteExperience(id);
                    return Results.Ok(deletedExperience);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }

            });

            app.MapGet("experience/{id}", async (ExperienceService service, int id) =>
            {
                try
                {
                    var experience = await service.GetExperienceById(id);
                    return Results.Ok(experience);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });
        }


    }


}
