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
            app.MapGet("/persons", async (PersonService useService) =>
            {
                try
                {
                    var persons = await useService.GetAllUsers();
                    return Results.Ok(persons);

                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound("No persons found");
                }
            });

            app.MapGet("/person/{id}", async (PersonService useService, int id) =>
            {
                try
                {
                    var person = await useService.GetUserById(id);
                    return Results.Ok(person);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound("Person not found");
                }
            });

            app.MapPut("person/{id}", async (PersonService userService, int id, PersonCreateDto person) =>
            {
                try
                {
                    var personToChange = await userService.ChangeUserById(id, person);
                    return Results.Ok(personToChange);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound("Person not found");
                }
            });

            app.MapPost("/person", async (PersonService service, PersonCreateDto personToCreate) =>
            {
                try
                {
                    var person = await service.CreatePerson(personToCreate);
                    return Results.Ok(personToCreate);
                }
                catch(ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });


        }


    }
}
