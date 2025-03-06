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

            app.MapPost("/education/{personId}", async (EducationService useService, int personId, EducationDto newEducation) =>
            {
                try
                {
                    var education = await useService.AddEducation(personId, newEducation);
                    return Results.Ok(newEducation);
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

            app.MapPut("education/{id}", async (EducationService useService, int id, EducationDto education) =>
            {
                try
                {
                    var updatedEducation = await useService.ChangeEducationById(id, education);
                    return Results.Ok(updatedEducation);
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

            app.MapDelete("education/{id}", async (EducationService useService, int id) =>
            {
                try
                {
                    var deletedEducation = await useService.DeleteEducation(id);
                    return Results.Ok(deletedEducation);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }

            });

            app.MapGet("education/{id}", async (EducationService useService, int id) =>
            {
                try
                {
                    var education = await useService.GetEducationById(id);
                    return Results.Ok(education);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });
        }
    }
}
