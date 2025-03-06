using RestApiCvManager.Services;

namespace RestApiCvManager.Endpoints.GithubEndpoints
{
    public class GithubEndpoints
    {
        public static async void RegisterGithubEndpoints(WebApplication app)
        {
            app.MapGet("/github/{username}", async (GithubService service, string username) =>
            {
                try
                {
                    var github = await service.GetRepos(username);
                    return Results.Ok(github);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
            });
        }
    }
}
