using RestApiCvManager.Dtos.GithubDTOs;
using System.Text.Json;

namespace RestApiCvManager.Services
{
    public class GithubService
    {
        private readonly HttpClient HttpClient;
        public GithubService(HttpClient _httpClient)
        {
            HttpClient = _httpClient;
        }

        public async Task<List<GithubDto>> GetRepos(string username)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/users/{username}/repos");
            request.Headers.Add("User-Agent", "RestApiCvManager");


            var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new KeyNotFoundException($"User not found or request failed: {errorMessage}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var data = JsonSerializer.Deserialize<List<GithubDto>>(json, options);

            if (data != null)
            {
                foreach (var item in data)
                {
                    if (item.Language == null)
                    {
                        item.Language = "No language";
                    }
                    if (item.Description == null)
                    {
                        item.Description = "No description";
                    }
                }
            }


            return data;
        }
    }
}
